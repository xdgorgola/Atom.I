using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

[RequireComponent(typeof(RayBox))]
public class BoxAtomContainer : MonoBehaviour
{
    public static BoxAtomContainer Container;

    // Componentes
    private RayBox box = null;

    private bool paused = false;

    private Vector2 cornerTL;
    private Vector2 cornerBR;

    /// <summary>
    /// Tiempo para isolar
    /// </summary>
   [SerializeField]
    private float isolationTime = 4f;
    /// <summary>
    /// Tiempo que te queda para terminar de isolar
    /// </summary>
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
    /// Anti Atomos dentro del cuadro de isolacion
    /// </summary>
    private List<GameObject> antiInside = new List<GameObject>();

    /// <summary>
    /// Cantidad de atomos al momento de capturar
    /// </summary>
    private int atomsCount = 0;
    /// <summary>
    /// Cantidad de anti atomos al momento de capturar
    /// </summary>
    private int antiCount = 0;

    /// <summary>
    /// Se llama cuando se empieza exitosamente el proceso de isolacion
    /// </summary>
    [HideInInspector]
    public UnityEvent onStartIsolation = new UnityEvent();

    /// <summary>
    /// Se llama cuando falla la isolacion
    /// </summary>
    [HideInInspector]
    public UnityEvent onFailedIsolation = new UnityEvent();
   
    /// <summary>
    /// Se llama cuando la isolacion es exitosa
    /// </summary>
    [HideInInspector]
    public UnityEvent onSucessfullIsolation = new UnityEvent();

    /// <summary>
    /// Se llama cuando un atomo es isolado (devuelve GameObject)
    /// </summary>
    public AtomEvent onAntiIsolated = new AtomEvent();
    /// <summary>
    /// Se llama cuando un atomo malo es sacado por equivocacion de la caja
    /// </summary>
    public AtomEvent onWrongIsolated = new AtomEvent();


    private void Awake()
    {
        if (Container != null && Container != this)
        {
            Debug.LogWarning("Ya existe el box atom container, borrando este...", gameObject);
            Destroy(gameObject);
        }
        Container = this;

        box = GetComponent<RayBox>();
        box.onBoxShoot.AddListener(CaptureAtomsInBox);
    }

    private void Start()
    {
        remainingTime = isolationTime;
        if (GameManagerScript.Manager != null)
        {
            GameManagerScript.Manager.onPause.AddListener(() => paused = true);
            GameManagerScript.Manager.onGameOver.AddListener(() => paused = true);
            GameManagerScript.Manager.onFinishedGame.AddListener(() => paused = true);

            GameManagerScript.Manager.onResume.AddListener(() => paused = false);
            GameManagerScript.Manager.onGameStarted.AddListener(() => paused = false);
        }
    }

    private void Update()
    {
        if (!isIsolating || paused) return;
        if (remainingTime > 0) remainingTime -= Time.deltaTime;
        else
        {
            FailIsolation();
            remainingTime = isolationTime;
        }
    }

