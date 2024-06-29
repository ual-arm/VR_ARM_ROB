# VR_ARM_ROB

&nbsp;&nbsp;&nbsp;&nbsp;Este proyecto genera una aplicación de realidad virtual desarrollada mediante el entorno de programación de Unity, en la cual se puede interacctuar de ditintas formas con los robots IRB140, IRB1090 y SCHUNK LWA 4P programando puntos trayectorias y programas en ellos, los cuales se pueden reproducir en cualquiera de los robots reales. Ademas, al conectar Unity a cualquiera de los robots se dispone de una nueva forma de interacción, la cual consiste en una conexión en directo entre el robot virtual y el robot real, es decir los desplazamientos efectuados sobre el robot virtual en Untiy son efectuados al mismo tiempo sobre el robot real. Para los robots ABB tambien se dispone de una celula de trabajo simulada mediante RbotStudio con la cual se puede establecer conexión y trabajar con ella de igual forma que se haría con el robot real.

Para obtener mas información acerca de este proyecto se recomienda leer al ***memoria del TFG.


  
&nbsp;&nbsp;&nbsp;&nbsp;Este GitHub incorpora tanto los archivos, como un manual de instalación para llevar a cabo la puesta en marcha de Unity, las celulas simuladas y las conexiones con los robots reales ABB y SCHUNK. Ademas se incluye una descripción sobre algunos de los codigo mas utilis empleados en el desempeño del proyecto.


## Manual instalación
### Instalación aplicación Unity
Para poder utilizar la aplicación desarrollada con Unity es necesario disponer del dispositivo de realidad virtual Oculus Rift S y un ordenador que cumpla los ***requisitos minimos impuestos por Oculus. Para proceder con la insatalación de dicha aplicación:
1.  Installar y registrarse en UnityHub
2.  Instalar la ***versión 2022.3.21f1 de Unity Hub
   
3.  Para descargar la aplicación puede hacerse en modo desarrollador (permite cambios en la aplicación y ver mensajes de control) o en modo usuario (no permite cambios):

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;3.1.  Modo usario: Descargar el archivo ROB_VR.exe y ejecutarlo en el dispositivo

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;3.2.  Modo desarrollador: 

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- Crear un nuevo proyecto en Unity Hub, con el nombre ROB_VR.

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- Acceder al direcctorio del proyecto y eliminar el contenido de la carpeta ROB_VR.

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- Descargar ROB_VR_desarrollador

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- Descomprimir el archivo descargado ROB_VR_desarollador en la carpeta creada ROB_VR.

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- Ejecuatar el programa desde UnityHub.

### Instalación y conexión celulas RobotStudio
Para disponer de una celucla robotizada simulada mediante RobotStudio, la cual permita conectar el robot virtual de Unity con el robot simulado los pasos a seguir son:

1.  Descargar RobotStudio
2.  Descargar el modelo de celula deseado, IRB140 o IRB1090
3.  Hacer PackandGo de la celula descarga desde RobotStudio
4.  Para establecer la conexión entre RobotStudio y Unity pueden darse dos situaciones:
   
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;4.1 Unity y RobotStudio en el mismo dispositivo (caso preconfigurado):

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; - Acceder al codigo RAPID de la celula y al declarar la variable IP establecer como IP la dirección 127.0.0.1 

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; - Acceder al proyecto Unity y en el menu de objetos de la escena se buscará la ubicación Pantallas/Robot &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; deseado (IRB1090 o IRB140)/Menu conexión Robot/Comunicación se establecerá la IP 127.0.0.1

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;4.2 Unity y RobotStudio en distintos dispositivos:

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; - Conectar ambos dispositivos a una misma red wifi o Ethernet

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; - En el dispositivo donde esta instalado RobotStudio obtener la dirección IP desde cmd:

***Codigo IP***

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; - Acceder al codigo RAPID de la celula y al declarar la variable IP establecer como IP la dirección IP &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; obtenida

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; - Acceder al proyecto Unity y en el menu de objetos de la escena se buscará la ubicación Pantallas/Robot &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; deseado (IRB1090 o IRB140)/Menu conexión Robot/Comunicación se establecerá la IP obtenida.

5. Ejecutar la simulación de RobotStudio
6. Ejecutar la simulación de Unity
7. Para conectase al robot basta con usar el panel conexión de dicho robot. Es necesario conocer que el servidor de RobotStudio tiene un tiempo maximo de espera. Si en el momento de establecer la conexión esta no se lleva a cabo será necesario reiniciar la celula de RobotStudio.


### Instalación y conexión ABB real

1. Descargar el programa rapid correspondiente al robot IRB140 o IRB1090
2. Transmitir el programa rapid al robot real, mediante un pendrive directo al FlexPendant de este robot o atraves de RobotStudio
3. Inicializar el programa RAPID desde el FlexPendant
4. Iniciar la comunicación desde Unity.

### Conexión SCHUNK LWA 4P

Dado que este proyecto no ha desarrollado la interfaz de control del robot SCHUNK, no se dispone de la información correspondiente a la descarga y establecimiento del nodo ROS, sino que la comunicación se debe realizar con el ordenador disponible en el laboratorio conectado al robot SCHUNK LWA 4P. Para llevar a cabo dicha conexión los pasos a seguir son:

1. Conectar el ordenador del laboratorio con el ordenador que contiene Unity mediante un cable Ethernet cruzado.
2. Congirar la red, para que los dispositivos puedan reconocerse. Para ello:
   - En el dispositivo del laboratorio: Desde configuración de red establecer la IP en 192.168.1.3
   - En el ordenador con Unity: Desde la configuraciónde red y ajustes avanzados configurar la IP como 192.168.1.2
3. Abrir la consola del ordenador del laboratorio y ejecutar los comandos:
4. Aparecerá el panel de control del robot SCHUNK en la pantalla, donde basta con pulsar el boton INICIAR par conectar con el nodo ROS.
5. Establecer al conexión desde el entorno virtual de Unity.


## Codigos de utilidad
