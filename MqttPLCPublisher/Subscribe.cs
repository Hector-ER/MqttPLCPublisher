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
        //static MqttTopicTemplate sampleTemplate = new MqttTopicTemplate("mqttnet/samples/topic/{id} {valor}");

        public String Topico = "";
        public Broker broker;

        public static Dictionary<String, Tagx> Tags;
        public static Dictionary<String, Broker> Brokers;

        public Tagx tag;

        public Subscribe(XmlNode conf)
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
        }

        public class Subscribe_Tag_class
        {
            /*public Subscribe_Tag_class(ITag t)
            {
                Value = t;
            }*/
            public long Timestamp { get; set; }
            public Object Value { get; set; }
        }

        public class Subscribe_Tag_class_DINT
        {
            /*public Subscribe_Tag_class(ITag t)
            {
                Value = t;
            }*/
            public long Timestamp { get; set; }
            public int Value { get; set; }
        }


        public async void ejecutar()
        {
            if (broker.mqttClient != null)
            {
                if (broker.mqttClient.IsConnected == false)
                {
                    broker.conectar();
                }

                //var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer(Topico).Build();

                //await broker.mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

                var mqttSubscribeOptions = broker.factory.CreateSubscribeOptionsBuilder()
                    .WithTopicFilter(Topico)
                    
          //          .WithTopicTemplate(sampleTemplate.WithParameter("id", "1"))
                .Build();

                broker.mqttClient.ApplicationMessageReceivedAsync += MessageRecive;

                    // += e =>
                /*{
                    Console.WriteLine("Received application message.");
                    //e.DumpToConsole();
                    Console.WriteLine(e.ToString());


                    return Task.CompletedTask;
                };*/

                var response = await broker.mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);


                /*   String s = $"\"id\": \"" + tag.Nombre + "\", \"v\": \"" + tag.Valor + "\", \"q\": \"" + tag.quality + "\", \"t\": \"" + tag.timestamp + "\"";
                   String s2 = "{ \"timestamp\" : \"+" + DateTime.Now.ToString("yy/MM/dd HH:mm:ss") + "\", \"values\" : {" + s + " } }";

                   //for (int i = 0; i < 10; i++)
                   //{
                   var message = new MqttApplicationMessageBuilder()
                       .WithTopic(Topico)
                       .WithPayload(s2)
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
                */

            }
        }
        public async Task MessageRecive(MqttApplicationMessageReceivedEventArgs e)
        {
            //Console.WriteLine(e.ToString());
            //var s = new Subscribe_Tag_class();
            Console.WriteLine(Encoding.ASCII.GetString(e.ApplicationMessage.Payload));
            //Subscribe_Tag_class s=new Subscribe_Tag_class(new TagDint());
            Subscribe_Tag_class_DINT s = null;
            try
            {
                s = JsonSerializer.Deserialize<Subscribe_Tag_class_DINT>(Encoding.ASCII.GetString(e.ApplicationMessage.Payload));
                Console.WriteLine("Valor: " + s.Value.ToString());

            } catch (Exception e1)
            {
                Console.WriteLine("Excepción: " + e1.ToString());
            }
            if (tag!=null && s !=null)
            {
                try
                {
                    if ("BOOL".CompareTo(tag.Tipo) == 0)
                    {
                        if ("TRUE".CompareTo(s.Value.ToString().ToUpper())==0)
                        {
                            tag.lib_tag.Value = true;
                        }
                        else
                        {
                            tag.lib_tag.Value = false;
                        }
                    }
                    else if ("DINT".CompareTo(tag.Tipo) == 0)
                    {
                        
                        tag.lib_tag.Value= s.Value;
                        
                    }
                    else if ("INT".CompareTo(tag.Tipo) == 0)
                    {
                        tag.lib_tag.Value = s.Value;
                    }
                    else if ("LINT".CompareTo(tag.Tipo) == 0)
                    {
                        tag.lib_tag.Value = s.Value;
                    }
                    else if ("LREAL".CompareTo(tag.Tipo) == 0)
                    {
                        tag.lib_tag.Value = s.Value;
                    }
                    else
                    {
                        s = null;
                    }

                    if (s != null)
                    {
                        // tag.lib_tag.Value = s.Value;
                        await tag.lib_tag.WriteAsync();
                    }
                } catch (Exception e1)
                {
                    Console.WriteLine("Excepción: " + e1.ToString());
                }
                
            }
        }
        public static async Task Subscribe_Topic()
        {
            /*
             * This sample subscribes to a topic.
             */

            var mqttFactory = new MqttFactory();

            using (var mqttClient = mqttFactory.CreateMqttClient())
            {
                var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer("broker.hivemq.com").Build();

                await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

                var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
                //    .WithTopicTemplate(sampleTemplate.WithParameter("id", "1"))
                    .Build();
               
                var response = await mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);

                Console.WriteLine("MQTT client subscribed to topic.");

                // The response contains additional data sent by the server after subscribing.
                //response.DumpToConsole();
            }
        }



        //-------------------------------------------------------------------------------



        public static async Task Handle_Received_Application_Message()
        {
            /*
             * This sample subscribes to a topic and processes the received message.
             */

            var mqttFactory = new MqttFactory();

            using (var mqttClient = mqttFactory.CreateMqttClient())
            {
                var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer("broker.hivemq.com").Build();

                // Setup message handling before connecting so that queued messages
                // are also handled properly. When there is no event handler attached all
                // received messages get lost.
                mqttClient.ApplicationMessageReceivedAsync += e =>
                {
                    Console.WriteLine("Received application message.");
                 //   e.DumpToConsole();

                    return Task.CompletedTask;
                };

                await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

                var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
                 //   .WithTopicTemplate(sampleTemplate.WithParameter("id", "2"))
                    .Build();

                await mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);

                Console.WriteLine("MQTT client subscribed to topic.");

                Console.WriteLine("Press enter to exit.");
                Console.ReadLine();
            }
        }

    }
}

