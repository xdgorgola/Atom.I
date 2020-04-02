using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LifeSystem : MonoBehaviour
{
    [SerializeField]
    private int hp = 3;

    public UnityEvent onLifeChange = new UnityEvent();
    public UnityEvent onLifeDepleted = new UnityEvent();

    private void Start()
    {
        BoxManager.Container.onFailedIsolation.AddListener(ReduceHP);
    }

    private void ReduceHP()
    {
        hp -= 1;
        onLifeChange.Invoke();
        if (hp <= 0)
        {
            onLifeDepleted.Invoke();
        }
    }
}
