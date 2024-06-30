using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Esconder_teclado : MonoBehaviour
{
    public Canvas teclado;
    
    // Start is called before the first frame update
    public void Esconder()
    {
       teclado.enabled=false;
    }


}
