﻿// See https://aka.ms/new-console-template for more information
using libplctag.DataTypes;
using libplctag;
using libplctag.DataTypes.Simple;
using MqttPLCPublisher;
using System.Xml;

Console.WriteLine("Hello, World!");

/*var myTag = new Tag<DintPlcMapper, int>()
{
    Name = "PROGRAM:SomeProgram.SomeDINT",
    Gateway = "192.168.1.10",
    Path = "1,0",
    PlcType = PlcType.ControlLogix,
    Protocol = Protocol.ab_eip,
    Timeout = TimeSpan.FromSeconds(5)
}; 

myTag.Initialize();
*/

// Lee archivo .xml
/*
 * El XML tendrá un elemento Tags
 *      Habrá objeto TAG
 *          Cada Objeto Tag tendrá Path, Tag, Tipo
 *          Tendrá Tiempo de refresco
 * Habrá otro obeto Brokers
 *      HAbrá Tags Broker
 *          Tendrá Nombre
 *          Dirección
 *          Puerto
 *          (Certificado, Usuario, Contraseña)
 * Tendrá otro objeto Publicaciones
 *      Tendrá nombre
 *          Broker
 *          Formato
 *          Tag
 *          Actualización: Tiempo de refresco / al cambiar
 */


Dictionary<String, PLC> PLCs = new Dictionary<String, PLC>();
Dictionary<String, Tagx> Tags = new Dictionary<string, Tagx>(); 
Dictionary<String, Broker> Brokers = new Dictionary<string, Broker>();
Dictionary<String, Publish> Publishes = new Dictionary<string, Publish>();

Tagx.PLCs = PLCs;
Publish.Brokers = Brokers;
Publish.Tags = Tags;


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
            if (e.Name == "PLC")
            {
                PLC p = new PLC(e);
                PLCs.Add(p.Nombre, p);
                /*if (e.Attributes.GetNamedItem("Name") != null)
                {
                    Console.WriteLine("Cargando... PLC = " + e.Attributes.GetNamedItem("Name").Value);
                }*/
            }
            if (e.Name == "Tag")
            {
                Tagx t = new Tagx(e);
                if (t.Plc == null)
                {
                    Console.WriteLine("Tag " + t.Nombre + " sin PLC.");
                }
                else
                {
                    Tags.Add(t.Nombre + "@" + t.Plc.Nombre, t);
                }

                /*if (e.Attributes.GetNamedItem("Name") != null)
                {
                    Console.WriteLine("Cargando... Tag = " + e.Attributes.GetNamedItem("Name").Value);
                }*/
            }
            if (e.Name == "Broker")
            {
                Broker b = new Broker(e);
                Brokers.Add(b.Nombre, b);

            }
            if (e.Name == "Publish")
            {
                Publish b = new Publish(e);
                Publishes.Add("", b);
            }

        }
    }
} catch (Exception e) 
{
    Console.WriteLine("Error al leer la configuración: " + e.ToString());
}

// Conecta al Broaker

//'Globales g = new Globales();

//g.conectarAlBroaker();

//g.publish();

foreach (Broker b in Brokers.Values)
{
    b.conectar();
}

// Lee los TAGS

foreach (Tagx t in Tags.Values)
{
    t.leer();
}

/*foreach (Publish p in Publishes.Values)
{
    p.ejecutar();
}*/
// Cada Tag se publica

while (true)
{
    //Thread.Sleep(1000);
    Console.ReadLine();
 //   Console.WriteLine(Tags.Values.First().Valor.ToString());
};

Console.WriteLine("Bye.!");

//Console.ReadLine();