/*

public static async Task Send_Responses()
{
    /*
     * This sample subscribes to a topic and sends a response to the broker. This requires at least QoS level 1 to work!
     *//*

    var mqttFactory = new MqttFactory();

    using (var mqttClient = mqttFactory.CreateMqttClient())
    {
        mqttClient.ApplicationMessageReceivedAsync += delegate (MqttApplicationMessageReceivedEventArgs args)
        {
            // Do some work with the message...

            // Now respond to the broker with a reason code other than success.
            args.ReasonCode = MqttApplicationMessageReceivedReasonCode.ImplementationSpecificError;
            args.ResponseReasonString = "That did not work!";

            // User properties require MQTT v5!
            args.ResponseUserProperties.Add(new MqttUserProperty("My", "Data"));

            // Now the broker will resend the message again.
            return Task.CompletedTask;
        };

        var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer("broker.hivemq.com").Build();

        await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

        var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
            .WithTopicTemplate(
                sampleTemplate.WithParameter("id", "1"))
            .Build();

        var response = await mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);

        Console.WriteLine("MQTT client subscribed to topic.");

        // The response contains additional data sent by the server after subscribing.
        response.DumpToConsole();
    }
}

public static async Task Subscribe_Multiple_Topics()
{
    /*
     * This sample subscribes to several topics in a single request.
     *//*

    var mqttFactory = new MqttFactory();

    using (var mqttClient = mqttFactory.CreateMqttClient())
    {
        var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer("broker.hivemq.com").Build();

        await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

        // Create the subscribe options including several topics with different options.
        // It is also possible to all of these topics using a dedicated call of _SubscribeAsync_ per topic.
        var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
            .WithTopicTemplate(
                sampleTemplate.WithParameter("id", "1"))
            .WithTopicTemplate(
                sampleTemplate.WithParameter("id", "2"), noLocal: true)
            .WithTopicTemplate(
                sampleTemplate.WithParameter("id", "3"), retainHandling: MqttRetainHandling.SendAtSubscribe)
            .Build();

        var response = await mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);

        Console.WriteLine("MQTT client subscribed to topics.");

        // The response contains additional data sent by the server after subscribing.
        response.DumpToConsole();
    }
}


static void ConcurrentProcessingDisableAutoAcknowledge(CancellationToken shutdownToken, IMqttClient mqttClient)
{
    /*
     * This sample shows how to achieve concurrent processing and not have message AutoAcknowledged
     * This to have a proper QoS1 (at-least-once) experience for what at least MQTT specification can provide
     *//*
    mqttClient.ApplicationMessageReceivedAsync += ea =>
    {
        ea.AutoAcknowledge = false;

        async Task ProcessAsync()
        {
            // DO YOUR WORK HERE!
            await Task.Delay(1000, shutdownToken);
            await ea.AcknowledgeAsync(shutdownToken);
            // WARNING: If process failures are not transient the message will be retried on every restart of the client
            //          A failed message will not be dispatched again to the client as MQTT does not have a NACK packet to let
            //          the broker know processing failed
            //
            // Optionally: Use a framework like Polly to create a retry policy: https://github.com/App-vNext/Polly#retry
        }

        _ = Task.Run(ProcessAsync, shutdownToken);

        return Task.CompletedTask;
    };
}

static void ConcurrentProcessingWithLimit(CancellationToken shutdownToken, IMqttClient mqttClient)
{
    /*
     * This sample shows how to achieve concurrent processing, with:
     * - a maximum concurrency limit based on Environment.ProcessorCount
     *//*

    var concurrent = new SemaphoreSlim(Environment.ProcessorCount);

    mqttClient.ApplicationMessageReceivedAsync += async ea =>
    {
        await concurrent.WaitAsync(shutdownToken).ConfigureAwait(false);

        async Task ProcessAsync()
        {
            try
            {
                // DO YOUR WORK HERE!
                await Task.Delay(1000, shutdownToken);
            }
            finally
            {
                concurrent.Release();
            }
        }

        _ = Task.Run(ProcessAsync, shutdownToken);
    };
}
}
*/