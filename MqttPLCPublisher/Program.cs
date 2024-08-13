// See https://aka.ms/new-console-template for more information

using libplctag.DataTypes;
using libplctag;
using libplctag.DataTypes.Simple;
using MqttPLCPublisher;
using System.Xml;
using System.Reflection;
using MQTTnet.Server;

Console.WriteLine("MqttPLCPublisher Ver."+ Assembly.GetExecutingAssembly().GetName().Version);

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
Dictionary<String, Subscribe> Subscribes = new Dictionary<string, Subscribe>();
Dictionary<String, Template> Templates = new Dictionary<string, Template>();

Tagx.PLCs = PLCs;
Publish.Brokers = Brokers;
Publish.Tags = Tags;
Subscribe.Brokers = Brokers;
Subscribe.Tags = Tags;



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
         /*   if (e.Name.ToUpper() == "TEMPLATE")
            {
                Template t = new Template(e);
                Templates.Add(t.Nombre, t);
                /*if (e.Attributes.GetNamedItem("Name") != null)
                {
                    Console.WriteLine("Cargando... PLC = " + e.Attributes.GetNamedItem("Name").Value);
                }*/
          //  }
            if (e.Name.ToUpper() == "PLC")
            {
                PLC p = new PLC(e);
                PLCs.Add(p.Nombre, p);
                /*if (e.Attributes.GetNamedItem("Name") != null)
                {
                    Console.WriteLine("Cargando... PLC = " + e.Attributes.GetNamedItem("Name").Value);
                }*/
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
                    Tags.Add(t.Nombre,t ); // + "@" + t.Plc.Nombre, t);
                }

                /*if (e.Attributes.GetNamedItem("Name") != null)
                {
                    Console.WriteLine("Cargando... Tag = " + e.Attributes.GetNamedItem("Name").Value);
                }*/
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
                Subscribes.Add(b.GetHashCode().ToString(), b);
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

foreach (Subscribe p in Subscribes.Values)
{
    p.ejecutar();
}

while (true)
{
    //Thread.Sleep(1000);
    Console.ReadLine();
 //   Console.WriteLine(Tags.Values.First().Valor.ToString());
};

Console.WriteLine("Bye.!");

//Console.ReadLine();
