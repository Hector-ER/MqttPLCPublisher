using libplctag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


namespace MqttPLCPublisher
{
    internal class PLC
    {
        public String Nombre = "";
        public String Protocolo = "";
        public String Tipo = "";
        public String Direccion = "";
        public String Ruta = "";
        public int TimeOut = 1000;
        public libplctag.Protocol lib_protocol = libplctag.Protocol.ab_eip;
        public libplctag.PlcType lib_type = libplctag.PlcType.ControlLogix;

        public PLC(XmlNode conf) {
            if (conf.Attributes != null)
            {
                foreach (XmlAttribute a in conf.Attributes)
                {
                    if ("NAME".CompareTo(a.Name.ToUpper()) == 0)
                    {
                        Nombre = a.Value.ToUpper();
                    }
                    else if ("PROTOCOL".CompareTo(a.Name.ToUpper()) == 0)
                    {
                        Protocolo = a.Value.ToUpper();
                    }
                    else if ("TYPE".CompareTo(a.Name.ToUpper()) == 0)
                    {
                        Tipo = a.Value.ToUpper();
                    }
                    else if ("ADDRESS".CompareTo(a.Name.ToUpper()) == 0)
                    {
                        Direccion = a.Value.ToUpper();
                    }
                    else if ("PATH".CompareTo(a.Name.ToUpper()) == 0)
                    {
                        Ruta = a.Value.ToUpper();
                    }
                    else if ("TIMEOUT".CompareTo(a.Name.ToUpper()) == 0)
                    {
                        TimeOut = Int32.Parse(a.Value.ToUpper());
                    } else
                    {
                        Console.Write("Advertencia:  Atributo " + a.Name + " no reconocido");
                        if (Nombre.Length != 0)
                        {
                            Console.WriteLine(" en PLC: " + Nombre + ".");
                        }
                        else
                        {
                            Console.WriteLine(".");
                        }
                    }
                }
            }

            if ("AB_EIP".CompareTo(Protocolo) == 0)
            {
                lib_protocol = Protocol.ab_eip;
            }
            else if ("MODBUS_TCP".CompareTo(Protocolo) == 0)
            {
                lib_protocol = Protocol.modbus_tcp;
            } else
            {
                Console.Write("Error:  Protocolo " + Protocolo + " no reconocido");
                if (Nombre.Length > 0)
                {
                    Console.WriteLine(" en PLC " + Nombre);
                }
                else
                {
                    Console.WriteLine(".");
                }
            }

            if ("CONTROLLOGIX".CompareTo(Tipo) == 0)
            {
                lib_type = libplctag.PlcType.ControlLogix;
            } else if ("LOGIXPCCC".CompareTo(Tipo) == 0)
            {
                lib_type = libplctag.PlcType.LogixPccc;
            }
            else if ("MICRO800".CompareTo(Tipo) == 0)
            {
                lib_type = libplctag.PlcType.Micro800;
            }
            else if ("MICROLOGIX".CompareTo(Tipo) == 0)
            {
                lib_type = libplctag.PlcType.MicroLogix;
            }
            else if ("OMRON".CompareTo(Tipo) == 0)
            {
                lib_type = libplctag.PlcType.Omron;
            }
            else if ("PLC5".CompareTo(Tipo) == 0)
            {
                lib_type = libplctag.PlcType.Plc5;
            }
            else if ("SLC500".CompareTo(Tipo) == 0)
            {
                lib_type = libplctag.PlcType.Slc500;
            } else
            {
                Console.Write("Error:  Tipo de PLC " + Tipo + " no reconocido");
                if (Nombre.Length >0 )
                {
                    Console.WriteLine(" en PLC " + Nombre);
                } else
                {
                    Console.WriteLine(".");
                }
            }

        }
        
    }
}
