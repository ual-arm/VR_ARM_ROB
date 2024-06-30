using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class creacion_texto_programa : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject textContainer;
    public void escribir_acciones(int numero, string tipoaccion, int posicion)
    {
        float aux;
        string part1;
        int part2;
        GameObject textoGO = new GameObject("TextoMeshPro");
        textoGO.transform.parent = transform;
        TextMeshProUGUI textoMeshPro = textoGO.AddComponent<TextMeshProUGUI>();


        part2 = numero;

        if (tipoaccion == "Mover a punto")
        {
            part1 = "Punto ";
            if (part2 == 0)
            {
                textoMeshPro.text = part1 + "Home";
            }
            else {
                textoMeshPro.text = part1 + part2;
            }

        }
        if (tipoaccion == "Mover trayectoria")
        {
            part1 = "Trayectoria ";
            part2 = part2 + 1;
            textoMeshPro.text = part1 + part2;
        }
        if (tipoaccion == "Activar salida")
        {
            part1 = "Salida ";
            part2 = part2 + 1;
            textoMeshPro.text = part1 + part2;
        }

        // Configurar el texto y otros ajustes
        // Texto que se mostrará
        textoMeshPro.fontSize = 16; // Tamaño de fuente
        textoMeshPro.color = Color.white; // Color del texto

        // Opcional: establecer la posición, la escala y el padre del GameObject del texto

        aux = 2.2f-0.1f*posicion;
        //textoGO.transform.localPosition = new Vector3(31.3f, aux, -234.78f); // Posición en el mundo
        textoGO.transform.localPosition = new Vector3(0.1f, aux, 0.4f);
        textoGO.transform.localScale = new Vector3(0.004f,0.004f,0.004f); // Escala del texto
        textoGO.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        textoGO.transform.SetParent(transform); // Establecer el padre del texto

        // Asegúrate de que el texto sea visible en el juego estableciendo el canvas como su padre
        // (Esto es necesario si el objeto de texto está destinado a ser visible en el juego)
        Canvas canvas = gameObject.GetComponent<Canvas>();
        if (canvas == null)
        {
            canvas = gameObject.AddComponent<Canvas>(); // Agregar un componente de Canvas si no existe
            canvas.renderMode = RenderMode.ScreenSpaceOverlay; // Establecer el modo de renderizado
        }
        textoGO.transform.SetParent(canvas.transform); // Establecer el canvas como padre del texto
    }
    public void escribir_condiciones(int condicion, float x, int y, int posicion)
    {
        float aux;
        string condicionSeleccionada;
        string valorX;
        string valorY;
        bool hayX=false;
        bool hayY=false;

        GameObject textoGO = new GameObject("TextoMeshProCondicion");
        textoGO.transform.parent = transform;
        TextMeshProUGUI textoMeshPro = textoGO.AddComponent<TextMeshProUGUI>();

        GameObject textoGOx = new GameObject("TextoMeshProCondicionx");
        textoGOx.transform.parent = transform;
        TextMeshProUGUI textoMeshProx = textoGOx.AddComponent<TextMeshProUGUI>();

        GameObject textoGOy = new GameObject("TextoMeshProCondiciony");
        textoGOy.transform.parent = transform;
        TextMeshProUGUI textoMeshProy = textoGOy.AddComponent<TextMeshProUGUI>();

        switch (condicion)
        {
            case 0:
                condicionSeleccionada = "Repetir      veces";
                hayX = true;
                break;
            case 1:
                condicionSeleccionada = "Inicio   con  entrada  ";
                hayY = true;
                break;
            case 2:
                condicionSeleccionada = "Inicio   cuando  no    ";
                hayY = true;
                break;
            case 3:
                condicionSeleccionada = "Repetir       cuando   ";
                hayY = true;
                hayX = true;
                break;
            case 4:
                condicionSeleccionada = "Repetir      cuando no ";
                hayY = true;
                hayX = true;
                break;
            default:
                condicionSeleccionada = "Condición desconocida";
                break;
        }



        textoMeshPro.text = condicionSeleccionada;
        textoMeshPro.fontSize = 16; // Tamaño de fuente
        textoMeshPro.color = Color.white; // Color del texto
        aux = 2.2f - 0.1f * posicion;
        textoGO.transform.localPosition = new Vector3(0.05f, aux, 0.4f); // Posición en el mundo
        textoGO.transform.localScale = new Vector3(0.004f, 0.004f, 0.004f); // Escala del texto
        textoGO.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        textoGO.transform.SetParent(transform); // Establecer el padre del texto

        if (hayX)
        {
            valorX = x.ToString();
            textoMeshProx.text = valorX;
            textoMeshProx.fontSize = 16; // Tamaño de fuente
            textoMeshProx.color = Color.white; // Color del texto
           
            textoGOx.transform.localPosition = new Vector3(0.28f, aux, 0.4f); // Posición en el mundo
            textoGOx.transform.localScale = new Vector3(0.004f, 0.004f, 0.004f); // Escala del texto
            textoGOx.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            textoGOx.transform.SetParent(transform); // Establecer el padre del texto
        }

        if (hayY)
        {
            valorY = (y+1).ToString();
            textoMeshProy.text = valorY;
            textoMeshProy.fontSize = 16; // Tamaño de fuente
            textoMeshProy.color = Color.white; // Color del texto
            
            textoGOy.transform.localPosition = new Vector3(0.7f, aux, 0.4f); // Posición en el mundo
            textoGOy.transform.localScale = new Vector3(0.004f, 0.004f, 0.004f); // Escala del texto
            textoGOy.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            textoGOy.transform.SetParent(transform); // Establecer el padre del texto
        }

        // Asegúrate de que el texto sea visible en el juego estableciendo el canvas como su padre
        // (Esto es necesario si el objeto de texto está destinado a ser visible en el juego)
        Canvas canvas = gameObject.GetComponent<Canvas>();
        if (canvas == null)
        {
            canvas = gameObject.AddComponent<Canvas>(); // Agregar un componente de Canvas si no existe
            canvas.renderMode = RenderMode.ScreenSpaceOverlay; // Establecer el modo de renderizado
        }
        textoGO.transform.SetParent(canvas.transform); // Establecer el canvas como padre del texto
    }

    public void BorrarTextos()
    {
        // Obtener todos los hijos del GameObject contenedor
        foreach (Transform child in textContainer.transform)
        {
            if (child.name == "TextoMeshPro")
            {
                // Destruir el GameObject del texto
                Destroy(child.gameObject);
            }
            if (child.name == "TextoMeshProCondicion")
            {
                // Destruir el GameObject del texto
                Destroy(child.gameObject);
            }
            if (child.name == "TextoMeshProCondicionx")
            {
                // Destruir el GameObject del texto
                Destroy(child.gameObject);
            }
            if (child.name == "TextoMeshProCondiciony")
            {
                // Destruir el GameObject del texto
                Destroy(child.gameObject);
            }
        }
    }
}
