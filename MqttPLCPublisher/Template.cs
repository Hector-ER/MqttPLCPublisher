using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MqttPLCPublisher
{
    internal class Template
    {
        public String Nombre = "";

        public Template(XmlNode conf) {
            if (conf.Attributes != null)
            {
                foreach (XmlAttribute a in conf.Attributes)
                {
                    if ("NAME".CompareTo(a.Name.ToUpper()) == 0)
                    {
                        Nombre = a.Value.ToUpper();
                    }
                    else
                    {
                        Console.Write("Advertencia:  Atributo " + a.Name + " no reconocido");
                        if (Nombre.Length != 0)
                        {
                            Console.WriteLine(" en Template: " + Nombre + ".");
                        }
                        else
                        {
                            Console.WriteLine(" en Template.");

                        }
                    }
                }
                XmlNode IN = conf.FirstChild;


            }

        }
    }
}
