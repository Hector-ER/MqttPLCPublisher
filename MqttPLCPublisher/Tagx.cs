using libplctag;
using libplctag.DataTypes;
using libplctag.DataTypes.Simple;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Xml;


namespace MqttPLCPublisher
{
   
    internal class Tagx
    {
        public static Dictionary<String, PLC> PLCs;
        public Dictionary<String, Publish> Publishes;

        public PLC Plc;
        public String Direccion = "";
        public int Periodo = 100000;
        public Object Valor;
        public int Estado = 0;  // 1 = leído
        public int quality = 0;
        public DateTime timestamp;
        public bool Inicializado = false;
        System.Timers.Timer timer;
        public String Nombre = "";
        public String Tipo = "";
        public String NombrePLC = "";
        
        ITag lib_tag; 


        public Tagx(XmlNode conf) {
            if (conf.Attributes != null)
            {
                foreach (XmlAttribute a in conf.Attributes)
                {
                    if ("NAME".CompareTo(a.Name.ToUpper()) == 0)
                    {
                        Nombre = a.Value.ToUpper();
                    }
                    else if ("TYPE".CompareTo(a.Name.ToUpper()) == 0)
                    {
                        Tipo = a.Value.ToUpper();
                 
                    }
                    else if ("PLC".CompareTo(a.Name.ToUpper()) == 0)
                    {
                        NombrePLC = a.Value.ToUpper();
                        Plc = PLCs.GetValueOrDefault(a.Value.ToUpper());
                    }
                    else if ("PERIOD".CompareTo(a.Name.ToUpper()) == 0)
                    {
                        Periodo = Int32.Parse(a.Value);
                    }
                    else if ("ADDRESS".CompareTo(a.Name.ToUpper()) == 0)
                    {
                        Direccion = a.Value;
                    }
                    else
                    {
                        Console.Write("Advertencia:  Atributo " + a.Name + " no reconocido");
                        if (Nombre.Length != 0)
                        {
                            Console.WriteLine(" en Tag: " + Nombre + ".");
                        }
                        else
                        {
                            Console.WriteLine(" en Tag.");

                        }
                    }
                }
            }

            if ("BOOL".CompareTo(Tipo) == 0)
            {
                lib_tag = new TagBool();

            }
            else if ("DINT".CompareTo(Tipo) == 0)
            {
                lib_tag = new TagDint();
            }
            else if ("INT".CompareTo(Tipo) == 0)
            {
                lib_tag = new TagInt();
            }
            else if ("LINT".CompareTo(Tipo) == 0)
            {
                lib_tag = new TagLint();
            }
            else if ("LREAL".CompareTo(Tipo) == 0)
            {
                lib_tag = new TagLreal();
            }
            else
            {
                if ("".CompareTo(Tipo) != 0)
                {
                    Console.WriteLine("Error: Tipo " + Tipo + " no reconocido.");
                } else {
                    Console.WriteLine("Error: Tag \"" + Plc.Nombre + "\\\"" + Nombre+" sin tipo.");
                }
            }
            

            XmlNodeList nl = conf.ChildNodes;
            foreach (XmlNode n in nl)
            {
                if (n.Name == "PLC")
                {
                    PLC p = new PLC(n);
                    PLCs.Add("", p);
                    Plc = p;
                }
            }
            if (Direccion == "")
            {
                Direccion = Nombre;
            }

            Publishes = new Dictionary<string, Publish>();

        }

        async public void leer()
        {
            
            if (lib_tag != null)
            {
                if (!Inicializado)
                {
                    try
                    {
                        lib_tag.Name = Direccion;
                        lib_tag.Gateway = Plc.Direccion;
                        lib_tag.Path = Plc.Ruta;
                        lib_tag.PlcType = Plc.lib_type;
                        lib_tag.Protocol = Plc.lib_protocol;
                        lib_tag.Timeout = TimeSpan.FromMilliseconds(Plc.TimeOut);
                        lib_tag.ReadCompleted += T_ReadCompleted;
                        //lib_tag.Aborted += T_Aborted;
                        //lib_tag.Destroyed += T_Destroyed;
                        //lib_tag.ReadStarted += T_ReadStarted;
                        //lib_tag.WriteCompleted += T_WriteCompleted;
                        //lib_tag.WriteStarted += T_WriteStarted;
                        timer = new System.Timers.Timer(Periodo);
                        timer.Elapsed += timer_Elapsed;
                        await lib_tag.InitializeAsync();
                        Inicializado = true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error: " + e);
                    }
                    timer.AutoReset = false;
                    
                }
                Estado = 0;
                try
                {
                    _ = await lib_tag.ReadAsync(); 
                } catch (Exception e)
                {
                    Console.WriteLine("Excepción " + e.ToString());
                }

            }
            timer.Start();
        }

        /*private void T_WriteStarted(object? sender, libplctag.TagEventArgs e)
        {
            throw new NotImplementedException();
        }
        private void T_Destroyed(object? sender, libplctag.TagEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void T_WriteCompleted(object? sender, libplctag.TagEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void T_ReadStarted(object? sender, libplctag.TagEventArgs e)
        {
            //((Tagx) sender).Valor = 1;
            //throw new NotImplementedException();
        } */

        public void T_Aborted(object? sender, libplctag.TagEventArgs e)
        {
            Console.Write("- XX");
            Estado = 2;  // Error
        }

        public void T_ReadCompleted(object? sender, libplctag.TagEventArgs e)
        {
            //((Tagx)sender).Valor = 2;
            try
            {
                var ValorAnterior = Valor;
                Console.Write("- "+ lib_tag.Value.ToString());
                Valor = lib_tag.Value;
                Estado = 1; // Lectura Ok
                quality = 1;
                timestamp = DateTime.Now;
                
                foreach (Publish p in Publishes.Values)
                {
                    if (!p.AlCambiar || !Valor.Equals(ValorAnterior))
                    {
                        p.ejecutar();
                    }
               
                }
            } catch (Exception e2)
            {
                    Console.WriteLine("Error: " + e2.ToString());
            }
            //timer.Elapsed += timer_Elapsed;
            //timer.Start();
            //catch
            //throw new NotImplementedException();
            //Console.WriteLine("------------");
        }

        public async void timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            leer();
         }

    }

    
}
