using MQTTnet.Protocol;
using MQTTnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using libplctag.DataTypes.Simple;

namespace MqttPLCPublisher
{
    internal class Publish
    {
        public static Dictionary<String, Tagx> Tags;
        public static Dictionary<String, Broker> Brokers;
        
        public String Topico;
        public Broker broker;
        public Tagx tag;
        public Boolean AlCambiar=true;
        public Publish(XmlNode conf)
        {
            if (conf.Attributes != null)
            {
                foreach (XmlAttribute a in conf.Attributes)
                {
                    if ("TOPIC".CompareTo(a.Name.ToUpper()) == 0)
                    {
                        Topico = a.Value;
                    }
                    else if ("BROKER".CompareTo(a.Name.ToUpper()) == 0)
                    {
                        broker = Brokers.GetValueOrDefault(a.Value.ToUpper());
                    }
                    else if ("TAG".CompareTo(a.Name.ToUpper()) == 0)
                    {
                        tag = Tags.GetValueOrDefault(a.Value.ToUpper());
                    } else if ("ONCHANGE".CompareTo(a.Name.ToUpper()) == 0)
                    {
                        if ("TRUE".CompareTo(a.Value.ToUpper()) == 0)
                        {
                            AlCambiar = true;
                        }else
                        {
                            AlCambiar = false;
                        }
                    }

                    else
                    {
                        Console.Write("Advertencia:  Atributo " + a.Name + " no reconocido en Publish");
                    }
                }
            }
            if (tag != null)
            {
                tag.Publishes.Add("", this);
            }
            
        }
        public class Publish_Tag_class
        {
            public long Timestamp { get; set; }
            public object Value { get; set; }
        }
        String conv_a_Json()
        {
            Publish_Tag_class d = new Publish_Tag_class()
            {
                Timestamp = (long)(tag.timestamp.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds,
                Value = tag.Valor
            };
            return JsonSerializer.Serialize<Publish_Tag_class>(d); ;
        }

        async public void ejecutar()
        {
            if (broker.mqttClient != null)
                {
                if (broker.mqttClient.IsConnected == false)
                {
                    broker.conectar();

                }
                String s = conv_a_Json();

                var message = new MqttApplicationMessageBuilder()
                            .WithTopic(Topico)
                            .WithPayload(s)
                            .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                            .WithRetainFlag()
                            .Build();

                    try
                    {
                    await broker.mqttClient.PublishAsync(message);
                    
                }
                    catch (Exception e)
                    {
                    
                        
                    }


                }
            }

    }
}