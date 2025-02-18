MODULE Module1
    VAR socketdev serverSocket;
    VAR socketdev clientSocket;
    VAR socketdev clientSocketf;
    VAR string Ip;
    VAR intnum TcpPort;
    VAR string dataTcp;
    VAR string response;
        
        
    VAR num valor_tiempo;

    VAR socketdev udpServerThread;
    VAR socketdev clientSocketThread;
    VAR string receivedCoordinateThread;
    VAR intnum UdpPort;
    VAR bool ActiveUDP;

    VAR string jp1;
    VAR string jp2;
    VAR string jp3;
    VAR string jp4;
    VAR string jp5;
    VAR string jp6;

    VAR bool C1;
    VAR bool C2;
    VAR bool C3;
    VAR bool C4;
    VAR bool C5;
    VAR bool C6;

    VAR num J1;
    VAR num J2;
    VAR num J3;
    VAR num J4;
    VAR num J5;
    VAR num J6;
    
    VAR num jk1;
    VAR num jk2;
    VAR num jk3;
    VAR num jk4;
    VAR num jk5;
    VAR num jk6;
    
    VAR num pos1;
    VAR num pos2;
    VAR num pos3;
    VAR num pos4;
    VAR num pos5;
    VAR num pos6;
    
     VAR num posx1;
    VAR num posx2;
    VAR num posx3;
    VAR num posx4;
    VAR num posx5;
    VAR num posx6;
    
    VAR string jx11;
    VAR string jx12;
    VAR string jx21;
    VAR string jx22;
    VAR string jx31;
    VAR string jx32;
    VAR string jx41;
    VAR string jx42;
    VAR string jx51;
    VAR string jx52;
    VAR string jx61;
    VAR string jx62;
    
    VAR string control;
    
    VAR num i;
    VAR num q;
    VAR num t;
    VAR num k;
    
    VAR robtarget targetPos;
    VAR jointtarget jt0;
    VAR jointtarget jt1;
    VAR jointtarget jg1;
    VAR jointtarget jmt1{600};
    VAR jointtarget jmt2;
    
    VAR string partes1;
    VAR string partes2;
    VAR string partes3;
    VAR string partes4;
    VAR string partes5;
    VAR string partes6;
    
    VAR string cad1;
    VAR string cad2;
    VAR string cad3;
    VAR string cad4;
    VAR string cad5;
    VAR string cad6;
    
    VAR string numpuntos;
    VAR num numeropuntos;
    
    VAR num PointList{30,6};
    
    VAR bool aux;
    VAR num distance{600};
    VAR string distance_;
    VAR string velocidad_;
    VAR num velocidad;
    VAR num velocidades{600};
    VAR speeddata velocidad_speed;
    VAR speeddata v_dynamic;
    
    VAR string tp;
    VAR string ti;
    VAR string tm;
    
    VAR string tpn;
    VAR string tin;
    VAR string tmn;
    
    VAR string tiempo_trayectoria;
    VAR num tiempo{300};
    VAR BOOL connected := FALSE;
    
    VAR string numpuntos_totales_;
    VAR num numpuntos_totales;
    VAR string numtrayectorias_totales_;
    VAR num numtrayectorias_totales;
    
    VAR jointtarget puntos_totales{40};
    VAR jointtarget trayectorias_totales{20,300};
    VAR num velocidades_totales{20,300};
    VAR num distancias_totales{20,300};
    VAR num info_trayectoria{20,2};
    VAR jointtarget currentPos;
    VAR robtarget currentRobTarget;
               
    VAR string numero_condiciones_;
    VAR num numero_condiciones;
    VAR string tipo_condicion_{10};
    VAR num tipo_condicion{10};
    VAR string valor_x_{10};
    VAR num valor_x{10};
    VAR string valor_y_{10};
    VAR num valor_y{10};
    VAR string numero_acciones_{10};
    VAR num numero_acciones{10};
    VAR string tipo_accion_{10,10};
    VAR num tipo_accion{10,10};
    VAR string numero_accion_{10,10};
    VAR num numero_accion{10,10};
    
    VAR signaldi Entrada{5};

    VAR signaldo Salida{5};
    
    VAR string entrada_{5};
    
    VAR num auxvar;

    
    
    
    
    PROC main()
        Ip := "192.168.144.150";
        !Ip :="127.0.0.1";
        TcpPort := 10000;
        UdpPort := 1000;
        
        receivedCoordinateThread :="";
        J1 :=0;
        J2 :=0;
        J3 :=0;
        J4 :=0;
        J5 :=0;
        
        
        jt0 :=[[-J1, -J2, -J3, -J4, -J5, 0],[9E9, 9E9, 9E9, 9E9, 9E9, 9E9]];

                    MoveAbsJ jt0, v60, fine, tool0; 
        
        ActiveUDP := TRUE;
