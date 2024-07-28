using MQTTnet.Protocol;
using MQTTnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MqttPLCPublisher
{
    internal class Publish
    {
        public static Dictionary<String, Tagx> Tags;
        public static Dictionary<String, Broker> Brokers;
        
        public String Topico;
        public Broker broker;
        public Tagx tag;
        public Publish(XmlNode conf)
        {
            if (conf.Attributes.GetNamedItem("Topic") != null)
            {
                Topico = conf.Attributes.GetNamedItem("Topic").Value;
            }
            if (conf.Attributes.GetNamedItem("Broker") != null)
            {
                broker = Brokers.GetValueOrDefault(conf.Attributes.GetNamedItem("Broker").Value);
            }
            if (conf.Attributes.GetNamedItem("Tag") != null)
            {
                tag = Tags.GetValueOrDefault(conf.Attributes.GetNamedItem("Tag").Value);
            }
            if (tag != null)
            {
                tag.Publishes.Add("", this);
            }
        }
        async public void ejecutar()
        {
            if (broker.mqttClient != null)
                {

                String s = $"\"id\": \"" + tag.Nombre + "\", \"v\": \"" + tag.Valor + "\", \"q\": 0 \"t\": 0";
                    for (int i = 0; i < 10; i++)
                    {
                        var message = new MqttApplicationMessageBuilder()
                            .WithTopic(Topico)
                            .WithPayload(s)
                            .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                            .WithRetainFlag()
                            .Build();

                        await broker.mqttClient.PublishAsync(message);
                        await Task.Delay(1000); // Wait for 1 second
                    }


                }
            }

    }
}