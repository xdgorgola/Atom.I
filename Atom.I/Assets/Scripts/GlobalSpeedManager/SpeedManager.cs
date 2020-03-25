using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpeedEvent : UnityEvent<float> { }

public class SpeedManager : MonoBehaviour
{
    public static SpeedManager Manager { get; private set; }

    [SerializeField]
    private float lastStrictSpeed = 5;
    [SerializeField]
    private float lastAddSpeed = 2;

    public SpeedEvent onStrictSpeedChange = new SpeedEvent();
    public SpeedEvent onAddSpeed = new SpeedEvent();

    private void Awake()
    {
        if (Manager != null && Manager != this)
        {
            Debug.LogWarning("SpeedManager ya existe! Eliminando este...", gameObject);
            Destroy(gameObject);
        }
        Manager = this;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            AddAtomSpeed(lastAddSpeed);
        }    
        else if (Input.GetKeyDown(KeyCode.I))
        {
            ChangeAtomSpeed(lastStrictSpeed);
        }
    }


    public void ChangeAtomSpeed(float newSpeed)
    {
        onStrictSpeedChange.Invoke(newSpeed);
        lastStrictSpeed = newSpeed;
    }


    public void AddAtomSpeed(float toAdd)
    {
        lastAddSpeed = toAdd;
        onAddSpeed.Invoke(toAdd);
    }
}
