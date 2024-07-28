﻿using MQTTnet.Protocol;
using MQTTnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MqttPLCPublisher
{
    internal class Subscribe
    {
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

        public async void ejecutar()
        {
            if (broker.mqttClient != null)
            {
                if (broker.mqttClient.IsConnected == false)
                {
                    broker.conectar();

                }

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
    }
}
