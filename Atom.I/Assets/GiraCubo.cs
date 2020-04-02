using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiraCubo : MonoBehaviour
{
    public Vector2 axis;

    private void Start()
    {
        axis = Random.insideUnitSphere.normalized;   
    }

    private void Update()
    {
        transform.Rotate(axis, 1);
    }
}
