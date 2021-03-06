﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CounterUI : MonoBehaviour
{
    public Text counterText = null;

    private void Start()
    {
        if (counterText == null)
        {
            Debug.LogWarning("No hay texto para el counter de tiempo");
        }
        if (GameManagerScript.Manager != null)
        {
            GameManagerScript.Manager.onCounterReduced.AddListener(UpdateCounter);
        }
        else
        {
            Debug.LogWarning("No hay GameStateManager para el contador de texto");
        }
    }

    private void UpdateCounter(int time)
    {
        int minutes = time / 60;
        int seconds = time - minutes * 60;
        string xd = (seconds < 10 ? "0" + seconds : seconds.ToString());
        counterText.text = minutes + ":" + xd; 
    }
}
