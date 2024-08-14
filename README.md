# MqttPLCPublisher
Release 0.1  
Copyright 2024 - Héctor E. Rey  
GPL v2.0  

**Descargar archivo binario [**Aquí**](http://hersoft.com.ar/MqttPLCPublisher/Release_0.1).**

MqttPLCPublisher realiza de forma fácil y sin ningúna aplicación intermediaria la publicación de variables de PLCs comunicados por Ethernet en brokers Mqtt.

La configuración se encuentra en un archivo config.xml en la carpeta en la que se ejecuta el programa.

Un ejemplo de configuración es el siguiente:

\<Config\>  
&nbsp;&nbsp;&nbsp;&nbsp;\<PLC Name="Mi_PLC"  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Protocol="AB_EIP"  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Type="ControlLogix"  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Address="192.168.1.1"  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Path="1,0"/\>  
&nbsp;&nbsp;&nbsp;&nbsp; \<Tag Name="Mi_Tag"  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Type="DINT"  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; PLC="Mi_PLC"  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Period= "100"/\>  
&nbsp;&nbsp;&nbsp;&nbsp; \<Broker Name="Mi_Broker" Address="10.10.1.1" Port="1883"/\>  
&nbsp;&nbsp;&nbsp;&nbsp; \<Publish Topic="Topico/Donde/Publico/El_Tag"  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Broker="Mi_Broker"  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Tag="Mi_Tag"/\>  
\</Config\>  

En este ejemplo el programa leerá de Mi_PLC el valor de Mi_Tag cada 100ms y lo publicará en Mi_Broker cada actualización que se realice.

## Configuración

El archivo de configuración se denomina Config.xml y debe encontrarse en el path por defecto en el que se está ejecutando la aplicación.
El formato del mismo es un archivo XML con un objeto general llamado \<Config\> que puede incluir los siguentes elementos:

### \<PLC\>

El objeto PLC define cada uno de los PLCs con los cuales queremos comunicarnos.  Los atributos posibles son:

**Name:** Le asigna un nombre al dispositivo. Este nombre será el utilizado para hacer referencia sea utilizado en otros objetos. (No es case sensitive).  
**Protocol:** Indica el protocolo utilizado para comunicarse.  Las opciones son **AB_ETH** (Ethernet/IP)  o **MODBUS_TCP**.  
**Type:**  Indica el tipo de dispositivo con el que quiero comunicarme. Las opciones son: **ControlLogix**, **LogixPCCC**, **Micro800**, **MicroLogix**, **Omron**, **PLC5** y **SLC500**.  
**Address:** Indica la dirección IP donde encontrará el dispositivo.  
**Path:** Indica la runta para acceder al dispositivo. Por ejemplo, lo mas común en ContolLogix es usar "1,0", dónde, **Address** indica la dirección IP de la placa ENBT, ENET o EN2T etc, el 1 indica que rutea al puerto 1 de la placa que sería el backplane, luego el 0 indica el slot 0 del backplane es donde se encuentra la CPU.  De esta forma pueden realizarse también rutas mas complejas.  
**Timeout:** Permite indicar un tiempo en milisegundos de timeout de la comunicación.  

### \<TAG\>

El objeto TAG define cual es el tag del PLC que vamos a utilizar, ya sea para leerlo y publicarlo y escribirlo al recibir alguna actualización de una subscripción.  Los atributos posibles son:

**Name:** Le asigna un nombre al tag para poder referirse luego para publicarlo o usarlo en una subscripción.  
**PLC:** Indica el PLC dónde buscará el TAG.  
**Address:** Indica como se denomina el tag en el PLC. Si no se indica Address se utiliza el valor en **Name**.  
**Type:** Indica el tipo de datos. En este primer release 0.1 los valores posibles son: **BOOL**, **DINT**, **INT**, **SINT**, **LINT**, **REAL**, **LREAL** y **STRING**.  
**Period:** Indica cada cuantos milisegundos se intentará leer el tag en el PLC. El valor por defecto es de 10 segundos (10000 ms).  

### \<Broker\>

El objeto Broker define cual es el Broker donde realizaremos las publicaciones o subscripciones. Los atributos posibles son:

**Name:** Indica el nombre del broker al que nos conectaremos para despues utilizarlo al publicar o subscribir.  
**Address:** Indica la dirección URL del broker.  
**Port:** Indica el puerto al que escucha el broker.  
**User:** Nombre del usuario para la conexión al broker.  
**Password:** Indica el password a utilizar para la conexión.  

### \<Publish\>

El objeto Publish define la publicación a realizar. Los atributos posibles son:

**Topic:** Indica el tópico donde se realizará la publicación.  
**Broker:** Indica el broker donde se realizará la publicación.  
**Tag:** Indica el nombre del tag que se publicará.  
**OnChange:** Si se indica OnChange="False" se realizará la publicación en cada lectura del PLC. El valor por defecto es True. Con OnChange="True" se realizará la publicación cada vez que cambie el valor del tag.  

### \<Subscribe\>

El objeto Subscribe define una subscripción a un tópico en el broker. Al realizar una subscripción se le asigna un tag que será escrito en el PLC cada vez que se reciba una actualización de la subscripción. Los atributos posibles son:

**Broker:** Indica el broker donde se realizará la subscripción.  
**Topic:** Indica el tópico al que nos subscribiremos.  
**Tag:** Indica dónde se escribirá el valor al recibir el mensaje de la subscripción.  

**Nota**: En este release 0.1 las publicaciones se realizan con em formato JSON con dos campos: Timestamp y Value. Este mismo formato es esperado en las subscripciones, donde el valor de Timestamp es ignorado y Value es pasado al PLC.  


