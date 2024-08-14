using MQTTnet.Protocol;
using MQTTnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using MQTTnet.Client;
using MQTTnet.Server;
using System.Text.Json;
using libplctag.DataTypes.Simple;
using libplctag;


namespace MqttPLCPublisher
{
    internal class Subscribe
    {
        public String Topico = "";
        public String Nombre = "";
        public Broker broker;

        public static Dictionary<String, Tagx> Tags;
        public static Dictionary<String, Broker> Brokers;
        public static Dictionary<String, Subscribe> Subscribes;

        public Tagx tag;

        public Subscribe(XmlNode conf)
        {
            Nombre = GetHashCode().ToString();
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
                    }
                    
                    else
                    {
                        Console.Write("Advertencia:  Atributo " + a.Name + " no reconocido en Subscribe");
                    }
                }
            }
        }

        public class Subscribe_Tag_class
        {
            public long Timestamp { get; set; }
            public Object Value { get; set; }
        }

        public class Subscribe_Tag_class_Bool
        {
            public long Timestamp { get; set; }
            public Boolean Value { get; set; }
        }

        public class Subscribe_Tag_class_Int
        {
            public long Timestamp { get; set; }
            public int Value { get; set; }
        }
        public class Subscribe_Tag_class_Int16
        {
            public long Timestamp { get; set; }
            public Int16 Value { get; set; }
        }
        public class Subscribe_Tag_class_Int64
        {
            public long Timestamp { get; set; }
            public Int64 Value { get; set; }
        }
        public class Subscribe_Tag_class_SByte
        {
            public long Timestamp { get; set; }
            public SByte Value { get; set; }
        }
        public class Subscribe_Tag_class_String
        {
            public long Timestamp { get; set; }
            public String Value { get; set; }
        }
        public class Subscribe_Tag_class_Double
        {
            public long Timestamp { get; set; }
            public double Value { get; set; }
        }

        public class Subscribe_Tag_class_Single
        {
            public long Timestamp { get; set; }
            public Single Value { get; set; }
        }

        public async void ejecutar()
        {
            if (broker.mqttClient != null)
            {
                if (broker.mqttClient.IsConnected == false)
                {
                    broker.conectar();
                }

                var mqttSubscribeOptions = broker.factory.CreateSubscribeOptionsBuilder()
                    .WithTopicFilter(Topico)
                .Build();

                broker.mqttClient.ApplicationMessageReceivedAsync += MessageRecive;

        
                var response = await broker.mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);


            }
        }
        public async Task MessageRecive(MqttApplicationMessageReceivedEventArgs e)
        {
            Tagx tag2 = Subscribes.GetValueOrDefault(e.ApplicationMessage.Topic).tag;
            if (tag2!=tag)
            {
                return;
            }
            try
            {
                var s = JsonSerializer.Deserialize<Subscribe_Tag_class>(Encoding.ASCII.GetString(e.ApplicationMessage.Payload));
                Console.Write("Tag: " + tag2.Nombre+"  ");
                Console.WriteLine("Valor: " + s.Value.ToString());

            } catch (Exception e1)
            {
                Console.WriteLine("Excepción: " + e1.ToString());
            }
            if (tag2!=null)
            {
                try
                {
                    if ("BOOL".CompareTo(tag2.Tipo) == 0)
                    {
                        var s = JsonSerializer.Deserialize<Subscribe_Tag_class_Bool>(Encoding.ASCII.GetString(e.ApplicationMessage.Payload));
                        tag2.lib_tag.Value = s.Value;
                    }
                    else if ("DINT".CompareTo(tag2.Tipo) == 0)
                    {
                        var s = JsonSerializer.Deserialize<Subscribe_Tag_class_Int>(Encoding.ASCII.GetString(e.ApplicationMessage.Payload));
                        tag2.lib_tag.Value= s.Value;
                        
                    }
                    else if ("INT".CompareTo(tag2.Tipo) == 0)
                    {
                        var s = JsonSerializer.Deserialize<Subscribe_Tag_class_Int16>(Encoding.ASCII.GetString(e.ApplicationMessage.Payload));
                        tag2.lib_tag.Value = s.Value;
                    }
                    else if ("SINT".CompareTo(tag2.Tipo) == 0)
                    {
                        var s = JsonSerializer.Deserialize<Subscribe_Tag_class_SByte>(Encoding.ASCII.GetString(e.ApplicationMessage.Payload));
                        tag2.lib_tag.Value = s.Value;
                    }
                    else if ("LINT".CompareTo(tag2.Tipo) == 0)
                    {
                        var s = JsonSerializer.Deserialize<Subscribe_Tag_class_Int64>(Encoding.ASCII.GetString(e.ApplicationMessage.Payload));
                        tag2.lib_tag.Value = s.Value;
                    }
                    else if ("REAL".CompareTo(tag2.Tipo) == 0)
                    {
                        var s = JsonSerializer.Deserialize<Subscribe_Tag_class_Single>(Encoding.ASCII.GetString(e.ApplicationMessage.Payload));
                        tag2.lib_tag.Value = s.Value;
                    }
                    else if ("LREAL".CompareTo(tag2.Tipo) == 0)
                    {
                        var s = JsonSerializer.Deserialize<Subscribe_Tag_class_Double>(Encoding.ASCII.GetString(e.ApplicationMessage.Payload));
                        tag2.lib_tag.Value = s.Value;
                    }
                    else if ("STRING".CompareTo(tag2.Tipo) == 0)
                    {
                        var s = JsonSerializer.Deserialize<Subscribe_Tag_class_String>(Encoding.ASCII.GetString(e.ApplicationMessage.Payload));
                        tag2.lib_tag.Value = s.Value;
                    }
                    else
                    {
                        Console.WriteLine("Error al procesar mensaje.");
                        return;
                    }
                        await tag2.lib_tag.WriteAsync();
                } catch (Exception e1)
                {
                    Console.WriteLine("Excepción: " + e1.ToString());
                }
                
            }
        }
        

    }
}
