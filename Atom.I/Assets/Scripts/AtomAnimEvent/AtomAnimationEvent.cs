using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomAnimationEvent : MonoBehaviour
{
    public GameObject particles;
    private GameObject inst;

    public void ActivarMuerte()
    {
        GetComponent<Rigidbody2D>().simulated = false;
        GetComponent<Animator>().SetBool("Death", true);
    }

    public void ShootParticles()
    {
        inst = Instantiate(particles, transform);
        inst.transform.SetParent(null);
        Invoke("DestruirTodo", 3);
    }

    public void DestruirTodo()
    {
        Destroy(inst);
        Destroy(gameObject);
    }
}
