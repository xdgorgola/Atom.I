﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class AtomSpawnerCounter : MonoBehaviour
{
    public static AtomSpawnerCounter Manager { get; private set; }

    private float spawnX;
    private float spawnY;
    private float yq;

    [SerializeField]
    private int maxAtoms = 40;
    [SerializeField]
    private int initialAtoms = 30;
    [SerializeField]
    private int levelAnti = 20;

    private List<GameObject> activeAtoms;
    private List<GameObject> activeAnti;

    [HideInInspector]
    public UnityEvent onNoMoreAnti = new UnityEvent();

    private void Awake()
    {
        if (Manager != null && Manager != this)
        {
            Debug.LogWarning("Ya hay un AtomSpawnerCounter, eliminando este...", gameObject);
            Destroy(gameObject);
        }
        Manager = this;
    }

 
    private void Start()
    {
        if (GameManagerScript.Manager != null)
        {
            spawnX = GameManagerScript.Manager.x;
            spawnY = GameManagerScript.Manager.y;
        }

        if (BoxManager.Container != null)
        {
            BoxManager.Container.onAntiIsolated.AddListener(ReduceAnti);
        }

        activeAtoms = new List<GameObject>(maxAtoms);
        activeAnti = new List<GameObject>(levelAnti);


        InitialSpawn();
    }


    private void InitialSpawn()
    {
        for (int i = 0; i < initialAtoms; i++)
        {
            SpawnAtom(Atoms.Atom).GetComponent<AtomMovement>().StopMovement();
        }

        for (int i = 0; i < levelAnti; i++)
        {
            SpawnAtom(Atoms.Anti).GetComponent<AtomMovement>().StopMovement();
        }
    }

    private GameObject SpawnAtom(Atoms tipo)
    {
        GameObject spawned = AtomPool.Pool.GetAtom(tipo);
        switch (tipo)
        {
            case Atoms.Anti:
                activeAnti.Add(spawned);
                break;
            case Atoms.Atom:
                activeAtoms.Add(spawned);
                break;
        }
        spawned.transform.position = Vector2.right * Random.Range(-0.8f, 0.8f) * (spawnX / 2) + Vector2.up * Random.Range(-(spawnY / 2) + 0.8f, (spawnY / 2) - 0.8f);
        Vector2 direction = Random.insideUnitCircle.normalized;
        spawned.GetComponent<AtomMovement>().InitializeAtom(direction, SpeedManager.Manager.GetSpeedRange());
        return spawned;
    }

    private void ReduceAnti(GameObject anti)
    {
        activeAtoms.Remove(anti);
        levelAnti -= 1;
        if (levelAnti == 0)
        {
            onNoMoreAnti.Invoke();  
        }
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, Vector3.up * spawnY + Vector3.right * spawnX + Vector3.forward);
    }
#endif
}
