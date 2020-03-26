using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

[RequireComponent(typeof(RayBox))]
public class BoxAtomContainer : MonoBehaviour
{
    // Componentes
    private RayBox box = null;

    private Vector2 cornerTL;
    private Vector2 cornerBR;

   [SerializeField]
    private float isolationTime = 4f;
    private float remainingTime = 4f;

    /// <summary>
    /// Indica si se esta en proceso de isolacion!
    /// </summary>
    private bool isIsolating = false;
    /// <summary>
    /// Atomos dentro del cuadro de isolacion
    /// </summary>
    private List<GameObject> atomsInside = new List<GameObject>();
    /// <summary>
    /// Cantidad de atomos al momento de capturar.
    /// </summary>
    private int atomsCount = 0;

    [HideInInspector]
    public UnityEvent onFailedIsolation = new UnityEvent();

    [HideInInspector]
    public UnityEvent onSucessfullIsolation = new UnityEvent();

    public AtomEvent onAtomIsolated = new AtomEvent();

    private void Awake()
    {
        box = GetComponent<RayBox>();
        box.onBoxShoot.AddListener(CaptureAtomsInBox);
    }

    private void Start()
    {
        remainingTime = isolationTime;    
    }

    private void Update()
    {
        if (!isIsolating) return;
        if (remainingTime > 0) remainingTime -= Time.deltaTime;
        else
        {
            FailIsolation();
            remainingTime = isolationTime;
        }
    }

    public void CaptureAtomsInBox()
    {
        cornerTL = (Vector2)transform.position + Vector2.up * box.max.y + Vector2.right * box.min.x;
        cornerBR = (Vector2)transform.position + Vector2.up * box.min.y + Vector2.right * box.max.x;

        Debug.DrawLine(cornerTL, cornerBR, Color.black, 4);
        Debug.Log(cornerTL);
        Debug.Log(cornerBR);
        Collider2D[] atomsCaptured = Physics2D.OverlapAreaAll(cornerTL, cornerBR);
        foreach (GameObject atom in atomsCaptured.Select(a => a.gameObject).Where(b => b.transform.childCount != 0))
        {
            atomsInside.Add(atom);
            atom.GetComponent<AtomMovement>().onAtomDragged.AddListener(ProcessDraggedAtom);
        }
        atomsCount = atomsInside.Count;
        isIsolating = true;

        if (atomsCount == 1) IsolateAtom();
        else if (atomsCount == 0) FailIsolation();
    }

    public void ProcessDraggedAtom(GameObject atom)
    {
        if (!atomsInside.Contains(atom) || !CheckIfIsOut(atom) || !isIsolating) return;
        Debug.Log("Lo drageaste afuera!!");
        atom.GetComponent<AtomMovement>().onAtomDragged.RemoveListener(ProcessDraggedAtom);
        atomsInside.Remove(atom);
        if (atomsInside.Count == 1)
        {
            Debug.Log("Lograste isolarlo!!");
            IsolateAtom();
        }
    }


    public void IsolateAtom()
    {
        // acitvar una animacion y desactivarlo luego a la pool!
        // Mandar como score atomsCount y remainingTime (mientras mayor sea remainingTime, mejor!)
        // pendiente que si es 1 el atomsCount, no deberia tener tanto score
        remainingTime = isolationTime;
        isIsolating = false;

        GameObject isolated = atomsInside[0];
        isolated.GetComponent<AtomMovement>().onAtomDragged.RemoveListener(ProcessDraggedAtom);
        // de mientras
        isolated.SetActive(false);
        atomsInside.Clear();

        onAtomIsolated.Invoke(isolated);
        onSucessfullIsolation.Invoke();
    }


    public void FailIsolation()
    {
        Debug.Log("pajuo fallaste");
        remainingTime = isolationTime;
        isIsolating = false;

        foreach (AtomMovement atom in atomsInside.Select(a=> a.GetComponent<AtomMovement>()))
        {
            atom.onAtomDragged.RemoveListener(ProcessDraggedAtom);
        }
        atomsInside.Clear();
        atomsCount = 0;

        onFailedIsolation.Invoke();
    }

    public bool CheckIfIsOut(GameObject atom)
    {
        Vector2 atomPos = atom.transform.position;
        return (atomPos.x < cornerTL.x || atomPos.x > cornerBR.x || atomPos.y < cornerBR.y || atomPos.y > cornerTL.y);
    }
}
