using libplctag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MqttPLCPublisher
{
    internal class Elemento
    {
        public String Nombre = "";
        public String Tipo = "";
        public Elemento(XmlNode conf) {
            if (conf.Attributes.GetNamedItem("Name") != null)
            {
                Nombre = conf.Attributes.GetNamedItem("Name").Value;
            }
            if (conf.Attributes.GetNamedItem("Type") != null)
            {
                Tipo = conf.Attributes.GetNamedItem("Type").Value;
            }

        }
    }
}
