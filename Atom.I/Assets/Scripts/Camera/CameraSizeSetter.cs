using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraSizeSetter : MonoBehaviour
{
    public float wantedX = 10;
    public float wantedY = 10;
    private float yq;

    private void Start()
    {
        if (GameManagerScript.Manager != null)
        {
            wantedX = GameManagerScript.Manager.x;
            wantedY = GameManagerScript.Manager.y;
        }
        SetCameraSize(wantedX, wantedY);
        float quarter = Screen.height * 0.13f;
        float size = Mathf.Abs((Camera.main.ScreenToWorldPoint(Vector3.up * quarter) - Camera.main.ScreenToWorldPoint(Vector3.zero)).magnitude);
        yq = Camera.main.ScreenToWorldPoint(Vector3.up * quarter).y;
        //Debug.Log("Y deseada: " + wantedY);
        //Debug.Log("Size: " + size);

        SetCameraSize(wantedX, wantedY + size * 2);
    }

    private void Update()
    {
        float actX = Screen.width;
        float actY = Screen.height;
        if (actX != wantedX || actY != wantedY) SetCameraSize(wantedX, wantedY);
    }

    public void SetCameraSize(float newX, float newY)
    {
        wantedX = newX;
        wantedY = newY;
        //Debug.Log(Screen.width);
        float screenRatio = (float)Screen.width / (float)Screen.height; // Que tanto X hay por cada Y actualmente
        float targetRatio = (float)wantedX / (float)wantedY; // Que tanto X quiero que haya por Y


        if (screenRatio >= targetRatio) // Si hay mas de lo que quiero, me asegura que alcance si seteo la Y nada mas (creo?)
        {
            Camera.main.orthographicSize = wantedY / 2;
        }
        else // Si no, calculamos la diferencia
        {
            float differenceInSize = targetRatio / screenRatio; // Que tanto de lo que quiero es lo que tengo
            Camera.main.orthographicSize = (wantedY / 2) * differenceInSize;
        }
    }
}