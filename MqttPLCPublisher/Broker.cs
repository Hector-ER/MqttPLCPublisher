using MQTTnet.Client;
using MQTTnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MqttPLCPublisher
{
    internal class Broker
    {
        public String Nombre = "";
        public String Direccion = "";
        public int Puerto = 0;
        public String Usuario = "";
        public String Password = "";

        public IMqttClient? mqttClient;

        public MqttFactory factory;

        public Broker(XmlNode conf)
        {
            if (conf.Attributes != null)
            {
                foreach (XmlAttribute a in conf.Attributes)
                {
                    if ("NAME".CompareTo(a.Name.ToUpper()) == 0)
                    {
                        Nombre = a.Value.ToUpper();
                    }
                    else if ("ADDRESS".CompareTo(a.Name.ToUpper()) == 0)
                    {
                        Direccion = a.Value.ToUpper();
                    }
                    else if ("PORT".CompareTo(a.Name.ToUpper()) == 0)
                    {
                        Puerto = Int32.Parse(a.Value);
                    }
                    else if ("USER".CompareTo(a.Name.ToUpper()) == 0)
                    {
                        Usuario = a.Value.ToUpper();
                    }
                    else if ("PASSWORD".CompareTo(a.Name.ToUpper()) == 0)
                    {
                        Password = a.Value.ToUpper();
                    }

                }
            }

            /*
             if (conf.Attributes.GetNamedItem("Name") != null)
            {
                Nombre = conf.Attributes.GetNamedItem("Name").Value;
            }
            if (conf.Attributes.GetNamedItem("Address") != null)
            {
                Direccion = conf.Attributes.GetNamedItem("Address").Value;
            }
            if (conf.Attributes.GetNamedItem("Port") != null)
            {
                Puerto = Int32.Parse(conf.Attributes.GetNamedItem("Port").Value);
            }*/
        }

        public async void conectar()
        {
            string broker = Nombre; //   "BA3490";
            int port = Puerto; // 1883;
            string clientId = Guid.NewGuid().ToString();
            //string topic = "Csharp/mqtt";
            string username = Usuario; // "emqxtest";
            string password = Password; // "******";

            // Create a MQTT client factory
            factory = new MqttFactory();

            // Create a MQTT client instance
            mqttClient = factory.CreateMqttClient();

            // Create MQTT client options
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer(broker, port) // MQTT broker address and port
                .WithCredentials(username, password) // Set username and password
                .WithClientId(clientId)
                .WithCleanSession()
                .Build();
            //var connectResult = await mqttClient.ConnectAsync(options);
            var t = mqttClient.ConnectAsync(options);
            DateTime ti = DateTime.Now;
            while ((DateTime.Now - ti).TotalSeconds < 50) 
            {
                if (mqttClient.IsConnected) { break; }
            }
            //System.Threading.Thread.Sleep(10000);

        }
    }
}