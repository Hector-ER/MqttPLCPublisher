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
        public PLC(XmlNode conf) {
            if (conf.Attributes.GetNamedItem("Name") != null)
            {
                Nombre = conf.Attributes.GetNamedItem("Name").Value;
            }
            if (conf.Attributes.GetNamedItem("Protocol") != null)
            {
                Protocolo = conf.Attributes.GetNamedItem("Protocol").Value;
            }
            if (conf.Attributes.GetNamedItem("Type") != null)
            {
                Tipo = conf.Attributes.GetNamedItem("Type").Value;
            }
            if (conf.Attributes.GetNamedItem("Address") != null)
            {
                Direccion = conf.Attributes.GetNamedItem("Address").Value;
            }
            if (conf.Attributes.GetNamedItem("Path") != null)
            {
                Ruta = conf.Attributes.GetNamedItem("Path").Value;
            }
            if (conf.Attributes.GetNamedItem("Timeout") != null)
            {
                TimeOut = Int32.Parse(conf.Attributes.GetNamedItem("Timeout").Value);
            }

        }
        //Console.WriteLine("Cargando... PLC = " + Nombre);

    }
}
