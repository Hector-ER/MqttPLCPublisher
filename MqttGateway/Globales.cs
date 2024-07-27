using MQTTnet.Client;
using MQTTnet;
using libplctag;
using MQTTnet.Protocol;
using MQTTnet.Server;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics;
using System.Runtime.Intrinsics.X86;
using MQTTnet.Channel;

namespace MqttPLCPublisher
{
    internal class Globales
    {
        Broker _broker;
        public Globales(Broker b)
        {
            _broker = b; 
        }

        IMqttClient? mqttClient;
        public async void conectarAlBroaker()
        {
            string broker = _broker.Nombre; //   "BA3490";
            int port = _broker.Puerto; // 1883;
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
            var connectResult =  mqttClient.ConnectAsync(options);

            System.Threading.Thread.Sleep(10000);

        }

        public async void publish()
        {
            if (mqttClient != null)
            {

                for (int i = 0; i < 10; i++)
                {
                    var message = new MqttApplicationMessageBuilder()
                        .WithTopic("MqttGateway/Test")
                        .WithPayload($"Hello, MQTT! Message number {i}")
                        .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                        .WithRetainFlag()
                        .Build();

                    await mqttClient.PublishAsync(message);
                    await Task.Delay(1000); // Wait for 1 second
                }


            }




        }

    }
}
    
