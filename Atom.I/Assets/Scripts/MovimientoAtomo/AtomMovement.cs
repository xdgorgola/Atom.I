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

    public AtomEvent onAtomDragged = new AtomEvent();

    //19-21 de speed ya es fastidioso :(


    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.gravityScale = 0;
        rb2d.freezeRotation = true;
        rb2d.angularDrag = 0;
        rb2d.drag = 0;
        rb2d.isKinematic = false;

        coll2d = GetComponent<Collider2D>();
        coll2d.isTrigger = false;

        amf = GetComponent<MouseFollower>();
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
        rb2d.velocity = initialDirection.normalized * speed;
    }


    /// <summary>
    /// Cambia la rapidez totalmente del atomo
    /// </summary>
    /// <param name="newSpeed">Nueva rapidez del atomo</param>
    private void ChangeSpeed(float newSpeed)
    {
        rb2d.velocity = rb2d.velocity.normalized * newSpeed;
        speed = newSpeed;
        Debug.Log("New Speed: " + speed);
    }


    /// <summary>
    /// Aumenta la rapidez del atomo una cantidad fija
    /// </summary>
    /// <param name="toAdd">Cantidad a aumentar</param>
    private void AddSpeed(float toAdd)
    {
        rb2d.velocity = rb2d.velocity.normalized * (speed + toAdd);
        speed = speed + toAdd;
        Debug.Log("New Speed: " + speed);
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
        // quizas aca chequear donde lo deje!
        speed = speedPreDrag;
        //rb2d.velocity = velocityPreDrag;
        // Conserva la direccion de drag del mouse
        rb2d.velocity = amf.GetDampVel().normalized * (speed);
        coll2d.enabled = true;
        rb2d.simulated = true;

        amf.SetMFStatus(false);

        onAtomDragged.Invoke(gameObject);
    }
}
