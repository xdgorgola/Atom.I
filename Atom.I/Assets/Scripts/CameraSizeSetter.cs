using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraSizeSetter : MonoBehaviour
{
    public float x = 10;
    public float y = 10;

    void Update()
    {
        float screenRatio = (float)Screen.width / (float)Screen.height; // Que tanto X hay por cada Y actualmente
        float targetRatio = x / y; // Que tanto X quiero que haya por Y

        if (screenRatio >= targetRatio) // Si hay mas de lo que quiero, me asegura que alcance si seteo la Y
        {
            Camera.main.orthographicSize = y / 2;
        }
        else // Si no, calculamos la diferencia
        {
            float differenceInSize = targetRatio / screenRatio; // Que tanto de lo que quiero es lo que tengo
            Camera.main.orthographicSize = (y / 2) * differenceInSize;
        }
    }
}