# MqttPLCPublisher

MqttPLCPublisher realiza de forma fácil y sin ningúna aplicación intermediaria la publicación de variables de PLCs comunicados por Ethernet en brokers Mqtt.

La configuración se encuentra en un archivo config.xml en la carpeta en la que se ejecuta el programa.

Un ejemplo de configuración es el siguiente:

<?xml version="1.0" encoding="utf-8" ?>
<Config>
	<PLC Name="Mi_PLC"
		Protocol="AB_EIP"
		Type="ControlLogix"
		Address="192.168.1.1"
		Path="1,0"/>
	<Tag Name="Mi_Tag"
		 Type="DINT"
		 PLC="Mi_PLC"
		 Period= "100"/>
	<Broker Name="Mi_Broker" Address="10.10.1.1" Port="1883"/>
	<Publish Topic="Topico/Donde/Publico/El_Tag"
			 Broker="Mi_Broker"
			 Tag="Mi_Tag"/>
</Config>

En este ejemplo el programa leerá de Mi_PLC el valor de Mi_Tag cada 100ms y lo publicará en Mi_Broker cada actualización que se realice.

