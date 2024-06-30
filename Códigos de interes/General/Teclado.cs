using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TecladoNumerico : MonoBehaviour
{

    public TMP_InputField campoEntrada;

    public void AgregarNumero(int numero)
    {
        campoEntrada.text += numero.ToString();
    }

    public void BorrarUltimo()
    {
        if (!string.IsNullOrEmpty(campoEntrada.text))
        {
            campoEntrada.text = campoEntrada.text.Substring(0, campoEntrada.text.Length - 1);
        }
    }

    public void ConfirmarNumeros()
    {
        // Aqu� puedes realizar la acci�n deseada con los n�meros introducidos
        string numerosIntroducidos = campoEntrada.text;
        
        Debug.Log("N�meros introducidos: " + numerosIntroducidos);

        // Por ejemplo, puedes utilizar los n�meros introducidos para realizar alguna operaci�n o acci�n
        // Luego, puedes limpiar el campo de entrada si es necesario
        //LimpiarCampoEntrada();
    }

    private void LimpiarCampoEntrada()
    {
        campoEntrada.text = "";
    }

    public void AgregarCero()
    {
        AgregarNumero(0);
    }

    public void AgregarUno()
    {
        AgregarNumero(1);
    }

    public void AgregarDos()
    {
        AgregarNumero(2);
    }

    public void AgregarTres()
    {
        AgregarNumero(3);
    }

    public void AgregarCuatro()
    {
        AgregarNumero(4);
    }

    public void AgregarCinco()
    {
        AgregarNumero(5);
    }

    public void AgregarSeis()
    {
        AgregarNumero(6);
    }

    public void AgregarSiete()
    {
        AgregarNumero(7);
    }

    public void AgregarOcho()
    {
        AgregarNumero(8);
    }

    public void AgregarNueve()
    {
        AgregarNumero(9);
    }
}
