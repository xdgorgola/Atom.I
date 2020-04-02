using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public GameObject titulo;
    public GameObject botonesPrincipales;
    public GameObject textoCredito;
    public GameObject botonVolver;

    private void Start()
    {
        titulo.SetActive(true);
        botonesPrincipales.SetActive(true);
        textoCredito.SetActive(false);
        botonVolver.SetActive(false);
    }


    public void ActivarCreditos()
    {
        titulo.SetActive(false);
        botonesPrincipales.SetActive(false);
        textoCredito.SetActive(true);
        botonVolver.SetActive(true);
    }

    public void DesactivarCreditos()
    {
        titulo.SetActive(true);
        textoCredito.SetActive(false);
        botonVolver.SetActive(false);
        botonesPrincipales.SetActive(true);
    }
}
