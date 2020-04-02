using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AtomEvent : UnityEvent<GameObject> { }

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(MouseFollower))]
public class AtomMovement : MonoBehaviour
{
    // Componentes
    private Rigidbody2D rb2d = null;
    private Collider2D coll2d = null;
    private MouseFollower amf = null;


    /// <summary>
    /// Direccion inicial del atomo
    /// </summary>
    public Vector2 initialDirection = Vector2.one;
    /// <summary>
    /// Rapidez del atomo (tambien es inicial)
    /// </summary>
    [SerializeField]
    private float speed = 5f;

    // No se si usarre estas dos cosas
    private Vector2 velocityPreDrag;
    private float speedPreDrag = 5f;

    [SerializeField]
    private float randomTime = 4;

    public AtomEvent onAtomDragged = new AtomEvent();

    //19-21 de speed ya es fastidioso :(


    private void Awake()
    {
        // Inicializando rigid body 2d
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.gravityScale = 0;
        rb2d.freezeRotation = true;
        rb2d.angularDrag = 0;
        rb2d.drag = 0;
        rb2d.isKinematic = false;

        coll2d = GetComponent<Collider2D>();
        coll2d.isTrigger = false;

        amf = GetComponent<MouseFollower>();

        rb2d.velocity = initialDirection.normalized * speed;
    }


    // Start is called before the first frame update
    void Start()
    {
        // Eventos de velocidad
        if (SpeedManager.Manager != null)
        {
            SpeedManager.Manager.onStrictSpeedChange.AddListener(ChangeSpeed);
            SpeedManager.Manager.onAddSpeed.AddListener(AddSpeed);
        }
        if (GameManagerScript.Manager != null)
        {
            GameManagerScript.Manager.onGameStarted.AddListener(ResumeMovement);
            Invoke("StartRandom", GameManagerScript.Manager.startTime);
        }
    }


    public void InitializeAtom(Vector2 direction, float newSpeed)
    {
        gameObject.SetActive(true);
        initialDirection = direction.normalized;
        speed = newSpeed;
        rb2d.velocity = direction.normalized * newSpeed;
    }


    public void StopMovement()
    {
        velocityPreDrag = rb2d.velocity;
        speedPreDrag = speed;
        rb2d.velocity = Vector2.zero;
    }


    public void ResumeMovement()
    {
        speed = speedPreDrag;
        rb2d.velocity = velocityPreDrag;
    }


    /// <summary>
    /// Cambia la rapidez totalmente del atomo
    /// </summary>
    /// <param name="newSpeed">Nueva rapidez del atomo</param>
    private void ChangeSpeed(float newSpeed)
    {
        rb2d.velocity = rb2d.velocity.normalized * newSpeed;
        speed = newSpeed;
    }


    /// <summary>
    /// Aumenta la rapidez del atomo una cantidad fija
    /// </summary>
    /// <param name="toAdd">Cantidad a aumentar</param>
    private void AddSpeed(float toAdd)
    {
        rb2d.velocity = rb2d.velocity.normalized * (speed + toAdd);
        speed = speed + toAdd;
    }


    /// <summary>
    /// Activa el dragging del atomo, desactivando su simulacion de rb2d,
    /// movimiento y activa el seguimiento del mouse.
    /// </summary>
    public void StartDragging()
    {
        speedPreDrag = speed;
        velocityPreDrag = rb2d.velocity;
        speed = 0;
        rb2d.velocity = Vector2.zero;
        
        // Desctivamos la simulacion fisica del atomo
        coll2d.enabled = false;
        rb2d.simulated = false;
        amf.SetMFStatus(true);
    }


    /// <summary>
    /// Desactiva el dragging del atomo, activa su simulacion de rb2d,
    /// movimiento y desactiva el seguimiento del mouse.
    /// </summary>
    public void StopDragging()
    {
        speed = speedPreDrag;

        // Conserva la direccion de drag del mouse
        rb2d.velocity = amf.GetDampVel().normalized * (speed);

        // Desactivamos el arraste
        coll2d.enabled = true;
        rb2d.simulated = true;
        amf.SetMFStatus(false);

        onAtomDragged.Invoke(gameObject);
    }

    public void StartRandom()
    {
        StartCoroutine(RandomDirectionRoutine());
    }
    public IEnumerator RandomDirectionRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(randomTime);
            rb2d.velocity = Random.insideUnitCircle.normalized * speed;
            //if (0.25f >= Random.Range(0f, 1f))
            //{
            //    rb2d.AddForce((rb2d))
            //}
        }
    }
}
