using libplctag.DataTypes;
using libplctag;
using libplctag.DataTypes.Simple;
using MqttPLCPublisher;
using System.Xml;
using System.Reflection;
using MQTTnet.Server;

Console.WriteLine("MqttPLCPublisher Ver." + Assembly.GetExecutingAssembly().GetName().Version);
Console.WriteLine("Copyright 2024 - Héctor E. Rey");
Console.WriteLine("GPL 2.0");
Console.WriteLine("");

Dictionary<String, PLC> PLCs = new Dictionary<String, PLC>();
Dictionary<String, Tagx> Tags = new Dictionary<string, Tagx>(); 
Dictionary<String, Broker> Brokers = new Dictionary<string, Broker>();
Dictionary<String, Publish> Publishes = new Dictionary<string, Publish>();
Dictionary<String, Subscribe> Subscribes = new Dictionary<string, Subscribe>();

Tagx.PLCs = PLCs;
Publish.Brokers = Brokers;
Publish.Tags = Tags;
Subscribe.Brokers = Brokers;
Subscribe.Tags = Tags;
Subscribe.Subscribes = Subscribes;

//  Lee el archivo de configuración

try
{
    XmlDocument config = new XmlDocument();
    config.Load("config.xml");
    XmlNode c_nodo = config.DocumentElement;
    if (c_nodo != null)
    {
        XmlNodeList nl = c_nodo.ChildNodes;
        foreach (XmlNode e in nl)
        {
            if (e.Name.ToUpper() == "PLC")
            {
                PLC p = new PLC(e);
                PLCs.Add(p.Nombre, p);
            }
            if (e.Name.ToUpper() == "TAG")
            {
                Tagx t = new Tagx(e);
                if (t.Plc == null)
                {
                    Console.WriteLine("Tag " + t.Nombre + " sin PLC.");
                }
                else
                {
                    Tags.Add(t.Nombre,t ); 
                }

            }
            if (e.Name.ToUpper() == "BROKER")
            {
                Broker b = new Broker(e);
                Brokers.Add(b.Nombre, b);

            }
            if (e.Name.ToUpper() == "PUBLISH")
            {
                Publish b = new Publish(e);
                Publishes.Add(b.GetHashCode().ToString(), b);
            }
            if (e.Name.ToUpper() == "SUBSCRIBE")
            {
                Subscribe b = new Subscribe(e);
                Subscribes.Add(b.Topico, b);
            }

        }
    }
} catch (Exception e) 
{
    Console.WriteLine("Error al leer la configuración: " + e.ToString());
}

// Conecta a los Broakers

foreach (Broker b in Brokers.Values)
{
    b.conectar();
}

// Lee los TAGS

foreach (Tagx t in Tags.Values)
{
    t.leer();
}

// Realiza las subsctipciones

foreach (Subscribe p in Subscribes.Values)
{
    p.ejecutar();
}

// Queda esperando por siempre.

while (true)
{
    Console.ReadLine();
};
