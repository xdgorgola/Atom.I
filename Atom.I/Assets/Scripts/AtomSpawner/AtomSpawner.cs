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
        activeAtoms = new List<GameObject>(maxAtoms);

        // Settear la camara aca por si acaso
        //Camera.main.GetComponent<CameraSizeSetter>().SetCameraSize(10, 10);

        h = Camera.main.orthographicSize * 2;
        w = ((float)Screen.width / (float)Screen.height) * h;
        InitialSpawn();
    }


    void InitialSpawn()
    {
        for (int i = 0; i < initialAtoms; i++)
        {
            GameObject spawned = AtomPool.FreeAtom;
            activeAtoms.Add(spawned);
            spawned.transform.position = Vector2.right * Random.Range(-1f, 1f) * (w / 2) + Vector2.up * Random.Range(-1f, 1f) * (h / 2);
            Vector2 direction = Random.insideUnitCircle.normalized;
            spawned.GetComponent<AtomMovement>().InitializeAtom(direction, SpeedManager.Manager.GetSpeedRange());
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, Vector3.up * h + Vector3.right * w + Vector3.forward);
    }
}
