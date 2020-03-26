using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomSpawner : MonoBehaviour
{
    private float w;
    private float h;

    [SerializeField]
    private int maxAtoms = 40;
    [SerializeField]
    private int initialAtoms = 30;

    private List<GameObject> activeAtoms;


    private void Start()
    {
        h = Camera.main.orthographicSize * 2;
        w = ((float)Screen.width / (float)Screen.height) * h;
    }


    void InitialSpawn()
    {
        for (int i = 0; i < initialAtoms; i++)
        {
            GameObject spawned = AtomPool.FreeAtom;
            activeAtoms.Add(spawned);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, Vector3.up * h + Vector3.right * w + Vector3.forward);
    }
}
