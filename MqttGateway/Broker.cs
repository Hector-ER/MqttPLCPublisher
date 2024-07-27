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

        public IMqttClient? mqttClient;

        public Broker(XmlNode conf)
        {
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
            }
        }

        public async void conectar()
        {
            string broker = Nombre; //   "BA3490";
            int port = Puerto; // 1883;
            string clientId = Guid.NewGuid().ToString();
            //string topic = "Csharp/mqtt";
            //string username = "emqxtest";
            //string password = "******";

            // Create a MQTT client factory
            var factory = new MqttFactory();

            // Create a MQTT client instance
            mqttClient = factory.CreateMqttClient();

            // Create MQTT client options
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer(broker, port) // MQTT broker address and port
                                             //.WithCredentials(username, password) // Set username and password
                .WithClientId(clientId)
                .WithCleanSession()
                .Build();
            var connectResult = mqttClient.ConnectAsync(options);

            System.Threading.Thread.Sleep(10000);

        }
    }
}