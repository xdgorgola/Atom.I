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

        topWall.transform.position = Vector2.up * (h / 2 + 0.5f);
        bottomWall.transform.position = Vector2.up * (-h / 2 - 0.5f);
        rightWall.transform.position = Vector2.right * (w / 2 + 0.5f);
        leftWall.transform.position = Vector2.right * (-w / 2 - 0.5f);
    }
}
