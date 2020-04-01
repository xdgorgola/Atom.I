using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternalWalls : MonoBehaviour
{
    public GameObject topWall = null;
    public GameObject bottomWall = null;
    public GameObject leftWall = null;
    public GameObject rightWall = null;

    private float h;
    private float w;

    // Start is called before the first frame update
    void Start()
    {
        h = Camera.main.orthographicSize * 2;
        w = ((float)Screen.width / (float)Screen.height) * h;

        float x = GetComponent<CameraSizeSetter>().x;
        float y = GetComponent<CameraSizeSetter>().y;

        //topWall.transform.position = Vector2.up * (h / 2 + 0.5f);
        topWall.transform.position = Vector2.up * y / 2;
        //topWall.transform.localScale = Vector3.up + Vector3.forward + Vector3.right * w;
        topWall.transform.localScale = Vector3.up + Vector3.forward + Vector3.right * x;

        float quarter = Screen.height * 0.13f;
        float size = Mathf.Abs((Camera.main.ScreenToWorldPoint(Vector3.up * quarter) - Camera.main.ScreenToWorldPoint(Vector3.zero)).magnitude);
        Debug.Log(size);
        //bottomWall.transform.position = Vector2.up * (-h / 2 - 0.5f);
        bottomWall.transform.position = Camera.main.ScreenToWorldPoint(Vector3.up * (quarter / 2) + Vector3.right * (Screen.width / 2));
        bottomWall.transform.localScale = Vector3.up * size + Vector3.forward + Vector3.right * w;


        //rightWall.transform.position = Vector2.right * (w / 2 + 0.5f);
        rightWall.transform.position = Vector2.right * (x / 2);
        rightWall.transform.localScale = Vector3.up + Vector3.forward + Vector3.right * y;

        //leftWall.transform.position = Vector2.right * (-w / 2 - 0.5f);
        leftWall.transform.position = Vector2.right * (-x / 2);
        leftWall.transform.localScale = Vector3.up + Vector3.forward + Vector3.right * h;
    }
}
