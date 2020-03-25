using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(AtomMouseFollower))]
public class AtomMovement : MonoBehaviour
{
    private Rigidbody2D rb2d = null;
    private Collider2D coll2d = null;
    private AtomMouseFollower amf = null;

    [SerializeField]
    private Vector2 initialDirection = Vector2.one;
    [SerializeField]
    private float speed = 5f;

    private Vector2 velocityPreDrag;
    private float speedPreDrag = 5f;

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

        amf = GetComponent<AtomMouseFollower>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (SpeedManager.Manager != null)
        {
            SpeedManager.Manager.onStrictSpeedChange.AddListener(ChangeSpeed);
            SpeedManager.Manager.onAddSpeed.AddListener(AddSpeed);
        }
        rb2d.velocity = initialDirection.normalized * speed;
        // Igualmente deberiamos rotar los centros y eso
    }

    private void ChangeSpeed(float newSpeed)
    {
        rb2d.velocity = rb2d.velocity.normalized * newSpeed;
        speed = newSpeed;
        Debug.Log("New Speed: " + speed);
    }

    private void AddSpeed(float toAdd)
    {
        rb2d.velocity = rb2d.velocity.normalized * (speed + toAdd);
        speed = speed + toAdd;
        Debug.Log("New Speed: " + speed);
    }

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
    }
}