!! -CODIGO PARA CREAR EL SERVIDOR

                               
        SocketCreate serverSocket;
        SocketBind serverSocket, Ip, TcpPort; 
        SocketListen serverSocket; 
        SocketAccept serverSocket, clientSocket, \Time:=WAIT_MAX; 
                                   

!! CODIGO PARA CREAR EL CLIENTE
        !SocketCreate clientSocket;
        ! WHILE (NOT connected) DO
         !    SocketConnect clientSocket, Ip, TcpPort;
            ! Reemplaza "192.168.1.100" por la direcci�n IP y 12345 por el puerto

          !  response := "Prueba";
          !  SocketSend clientSocket, \Str:=response;
          !  SocketReceive clientSocket, \Str:=dataTcp;            
            ! Verificar si la conexi�n fue exitosa
          !  IF (dataTcp = "ok") THEN
          !      connected := TRUE;
           ! ELSE
           !      WaitTime 5;  ! Esperar 5 segundos antes de intentar de nuevo
          !  ENDIF

     !   ENDWHILE

        
        ! Realizar el intercambio TCP inicial
        SocketReceive clientSocket, \Str:=dataTcp;
        IF (dataTcp = "Hello, server") THEN
            response := "Hello";
        ELSE
            response := "Error";
        ENDIF
        
        SocketSend clientSocket, \Str:=response;

         WHILE ActiveUDP = TRUE DO
           
            SocketReceive clientSocket, \Str:=receivedCoordinateThread;
            
    !! CODIGO PARA COMUNICACION EN DIRECTO
            IF(receivedCoordinateThread="m")THEN
                  control :="ok";
                  SocketSend clientSocket, \Str:=control;
                WHILE NOT(receivedCoordinateThread="fm") DO
                    SocketReceive clientSocket, \Str:=receivedCoordinateThread;
                    IF NOT(receivedCoordinateThread="fm") THEN
                        ProcessReceivedCoordinate(receivedCoordinateThread);
                        jt1 :=[[-J1, -J2, -J3, -J4, -J5, 0],[9E9, 9E9, 9E9, 9E9, 9E9, 9E9]];
                        MoveAbsJ jt1, v1000, fine, tool0;
                    ENDIF
                     control :="ok";
                     SocketSend clientSocket, \Str:=control;
                ENDWHILE
            ENDIF
            
    !! CODIGO PARA MOVER PUNTOS   
            IF(receivedCoordinateThread="mp")THEN
                 i:=1;
                 control :="ok";
                 SocketSend clientSocket, \Str:=control;
                 SocketReceive clientSocket, \Str:=receivedCoordinateThread;
                 
                 currentPos := CJointT();
                 ProcessReceivedCoordinate(receivedCoordinateThread);
                 jg1 :=[[J1, J2, J3, J4, J5, 0],[9E9, 9E9, 9E9, 9E9, 9E9, 9E9]];  
                 distance{i}:= CalcJointDistance(jg1, currentPos);
                 
                 distance_:=ValToStr(distance{i});
                 SocketSend clientSocket, \Str:=distance_;
                 
                 SocketReceive clientSocket, \Str:=velocidad_;
                 aux:=STrToVal(velocidad_,velocidad);
                 SocketSend clientSocket, \Str:=control;
                 

                    IF NOT (receivedCoordinateThread="fgp") THEN
                        velocidad_speed := [velocidad, velocidad, velocidad, velocidad];
                        MoveAbsJ jg1, velocidad_speed, z10, tool0;
                    ENDIF                               
                    i:=i+1;  
            ENDIF
            
            
    !! CODIGO PARA RECIBIR TRAYECTORIAS 
                                            
            IF(receivedCoordinateThread="mt")THEN
                i:=1;
                control :="ok";
                SocketSend clientSocket, \Str:=control;
                SocketReceive clientSocket, \Str:=numpuntos;
                aux:=StrToVal(numpuntos, numeropuntos);
                SocketSend clientSocket, \Str:=control;            
                WHILE NOT(receivedCoordinateThread="fmt") DO
                   
                    SocketReceive clientSocket, \Str:=receivedCoordinateThread;
                    IF NOT (receivedCoordinateThread="fmt") THEN 
                        ProcessReceivedCoordinate(receivedCoordinateThread);
                        jmt1{i} :=[[J1, J2, J3, J4, J5, 0],[9E9, 9E9, 9E9, 9E9, 9E9, 9E9]];
                         IF i=1 THEN
                            !currentRobTarget := CRobT(\Tool:=tool0 \WObj:=wobj0);
                            currentPos := CJointT();
                            distance{i}:= CalcJointDistance(jmt1{i}, currentPos);
                        ENDIF
                        IF i>1 THEN
                            distance{i} := CalcJointDistance(jmt1{i}, jmt1{i-1});
                        ENDIF
                        
                         distance_:=ValToStr(distance{i});
                    SocketSend clientSocket, \Str:=distance_;
                    SocketReceive clientSocket, \Str:=velocidad_;
                    aux:=STrToVal(velocidad_,velocidades{i});
                    SocketSend clientSocket, \Str:=control;
                    
                    SocketReceive clientSocket, \Str:=tiempo_trayectoria;
                    aux:=STrToVal(tiempo_trayectoria,tiempo{i});
                    SocketSend clientSocket, \Str:=control;
                    ENDIF
                   

                    i:=i+1; 
                ENDWHILE
                  SocketSend clientSocket, \Str:=control;  
            ENDIF
            
        !! CODIGO PARA MOVER A PUNTO INICIAL TRAYECTORIA
            
             IF(receivedCoordinateThread="mti")THEN
                control :="ok";
                SocketSend clientSocket, \Str:=control; 
                jmt2:=jmt1{1};
                MoveAbsJ jmt1{1}, v200, z10, tool0;
             
             ENDIF
             
        !! CODIGO PARA MOVER TRAYECTORIA
            
             IF(receivedCoordinateThread="mtc")THEN
                control :="ok";
                SocketSend clientSocket, \Str:=control;        
                FOR i FROM 2 TO numeropuntos DO
                    IF velocidades{i}>0 THEN
                        v_dynamic := [velocidades{i}, velocidades{i}, velocidades{i}, velocidades{i}];
                        MoveAbsJ jmt1{i}, v_dynamic, z10, tool0;
                    ELSE
                        waitTime(tiempo{i});
                    ENDIF
                ENDFOR
             ENDIF
             
        !! CODIGO PARA RECIBIR PUNTOS
             IF(receivedCoordinateThread="envio_puntos")THEN
                 control :="ok";
                 SocketSend clientSocket, \Str:=control;
                 SocketReceive clientSocket, \Str:=numpuntos_totales_;
                 aux:=StrToVal(numpuntos_totales_, numpuntos_totales);
                 i:=1;
                 SocketSend clientSocket, \Str:=control;
                 WHILE(i<=numpuntos_totales) DO
                    SocketReceive clientSocket, \Str:=receivedCoordinateThread;
                    ProcessReceivedCoordinate(receivedCoordinateThread);
                    puntos_totales{i} :=[[J1, J2, J3, J4, J5, 0],[9E9, 9E9, 9E9, 9E9, 9E9, 9E9]];   
                    control :="ok";
                    SocketSend clientSocket, \Str:=control;
                    i:=i+1;  
                ENDWHILE
            ENDIF
            
        !! CODIGO PARA RECIBIR TRAYECTORIAS
        IF(receivedCoordinateThread="envio_trayectorias")THEN
                control :="ok";
                SocketSend clientSocket, \Str:=control;
                SocketReceive clientSocket, \Str:=numtrayectorias_totales_;
                aux:=StrToVal(numtrayectorias_totales_, numtrayectorias_totales);
                SocketSend clientSocket, \Str:=control;
                t:=1;
                WHILE t<=(numtrayectorias_totales) DO
                    SocketReceive clientSocket, \Str:=numpuntos;
                    aux:=StrToVal(numpuntos, numeropuntos);
                    SocketSend clientSocket, \Str:=control;
                    SocketReceive clientSocket, \Str:=tiempo_trayectoria;
                    aux:=StrToVal(tiempo_trayectoria, tiempo{t});
                    SocketSend clientSocket, \Str:=control;
                    info_trayectoria{t,1}:=numeropuntos;
                    info_trayectoria{t,2}:=tiempo{t};
                    i:=1;
                    WHILE i<=numeropuntos DO
                        SocketReceive clientSocket, \Str:=receivedCoordinateThread;
                        ProcessReceivedCoordinate(receivedCoordinateThread);
                        trayectorias_totales{t,i} :=[[J1, J2, J3, J4, J5, 0],[9E9, 9E9, 9E9, 9E9, 9E9, 9E9]];
                        
                        
                        IF i=1 THEN
                            !currentRobTarget := CRobT(\Tool:=tool0 \WObj:=wobj0);
                            currentPos := CJointT();
                            distancias_totales{t,i}:= CalcJointDistance( trayectorias_totales{t,i}, currentPos);
                        ENDIF
                        IF i>1 THEN
                            distancias_totales{t,i} := CalcJointDistance( trayectorias_totales{t,i},  trayectorias_totales{t,i-1});
                        ENDIF
                        
                        distance_:=ValToStr(distancias_totales{t,i});
                        SocketSend clientSocket, \Str:=distance_;
                        SocketReceive clientSocket, \Str:=velocidad_;
                        aux:=STrToVal(velocidad_,velocidades_totales{t,i});
                        SocketSend clientSocket, \Str:=control;
                    
                        i:=i+1; 
                    ENDWHILE
                    t:=t+1;
                ENDWHILE
            ENDIF
            
            !! CODIGO PARA RECIBIR PROGRAMA
            IF(receivedCoordinateThread="Envio_programa")THEN
                
                control :="ok";
                SocketSend clientSocket, \Str:=control;
                
                SocketReceive clientSocket, \Str:=numero_condiciones_;
                aux:=StrToVal(numero_condiciones_, numero_condiciones);
                SocketSend clientSocket, \Str:=control;
                i:=1;
                WHILE i<=numero_condiciones DO
                    
                    SocketReceive clientSocket, \Str:=tipo_condicion_{i};
                    aux:=StrToVal(tipo_condicion_{i}, tipo_condicion{i});
                    SocketSend clientSocket, \Str:=control;
                
                    SocketReceive clientSocket, \Str:=valor_x_{i};
                    aux:=StrToVal(valor_x_{i}, valor_x{i});
                    SocketSend clientSocket, \Str:=control;
                
                    SocketReceive clientSocket, \Str:=valor_y_{i};
                    aux:=StrToVal(valor_y_{i}, valor_y{i});
                    SocketSend clientSocket, \Str:=control;
                
                    SocketReceive clientSocket, \Str:=numero_acciones_{i};
                    aux:=StrToVal(numero_acciones_{i}, numero_acciones{i});
                    SocketSend clientSocket, \Str:=control;
                    
                    q:=1;
                    WHILE q<=numero_acciones{i} DO
                        
                        SocketReceive clientSocket, \Str:=tipo_accion_{i,q};
                        !!aux:=StrToVal(tipo_accion_{i,q}, tipo_accion{i,q});
                        SocketSend clientSocket, \Str:=control;
                        
                        SocketReceive clientSocket, \Str:=numero_accion_{i,q};
                        aux:=StrToVal(numero_accion_{i,q}, numero_accion{i,q});
                        SocketSend clientSocket, \Str:=control;                       
                        
                        q:=q+1;
                    ENDWHILE
                    i:=i+1;
                ENDWHILE
                
                i:=1;
                auxvar:=i;
                WHILE i<=numero_condiciones DO
                    
                    IF(auxvar=i) THEN
                        SocketReceive clientSocket, \Str:=entrada_{1};
                        SocketSend clientSocket, \Str:=control;
                    
                        SocketReceive clientSocket, \Str:=entrada_{2};
                        SocketSend clientSocket, \Str:=control;
                    
                        SocketReceive clientSocket, \Str:=entrada_{3};
                        SocketSend clientSocket, \Str:=control;
                    
                        SocketReceive clientSocket, \Str:=entrada_{4};  
                        SocketSend clientSocket, \Str:=control;
                    
                        SocketReceive clientSocket, \Str:=entrada_{5};
                        SocketSend clientSocket, \Str:=control;
                    ENDIF
                    auxvar:=i;
                    
                    IF tipo_condicion{i}=0 THEN
                        q:=1;
                        WHILE q<=valor_x{i} DO
                            t:=1;
                            WHILE t<=numero_acciones{i} DO
                                IF tipo_accion_{i,t}="Mover a punto" THEN
                                    MoveAbsJ  puntos_totales{numero_accion{i,t}+1}, v800, z10, tool0;
                                ENDIF
                                IF tipo_accion_{i,t}="Mover trayectoria" THEN
                                    
                                     jmt2:=jmt1{1};
                                      v_dynamic:=[190,190,190,190];
                                     MoveAbsJ  trayectorias_totales{numero_accion{i,t}+1,1},  v_dynamic, z10, tool0;
                                     k:=2;
                                     WHILE k < info_trayectoria{numero_accion{i,t}+1,1} DO
                                        IF velocidades_totales{numero_accion{i,t}+1,k}>0 THEN
                                         v_dynamic := [velocidades_totales{numero_accion{i,t}+1,k}, velocidades_totales{numero_accion{i,t}+1,k}, velocidades_totales{numero_accion{i,t}+1,k}, velocidades_totales{numero_accion{i,t}+1,k}];
                                        MoveAbsJ  trayectorias_totales{numero_accion{i,t}+1,k}, v_dynamic, z10, tool0;
                                        
                                        !waitTime(0.05);
                                        ENDIF
                                        k:=k+1;
                                     ENDWHILE
                                    
                                    !MoveAbsJ trayectorias_totales{numero_accion{i,t}+1,1}, v800, z10, tool0;
                                    !MoveAbsJ trayectorias_totales{numero_accion{i,t}+1,1}, v_dynamic, z10, tool0;
                                ENDIF
                                IF tipo_accion_{i,t}="Activar salida" THEN
                                    SetDO Salida{numero_accion{i,t}},1;
                                ENDIF
                                t:=t+1;
                            ENDWHILE
                            q:=q+1;
                        ENDWHILE
                        i:=i+1;
                    ENDIF
                    IF tipo_condicion{i}=1 THEN
                        IF (entrada_{valor_y{i}+1}="1") THEN
                          t:=1;
                            WHILE t<=numero_acciones{i} DO
                                IF tipo_accion_{i,t}="Mover a punto" THEN
                                    MoveAbsJ  puntos_totales{numero_accion{i,t}+1}, v800, z10, tool0;
                                ENDIF
                                IF tipo_accion_{i,t}="Mover trayectoria" THEN
                                    jmt2:=jmt1{1};
                                      v_dynamic:=[190,190,190,190];
                                     MoveAbsJ  trayectorias_totales{numero_accion{i,t}+1,1},  v_dynamic, z10, tool0;
                                     k:=2;
                                     WHILE k < info_trayectoria{numero_accion{i,t}+1,1} DO
                                        IF velocidades_totales{numero_accion{i,t}+1,k}>0 THEN
                                         v_dynamic := [velocidades_totales{numero_accion{i,t}+1,k}, velocidades_totales{numero_accion{i,t}+1,k}, velocidades_totales{numero_accion{i,t}+1,k}, velocidades_totales{numero_accion{i,t}+1,k}];
                                        MoveAbsJ  trayectorias_totales{numero_accion{i,t}+1,k}, v_dynamic, z10, tool0;
                                        
                                        !waitTime(0.05);
                                        ENDIF
                                        k:=k+1;
                                     ENDWHILE
                                ENDIF
                                IF tipo_accion_{i,t}="Activar salida" THEN
                                    SetDO Salida{numero_accion{i,t}},1;
                                ENDIF
                                t:=t+1;
                            ENDWHILE
                            i:=i+1;
                        ENDIF
                    ENDIF
                    IF tipo_condicion{i}=2 THEN
                        IF (Entrada{valor_y{i}}=0 OR entrada_{1}="0") THEN
                            t:=1;
                            WHILE t<=numero_acciones{i} DO
                                IF tipo_accion_{i,t}="Mover a punto" THEN
                                    MoveAbsJ  puntos_totales{numero_accion{i,t}+1}, v800, z10, tool0;
                                ENDIF
                                IF tipo_accion_{i,t}="Mover trayectoria" THEN
                                   jmt2:=jmt1{1};
                                      v_dynamic:=[190,190,190,190];
                                     MoveAbsJ  trayectorias_totales{numero_accion{i,t}+1,1},  v_dynamic, z10, tool0;
                                     k:=2;
                                     WHILE k < info_trayectoria{numero_accion{i,t}+1,1} DO
                                        IF velocidades_totales{numero_accion{i,t}+1,k}>0 THEN
                                         v_dynamic := [velocidades_totales{numero_accion{i,t}+1,k}, velocidades_totales{numero_accion{i,t}+1,k}, velocidades_totales{numero_accion{i,t}+1,k}, velocidades_totales{numero_accion{i,t}+1,k}];
                                        MoveAbsJ  trayectorias_totales{numero_accion{i,t}+1,k}, v_dynamic, z10, tool0;
                                        
                                        !waitTime(0.05);
                                        ENDIF
                                        k:=k+1;
                                     ENDWHILE
                                ENDIF
                                IF tipo_accion_{i,t}="Activar salida" THEN
                                    SetDO Salida{numero_accion{i,t}+1},1;
                                ENDIF
                                t:=t+1;
                            ENDWHILE
                            i:=i+1;
                        ENDIF
                    ENDIF
                    IF tipo_condicion{i}=3 THEN
                        IF ( entrada_{1}="1") THEN
                            q:=1;
                            WHILE q<=valor_x{i} DO
                            t:=1;
                            WHILE t<=numero_acciones{i} DO
                                IF tipo_accion_{i,t}="Mover a punto" THEN
                                    MoveAbsJ  puntos_totales{numero_accion{i,t}+1}, v150, z10, tool0;
                                ENDIF
                                IF tipo_accion_{i,t}="Mover trayectoria" THEN
                                     jmt2:=jmt1{1};
                                      v_dynamic:=[190,190,190,190];
                                     MoveAbsJ  trayectorias_totales{numero_accion{i,t}+1,1},  v_dynamic, z10, tool0;
                                     k:=2;
                                     WHILE k < info_trayectoria{numero_accion{i,t}+1,1} DO
                                        IF velocidades_totales{numero_accion{i,t}+1,k}>0 THEN
                                         v_dynamic := [velocidades_totales{numero_accion{i,t}+1,k}, velocidades_totales{numero_accion{i,t}+1,k}, velocidades_totales{numero_accion{i,t}+1,k}, velocidades_totales{numero_accion{i,t}+1,k}];
                                        MoveAbsJ  trayectorias_totales{numero_accion{i,t}+1,k}, v200, z10, tool0;
                                        
                                        !waitTime(0.05);
                                        ENDIF
                                        k:=k+1;
                                     ENDWHILE
                                ENDIF
                                IF tipo_accion_{i,t}="Activar salida" THEN
                                    SetDO Salida{numero_accion{i,t}+1},1;
                                ENDIF
                                t:=t+1;
                            ENDWHILE
                            q:=q+1;
                            ENDWHILE
                            i:=i+1;
                        ENDIF
                    ENDIF
                    IF tipo_condicion{i}=4 THEN
                         IF (Entrada{valor_y{i}}=0 OR entrada_{1}="0") THEN
                        q:=1;
                        WHILE q<=valor_x{i} DO
                             t:=1;
                            WHILE t<=numero_acciones{i} DO
                                IF tipo_accion_{i,t}="Mover a punto" THEN
                                    MoveAbsJ  puntos_totales{numero_accion{i,t}+1}, v800, z10, tool0;
                                ENDIF
                                IF tipo_accion_{i,t}="Mover trayectoria" THEN
                                     jmt2:=jmt1{1};
                                      v_dynamic:=[190,190,190,190];
                                     MoveAbsJ  trayectorias_totales{numero_accion{i,t}+1,1},  v_dynamic, z10, tool0;
                                     k:=2;
                                     WHILE k < info_trayectoria{numero_accion{i,t}+1,1} DO
                                        IF velocidades_totales{numero_accion{i,t}+1,k}>0 THEN
                                         v_dynamic := [velocidades_totales{numero_accion{i,t}+1,k}, velocidades_totales{numero_accion{i,t}+1,k}, velocidades_totales{numero_accion{i,t}+1,k}, velocidades_totales{numero_accion{i,t}+1,k}];
                                        MoveAbsJ  trayectorias_totales{numero_accion{i,t}+1,k}, v_dynamic, z10, tool0;
                                        
                                        !waitTime(0.05);
                                        ENDIF
                                        k:=k+1;
                                     ENDWHILE
                                ENDIF
                                IF tipo_accion_{i,t}="Activar salida" THEN
                                    SetDO Salida{numero_accion{i,t}+1},1;
                                ENDIF
                                t:=t+1;
                            ENDWHILE
                            q:=q+1;
                        ENDWHILE
                        i:=i+1;
                         ENDIF
                    ENDIF
                ENDWHILE    
                 TPWrite "Programa terminado";
            ENDIF
            
        !! CODIGO PARA CERRRA COMUNICACION
             IF(receivedCoordinateThread="Close") THEN
                 ActiveUDP := FALSE;
                 SocketClose clientSocket;
                 SocketClose serverSocket;
                 WaitTime(1);
            ENDIF

         ENDWHILE

        ! Detener el hilo UDP al salir
        ActiveUDP := FALSE;
        SocketClose clientSocket;
        SocketClose serverSocket;
        WaitTime(1);

    ENDPROC
    
    PROC ProcessReceivedTime(string tiempo_)
        VAR num numero;
        C1 :=FALSE;
        pos1:=StrFind(tiempo_,1,",");
        jp1:=StrPart(tiempo_, 1, pos1-1);
        IF (END_OF_LIST-pos1>1)THEN
        jp2:=StrPart(tiempo_, pos1+1, END_OF_LIST);
        ENDIF
        
        cad1:=jp1+"."+jp2;
        C1 := StrToVal(cad1, numero);
        tiempo{i}:=numero;
        
        
        
        
    ENDPROC
    
     PROC ProcessReceivedCoordinate(string dataCoordinate)
        
        C1 :=FALSE;
        C2 :=FALSE;
        C3 :=FALSE;
        C4 :=FALSE;
        C5 :=FALSE;
        C6 :=FALSE;
        
        !pos1:=StrFind(receivedCoordinateThread,1,";");
        pos1:=StrFind(receivedCoordinateThread,1,";");
        pos2:=StrFind(receivedCoordinateThread,pos1+1,";");
        pos3:=StrFind(receivedCoordinateThread,pos2+1,";");
        pos4:=StrFind(receivedCoordinateThread,pos3+1,";");
        pos5:=StrFind(receivedCoordinateThread,pos4+1,";");
        pos6:=StrFind(receivedCoordinateThread,pos5+1,";");
        
        jp1:=StrPart(receivedCoordinateThread, 1, pos1-1);
        jp2:=StrPart(receivedCoordinateThread, pos1+1, pos2-pos1-1);
        jp3:=StrPart(receivedCoordinateThread, pos2+1, pos3-pos2-1);
        jp4:=StrPart(receivedCoordinateThread, pos3+1, pos4-pos3-1);
        jp5:=StrPart(receivedCoordinateThread, pos4+1, pos5-pos4-1);
        jp6:=StrPart(receivedCoordinateThread, pos5+1, pos6-pos5-1);
        
        posx1:=StrFind(jp1,1,",");
        posx2:=StrFind(jp2,1,",");
        posx3:=StrFind(jp3,1,",");
        posx4:=StrFind(jp4,1,",");
        posx5:=StrFind(jp5,1,",");
        posx6:=StrFind(jp6,1,",");
        
        jx11:=StrPart(jp1, 1, posx1-1);
         IF (pos1-posx1>1)THEN
        jx12 := StrPart(jp1, posx1 + 1, pos1 - posx1 - 1);
         ENDIF
         
        jx21:=StrPart(jp2, 1, posx2-1);
        IF (pos2-pos1-posx2>1)THEN 
        jx22:=StrPart(jp2, posx2+1, pos2-pos1-1-posx2);
        ENDIF
                
        jx31:=StrPart(jp3, 1, posx3-1);
        IF (pos3-pos2-posx3>1)THEN
        jx32:=StrPart(jp3, posx3+1, pos3-pos2-1-posx3);
        ENDIF
        
        jx41:=StrPart(jp4, 1, posx4-1);
        IF (pos4-pos3-posx4>1)THEN
        jx42:=StrPart(jp4, posx4+1, pos4-pos3-1-posx4);
        ENDIF
        
        
        jx51:=StrPart(jp5, 1, posx5-1);
        IF (pos5-pos4-posx5>1)THEN
        jx52:=StrPart(jp5, posx5+1, pos5-pos4-1-posx5);
        ENDIF
        
        jx61:=StrPart(jp6, 1, posx6-1);
        IF (pos6-pos5-posx6>1)THEN
        jx62:=StrPart(jp6, posx6+1, pos6-pos5-1-posx6);
        ENDIF
        
        cad1:=jx11+"."+jx12;
        cad2:=jx21+"."+jx22;
        cad3:=jx31+"."+jx32;
        cad4:=jx41+"."+jx42;
        cad5:=jx51+"."+jx52;
        cad6:=jx61+"."+jx62;
        
        WHILE NOT C1 DO
        C1 := StrToVal(cad1, J1);
            IF (J1>178)THEN
                J1:=J1-360;
            ENDIF
        ENDWHILE
        
        WHILE (C2=FALSE) DO
        C2 := StrToVal(cad2, J2);
            IF (J2>87)THEN
                J2:=J2-360;
            ENDIF
        ENDWHILE
        
        WHILE (C3=FALSE) DO
        C3 := StrToVal(cad3, J3);
            IF (J3>227)THEN 
                J3:=J3-360;
            ENDIF
        ENDWHILE
        
        WHILE (C4=FALSE) DO
        C4 := StrToVal(cad4, J4);
            IF (J4>197)THEN
                J4:=J4-360;
            ENDIF
        ENDWHILE
        
        WHILE (C5=FALSE) DO
        C5 := StrToVal(cad5, J5);
            IF (J5>112)THEN
                J5:=J5-360;
            ENDIF
        ENDWHILE
        
        WHILE (C6=FALSE) DO
        C6 := StrToVal(cad6, J6);
            IF (J6>357)THEN
                J6:=J6-360;
            ENDIF
        ENDWHILE
        
    ENDPROC
    
    
    
     FUNC num CalcJointDistance( jointtarget jmt1,  jointtarget jmt2)
        ! Calcular la distancia euclidiana entre dos jointtargets
        VAR robtarget rt1;
        VAR robtarget rt2;
        VAR num dx;
        VAR num dy;
        VAR num dz;
        VAR num dist;
        
        rt1 := CalcRobT(jmt1, tool0);
        rt2 := CalcRobT(jmt2, tool0);
        dx := rt2.trans.x - rt1.trans.x;
        dy := rt2.trans.y - rt1.trans.y;
        dz := rt2.trans.z - rt1.trans.z;
        dist := SQRT(dx*dx + dy*dy + dz*dz);
        RETURN dist;
    ENDFUNC
    
ENDMODULE