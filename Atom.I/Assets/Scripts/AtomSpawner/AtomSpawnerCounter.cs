using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class AtomSpawnerCounter : MonoBehaviour
{
    public static AtomSpawnerCounter Manager { get; private set; }

    public BoxAtomContainer container;

    private float w;
    private float h;

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

        if (container != null)
        {
            container.onAntiIsolated.AddListener((a) => ReduceAnti());
        }
    }

    private void Start()
    {
        activeAtoms = new List<GameObject>(maxAtoms);
        activeAnti = new List<GameObject>(levelAnti);

        // Settear la camara aca por si acaso
        //Camera.main.GetComponent<CameraSizeSetter>().SetCameraSize(10, 10);

        h = Camera.main.orthographicSize * 2;
        w = ((float)Screen.width / (float)Screen.height) * h;
        InitialSpawn();
    }


    private void InitialSpawn()
    {
        for (int i = 0; i < initialAtoms; i++)
        {
            GameObject spawned = AtomPool.Pool.GetAtom(Atoms.Atom);
            activeAtoms.Add(spawned);
            spawned.transform.position = Vector2.right * Random.Range(-0.8f, 0.8f) * (w / 2) + Vector2.up * Random.Range(-0.8f, 0.8f) * (h / 2);
            Vector2 direction = Random.insideUnitCircle.normalized;
            spawned.GetComponent<AtomMovement>().InitializeAtom(direction, SpeedManager.Manager.GetSpeedRange());
        }

        for (int i = 0; i < levelAnti; i++)
        {
            GameObject spawned = AtomPool.Pool.GetAtom(Atoms.Anti);
            activeAnti.Add(spawned);
            spawned.transform.position = Vector2.right * Random.Range(-0.8f, 0.8f) * (w / 2) + Vector2.up * Random.Range(-0.8f, 0.8f) * (h / 2);
            Vector2 direction = Random.insideUnitCircle.normalized;
            spawned.GetComponent<AtomMovement>().InitializeAtom(direction, SpeedManager.Manager.GetSpeedRange());
        }
    }

    private void ReduceAnti()
    {
        levelAnti -= 1;
        Debug.Log(levelAnti);
        if (levelAnti == 0)
        {
            Debug.Log("No hay mas anti!");
            onNoMoreAnti.Invoke();  
        }
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, Vector3.up * h + Vector3.right * w + Vector3.forward);
    }
#endif
}
