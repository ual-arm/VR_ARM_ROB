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

&nbsp;&nbsp;&nbsp;&nbsp;3.1.  Modo usario: Descargar el archivo ROB_VR.exe y ejecutarlo en el dispositivo

&nbsp;&nbsp;&nbsp;&nbsp;3.2.  Modo desarrollador: 

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- Crear un nuevo proyecto en Unity Hub, con el nombre ROB_VR.

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- Acceder al direcctorio del proyecto y eliminar el contenido de la carpeta ROB_VR.

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- Descargar ROB_VR_desarrollador

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- Descomprimir el archivo descargado ROB_VR_desarollador en la carpeta creada ROB_VR.

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- Ejecuatar el programa desde UnityHub.

### Instalación y conexión celulas RobotStudio
Para disponer de una celucla robotizada simulada mediante RobotStudio, la cual permita conectar el robot virtual de Unity con el robot simulado los pasos a seguir son:

1.  Descargar RobotStudio
2.  Descargar el modelo de celula deseado, IRB140 o IRB1090
3.  Hacer PackandGo de la celula descarga desde RobotStudio
4.  Para establecer la conexión entre RobotStudio y Unity pueden darse dos situaciones:
&nbsp;&nbsp;&nbsp;&nbsp;4.1 Unity y RobotStudio en el mismo dispositivo (caso preconfigurado):

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; - Acceder al codigo RAPID de la celula y al declarar la variable IP establecer como IP la dirección 127.0.0.1 

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; - Acceder al proyecto Unity y en el menu de objetos de la escena se buscará la ubicación Pantallas/Robot &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; deseado (IRB1090 o IRB140)/Menu conexión Robot/Comunicación se establecerá la IP 127.0.0.1

&nbsp;&nbsp;&nbsp;&nbsp;4.2 Unity y RobotStudio en distintos dispositivos:

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; - Conectar ambos dispositivos a una misma red wifi o Ethernet

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; - En el dispositivo donde esta instalado RobotStudio obtener la dirección IP desde cmd:

***Codigo IP***

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; - Acceder al codigo RAPID de la celula y al declarar la variable IP establecer como IP la dirección IP obtenida

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; - Acceder al proyecto Unity y en el menu de objetos de la escena se buscará la ubicación Pantallas/Robot &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; deseado (IRB1090 o IRB140)/Menu conexión Robot/Comunicación se establecerá la IP obtenida.


### Instalación y conexión ABB real
### Conexión SCHUNK LWA 4P

## Codigos de utilidad
