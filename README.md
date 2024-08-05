# MqttPLCPublisher

MqttPLCPublisher realiza de forma fácil y sin ningúna aplicación intermediaria la publicación de variables de PLCs comunicados por Ethernet en brokers Mqtt.

La configuración se encuentra en un archivo config.xml en la carpeta en la que se ejecuta el programa.

Un ejemplo de configuración es el siguiente:

\<?xml version="1.0" encoding="utf-8" ?\>  
\<Config\>  
&nbsp;&nbsp;&nbsp;&nbsp;	\<PLC Name="Mi_PLC"  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;		Protocol="AB_EIP"  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;		Type="ControlLogix"  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;		Address="192.168.1.1"  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;		Path="1,0"/\>  
&nbsp;&nbsp;&nbsp;&nbsp;	\<Tag Name="Mi_Tag"  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;		 Type="DINT"  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;		 PLC="Mi_PLC"  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;		 Period= "100"/\>  
&nbsp;&nbsp;&nbsp;&nbsp;	\<Broker Name="Mi_Broker" Address="10.10.1.1" Port="1883"/\>  
&nbsp;&nbsp;&nbsp;&nbsp;	\<Publish Topic="Topico/Donde/Publico/El_Tag"  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;			 Broker="Mi_Broker"  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;			 Tag="Mi_Tag"/\>  
\</Config\>  

En este ejemplo el programa leerá de Mi_PLC el valor de Mi_Tag cada 100ms y lo publicará en Mi_Broker cada actualización que se realice.

