# VR_ARM_ROB

&nbsp;&nbsp;&nbsp;&nbsp;Este proyecto genera una aplicación de realidad virtual desarrollada mediante el entorno de programación de Unity, en la cual se puede interacctuar de ditintas formas con los robots IRB140, IRB1090 y SCHUNK LWA 4P programando puntos, trayectorias y programas, los cuales se pueden reproducir en cualquiera de los robots reales. Además, al conectar Unity a cualquiera de los robots se dispone de una nueva forma de interacción, la cual consiste en una conexión en directo entre el robot virtual y el robot real, es decir, los desplazamientos efectuados sobre el robot virtual en Untiy son realizados al mismo tiempo sobre el robot real. Para los robots ABB tambien se dispone de una célula de trabajo simulada mediante RobotStudio con la cual, se puede establecer conexión y trabajar con ella de igual forma que se haría con el robot real.

Para obtener más información acerca de este proyecto se recomienda leer al [memoria del TFG](TFG_Antonio_Martinez_Navarro.pdf).


  
&nbsp;&nbsp;&nbsp;&nbsp;Este GitHub incorpora tanto los archivos, como un manual de instalación para llevar a cabo la puesta en marcha de Unity, las celulas simuladas y las conexiones con los robots reales ABB y SCHUNK. Además, se incluye una descripción sobre algunos de los códigos más utiles empleados en el desempeño del proyecto.



## Manual instalación
### Instalación aplicación Unity
Para poder utilizar la aplicación desarrollada con Unity es necesario disponer del dispositivo de realidad virtual Oculus Rift S y un ordenador que cumpla los ***requisitos minimos impuestos por Oculus. Para proceder con la insatalación de dicha aplicación:
1.  Instalar y registrarse en [UnityHub](https://unity.com/es/download).
2.  Instalar la [versión 2022.3.21f1](https://unity.com/es/releases/editor/archive) de Unity Hub
   
3.  Para descargar la aplicación puede hacerse en modo desarrollador (permite cambios en la aplicación y ver mensajes de control) o en modo usuario (no permite cambios):

     3.1.  Modo usario: Descargar el contenido de la [carpeta](ROB_VR/) y ejecutar el archivo [ROB_VR.exe](ROB_VR/ROB_VR.exe) en el dispositivo

     3.2.  Modo desarrollador: 

      - Crear un nuevo proyecto en Unity Hub, con el nombre ROB_VR.

      - Acceder al direcctorio del proyecto y eliminar el contenido de la carpeta ROB_VR.

      - Descargar el contenido de la carpeta [ROB_VR_desarrollador](ROB_VR_desarrollador/).

      - Copiar los archivos descargados ROB_VR_desarollador en la carpeta creada ROB_VR.

      - Ejecuatar el programa desde UnityHub.

### Instalación y conexión celulas RobotStudio
Para disponer de una celula robotizada simulada mediante RobotStudio, la cual permita conectar el robot virtual de Unity con el robot simulado los pasos a seguir son:

1.  Descargar [RobotStudio](https://new.abb.com/products/robotics/es/robotstudio/descargas).
2.  Descargar el modelo de celula deseado, [IRB140](Irb_140.rspag) o [IRB1090](Irb_1090.rspag)
3.  Hacer Pack&Go de la celula descarga desde RobotStudio
4.  Para establecer la conexión entre RobotStudio y Unity pueden darse dos situaciones:
   
      4.1 Unity y RobotStudio en el mismo dispositivo (caso preconfigurado):

       - Acceder al codigo RAPID de la celula y al declarar la variable IP establecer como IP la dirección 127.0.0.1 

       - Acceder al proyecto Unity y en el menú de objetos de la escena se buscará la ubicación Pantallas/Robot deseado (IRB1090 o IRB140)/Menu conexión Robot/Comunicación se establecerá la IP 127.0.0.1
         
      4.2 Unity y RobotStudio en distintos dispositivos:

       - Conectar ambos dispositivos a una misma red wifi o Ethernet

       - En el dispositivo donde esta instalado RobotStudio obtener la dirección IP desde CMD:

        ipconfig


       - Acceder al codigo RAPID de la celula y al declarar la variable IP establecer como IP la dirección IP obtenida

       - Acceder al proyecto Unity y en el menu de objetos de la escena se buscará la ubicación Pantallas/Robot deseado (IRB1090 o IRB140)/Menu conexión Robot/Comunicación se establecerá la IP obtenida.

5. Ejecutar la simulación de RobotStudio
6. Ejecutar la simulación de Unity
7. Para conectase al robot basta con usar el panel conexión de dicho robot. Es necesario conocer que el servidor de RobotStudio tiene un tiempo maximo de espera. Si en el momento de establecer la conexión esta no se lleva a cabo será necesario reiniciar la celula de RobotStudio.

### Instalación y conexión ABB real

1. Descargar el programa RAPID correspondiente al robot [IRB140](Module_irb140.mod) o [IRB1090](Module_irb1090.modx)
2. Transmitir el programa RAPID al robot real, mediante un pendrive directo al FlexPendant de este robot o atraves de RobotStudio
3. Inicializar el programa RAPID desde el FlexPendant
4. Iniciar la comunicación desde Unity.

### Conexión SCHUNK LWA 4P

Dado que este proyecto no ha desarrollado la interfaz de control del robot SCHUNK, no se dispone de la información correspondiente a la descarga y establecimiento del nodo ROS, sino que la comunicación se debe realizar con el ordenador disponible en el laboratorio conectado al robot SCHUNK LWA 4P. Para llevar a cabo dicha conexión los pasos a seguir son:

1. Conectar el ordenador del laboratorio con el ordenador que contiene Unity mediante un cable Ethernet cruzado.
2. Congirar la red, para que los dispositivos puedan reconocerse. Para ello:
   - En el dispositivo del laboratorio: Desde configuración de red establecer la IP en 192.168.1.3
   - En el ordenador con Unity: Desde la configuraciónde red y ajustes avanzados configurar la IP como 192.168.1.2
3. Abrir la consola del ordenador del laboratorio y ejecutar los comandos:

        cd catkin_ws
        catkin_make
        . devel/setup.bash
        roslaunch schunk_lwa4p robot.launch
   
5. Aparecerá el panel de control del robot SCHUNK en la pantalla, donde basta con pulsar el boton INICIAR par conectar con el nodo ROS.
6. Establecer al conexión desde el entorno virtual de Unity.



## Codigos de utilidad
Con el objetivo de facilitar la comprensión de la aplicación de Rv desarrollada en Unity se incluyen los codigos mas representativos de este proyecto, entre los que destacan:
- Protocolos de comunicación que emplea Unity para los distintos robots, [IRB140](/Códigos de interes), [IRB1090](Códigos de interes/IRB1090/Comunicación/Comunicacion_irb1090.cs) y [SCHUNK](Códigos de interes/SCHUNK/Comunicación/Comunicacion_schunk.cs).
- Codigo para crear, gestionar y desplazar a los puntos deseados, para el [IRB140](VR_ARM_ROB/Códigos de interes/IRB140/Comunicación
/Comunicacion_irb140.cs), [IRB1090]() y [SCHUNK]().
- Codigo para crear, gestionar y desplazar a las trayectorias deseadas, para el [IRB140](), [IRB1090]() y [SCHUNK]().
- Codigo para crear, gestionar y ejecutar los programas elaborados, para el [IRB140](), [IRB1090]() y [SCHUNK]().
