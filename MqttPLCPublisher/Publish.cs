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
        public Boolean AlCambiar;
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
                    /*
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
                tag = Tags.GetValueOrDefault(conf.Attributes.GetNamedItem("Tag").Value.ToUpper());
            }*/
            if (tag != null)
            {
                tag.Publishes.Add("", this);
            }
            //AlCambiar = false;
        }
        public class Publish_Tag_class
        {
            public long Timestamp { get; set; }
            public object Value { get; set; }
        }
        String conv_a_Json()
        {
            /*if ("BOOL".CompareTo(Tipo) == 0)
            {
                lib_tag = new TagBool();

            }
            else if ("DINT".CompareTo(Tipo) == 0)
            {
                lib_tag = new TagDint();
            }
            else if ("INT".CompareTo(Tipo) == 0)
            {
                lib_tag = new TagInt();
            }
            else if ("LINT".CompareTo(Tipo) == 0)
            {
                lib_tag = new TagLint();
            }
            else if ("LREAL".CompareTo(Tipo) == 0)
            {
                lib_tag = new TagLreal();
            }
            else
            {
                if ("".CompareTo(Tipo) != 0)
                {
                    Console.WriteLine("Error: Tipo " + Tipo + " no reconocido.");
                }
                else
                {
                    Console.WriteLine("Error: Tag \"" + Plc.Nombre + "\\\"" + Nombre + " sin tipo.");
                }
            }*/

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
                //Publish_Dint d = new Publish_Dint();
                //d.Timestamp = (long) (tag.timestamp.ToUniversalTime()- new DateTime(1970,1,1)).TotalSeconds;

                //d.Value = (int) tag.Valor;

                //String s3 = JsonSerializer.Serialize<Publish_Dint>(d);
                String s3 = conv_a_Json();

                String s = $"\"id\": \"" + tag.Nombre + "\", \"v\": \"" + tag.Valor + "\", \"q\": \""+tag.quality+"\", \"t\": \""+tag.timestamp+"\"";
                String s2 = "{ \"timestamp\" : \"+"+ DateTime.Now.ToString("yy/MM/dd HH:mm:ss")+"\", \"values\" : {"+s+" } }";

                    //for (int i = 0; i < 10; i++)
                    //{
                        var message = new MqttApplicationMessageBuilder()
                            .WithTopic(Topico)
                            .WithPayload(s3)
                            .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                            .WithRetainFlag()
                            .Build();

                    try
                    {
                    await broker.mqttClient.PublishAsync(message);
                    //await Task.Delay(1000); // Wait for 1 second
                }
                    catch (Exception e)
                    {
                    
                        
                    }


                }
            }

    }
}