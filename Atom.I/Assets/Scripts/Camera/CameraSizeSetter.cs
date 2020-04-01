using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraSizeSetter : MonoBehaviour
{
    public float x = 10;
    public float y = 10;

    private void Awake()
    {
        SetCameraSize(x, y);
    }

    private void Update()
    {
        float actX = Screen.width;
        float actY = Screen.height;
        if (actX != x || actY != y) SetCameraSize(x, y);
    }

    public void SetCameraSize(float newX, float newY)
    {
        x = newX;
        y = newY;
        //Debug.Log(Screen.width);
        float screenRatio = (float)Screen.width / (float)Screen.height; // Que tanto X hay por cada Y actualmente
        float targetRatio = (float)x / (float)y; // Que tanto X quiero que haya por Y


        if (screenRatio >= targetRatio) // Si hay mas de lo que quiero, me asegura que alcance si seteo la Y nada mas (creo?)
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