    /// <summary>
    /// Encierra los atomos que se encuentren dentro de la caja
    /// </summary>
    public void CaptureAtomsInBox()
    {
        atomsCount = 0;
        antiCount = 0;

        // Tamano con colliders
        Vector2 expandedTL = (Vector2)transform.position + Vector2.up * (box.max.y + 0.351f) + Vector2.right * (box.min.x - 0.351f);
        Vector2 expandedBR = (Vector2)transform.position + Vector2.up * (box.min.y - 0.351f) + Vector2.right * (box.max.x + 0.351f);

        // Tamano vanilla caja
        cornerTL = (Vector2)transform.position + Vector2.up * box.max.y + Vector2.right * box.min.x;
        cornerBR = (Vector2)transform.position + Vector2.up * box.min.y + Vector2.right * box.max.x;

        // Centro caja
        Vector2 center = Vector2.Lerp(cornerTL, cornerBR, 0.5f);

        Debug.DrawLine(expandedTL, expandedBR, Color.black, 4);
        
        // Atomos capturados
        Collider2D[] atomsCaptured = Physics2D.OverlapAreaAll(expandedTL, expandedBR);
        foreach (GameObject atom in atomsCaptured.Select(a => a.gameObject).Where(b => b.transform.childCount != 0))
        {
            AtomLight light = atom.transform.GetChild(1).transform.GetChild(0).GetComponent<AtomLight>();

            if (CheckIfIsOut(atom)) atom.transform.position = center;

            atom.GetComponent<AtomMovement>().onAtomDragged.AddListener(ProcessDraggedAtom);

            if (light.atomKind == Atoms.Atom)
            {
                atomsInside.Add(atom);
                atomsCount += 1;
            }
            else
            {
                antiInside.Add(atom);
                antiCount += 1;
            }

            // Le indica al atomo que se encuentra dentro de la caja
            light.state = AtomState.InBox;
        }

        isIsolating = true;

        if (antiCount == 0) FailIsolation();
        else if (atomsCount == 0) IsolateAtoms(); // Isola todos, hacer metodo
        else onStartIsolation.Invoke();
    }

    
    /// <summary>
    /// Procesa un atomo draggeado de la caja
    /// </summary>
    /// <param name="atom">Atomo draggeado</param>
    public void ProcessDraggedAtom(GameObject atom)
    {
        if ((!atomsInside.Union(antiInside).Contains(atom)) || !CheckIfIsOut(atom) || !isIsolating) return;
        Debug.Log("Lo drageaste afuera!!");
        atom.GetComponent<AtomMovement>().onAtomDragged.RemoveListener(ProcessDraggedAtom);
        if (atom.GetComponentInChildren<AtomLight>().atomKind == Atoms.Anti)
        {
            antiCount -= 1;
            antiInside.Remove(atom);
        }
        else
        {
            onWrongIsolated.Invoke(atom);
            atomsCount -= 1;
            atomsInside.Remove(atom);
        }

        if (antiCount == 0)
        {
            FailIsolation();
        }
        else if (atomsCount == 0)
        {
            IsolateAtoms();
        }
    }


    /// <summary>
    /// Isola todos los atomos malos de la caja
    /// </summary>
    public void IsolateAtoms()
    {
        Debug.Log("Lograste isolarlos!!");
        // Chequeo por si acaso
        if (atomsCount > 0) Debug.LogWarning("Esto esta raro");

        foreach (GameObject anti in antiInside)
        {
            IsolateAtom(anti);
        }

        remainingTime = isolationTime;
        isIsolating = false;

        antiInside.Clear();
        onSucessfullIsolation.Invoke();
    }

    /// <summary>
    /// Isola un atomo
    /// </summary>
    public void IsolateAtom(GameObject anti)
    {
        anti.GetComponent<AtomMovement>().onAtomDragged.RemoveListener(ProcessDraggedAtom);
        // de mientras
        anti.SetActive(false);

        Debug.Log("Isolado el anti");
        onAntiIsolated.Invoke(anti);
    }

    /// <summary>
    /// Fallo de la isolacion de los atomos. Limpia las listas y resetea todo
    /// </summary>
    public void FailIsolation()
    {
        Debug.Log("pajuo fallaste");
        remainingTime = isolationTime;
        isIsolating = false;

        foreach (AtomMovement atom in (atomsInside.Union(antiInside)).Select(a=> a.GetComponent<AtomMovement>()))
        {
            atom.onAtomDragged.RemoveListener(ProcessDraggedAtom);
        }
        atomsInside.Clear();
        antiInside.Clear();

        atomsCount = 0;
        antiCount = 0;

        onFailedIsolation.Invoke();
    }


    /// <summary>
    /// Chequea si un atomo esta fuera de la caja
    /// </summary>
    /// <param name="atom">Atomo a chequear posicion</param>
    /// <returns>True si el atomo esta fuera/False en otro caso</returns>
    public bool CheckIfIsOut(GameObject atom)
    {
        Vector2 atomPos = atom.transform.position;
        return (atomPos.x < cornerTL.x || atomPos.x > cornerBR.x || atomPos.y < cornerBR.y || atomPos.y > cornerTL.y);
    }
}
