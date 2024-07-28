using libplctag.DataTypes;
using libplctag.DataTypes.Simple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Xml;


namespace MqttPLCPublisher
{
   
    internal class Tagx: Elemento
    {
        public static Dictionary<String, PLC> PLCs;
        public Dictionary<String, Publish> Publishes;

        public PLC Plc;
        public String Direccion = "";
        public int Periodo = 100000;
        public int Valor = 0;
        public int Estado = 0;  // 1 = leído
        public int quality = 0;
        public DateTime timestamp;
        public bool Inicializado = false;
        TagDint t = new TagDint();
        System.Timers.Timer timer;


        public Tagx(XmlNode conf):base (conf) {
            /*foreach (XmlAttribute a in conf.Attributes)
            {
                if ("PLC".CompareTo(a.Name.ToUpper())==0) {
                    Plc = PLCs.GetValueOrDefault(a.Value.ToUpper());
                }
                {

                }
            */

                        if (conf.Attributes.GetNamedItem("PLC") != null)
            {
                Plc = PLCs.GetValueOrDefault(conf.Attributes.GetNamedItem("PLC").Value);
            }
            if (conf.Attributes.GetNamedItem("Period") != null)
            {
                Periodo = Int32.Parse(conf.Attributes.GetNamedItem("Period").Value);
            }
            if (conf.Attributes.GetNamedItem("Address") != null)
            {
                Direccion = conf.Attributes.GetNamedItem("Address").Value;
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

        public void leer()
        {
            
            if (Tipo == "DINT")
            {
                if (!Inicializado)
                {
                    try
                    {
                        t.Name = Direccion;
                        t.Gateway = Plc.Direccion;
                        t.Path = Plc.Ruta;
                        t.PlcType = libplctag.PlcType.ControlLogix;
                        t.Protocol = libplctag.Protocol.ab_eip;
                        t.Timeout = TimeSpan.FromSeconds(5);
                        //t.Timeout = new TimeSpan(450000000);
                        t.ReadCompleted += T_ReadCompleted;
                        t.Aborted += T_Aborted;
                        t.Destroyed += T_Destroyed;
                        t.ReadStarted += T_ReadStarted;
                        t.WriteCompleted += T_WriteCompleted;
                        t.WriteStarted += T_WriteStarted;
                        timer = new System.Timers.Timer(Periodo);
                        timer.Elapsed += timer_Elapsed;
                        t.Initialize();
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
                    t.ReadAsync(); //.ContinueWith(delegate { postleer(); } ).ContinueWith( delegate { timer.Start(); });
                } catch (Exception e)
                {
                    Console.WriteLine("Excepción " + e.ToString());
                    

                }

            }
            timer.Start();
        }

        private void T_WriteStarted(object? sender, libplctag.TagEventArgs e)
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
        }

        public void T_Aborted(object? sender, libplctag.TagEventArgs e)
        {
            Console.Write("- XX");
        }

        public void T_ReadCompleted(object? sender, libplctag.TagEventArgs e)
        {
            //((Tagx)sender).Valor = 2;
            try
            {
                var ValorAnterior = Valor;
                Console.Write("- "+ t.Value.ToString());
                Valor = t.Value;
                Estado = 1;
                quality = 1;
                timestamp = DateTime.Now;
                
                foreach (Publish p in Publishes.Values)
                {
                    if (!p.AlCambiar || Valor != ValorAnterior)
                    {
                        p.ejecutar();
                    }
                    
                }
            } catch
            {
                Console.WriteLine("------------");
            }
            //timer.Elapsed += timer_Elapsed;
            //timer.Start();
            //catch
            //throw new NotImplementedException();
            //Console.WriteLine("------------");
        }

        private async void timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            //timer.Stop();
            procesaMqtt();
            //throw new NotImplementedException();
            leer();
         }

        void procesaMqtt()
        {

        }


    }

    
}
