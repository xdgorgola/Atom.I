using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private bool canShoot = true;
    public float scrollScale = 0.6f;

    private GameObject draggedAtom = null;
    public RayBox box;

    private void Start()
    {
        if (box.gameObject.TryGetComponent(out BoxAtomContainer container))
        {
            container.onFailedIsolation.AddListener(() => canShoot = true);
            container.onSucessfullIsolation.AddListener(() => canShoot = true);
        }
    }

    private void Update()
    {
        ClickInput();
        WheelInput();
    }

    private void ClickInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Solo la layer atomo
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 0.1f, 1 << 8);
            if (hit)
            {
                ProcessClickedObject(hit.collider.gameObject);
            }
        }
        else if (Input.GetMouseButtonUp(0) && draggedAtom != null)
        {
            draggedAtom.GetComponent<AtomMovement>().StopDragging();
            draggedAtom = null;
        }
        // Se puede repetir, bloquear
        else if (Input.GetMouseButtonDown(1) && canShoot)
        {
            Debug.Log("cajita");
            canShoot = false;
            box.ShootBox();
        }
    }

    private void WheelInput()
    {
        if (!canShoot) return;
        float delta = Input.mouseScrollDelta.y;
        if (delta != 0)
        {
            // Multiplicar por Time.deltaTime?
            box.ScaleBox(delta * scrollScale);
        }
    }

    private void ProcessClickedObject(GameObject clicked)
    {
        // Si es el hijo que no tiene nada, cambia clicked al padre
        if (clicked.transform.childCount == 0)
        {
            clicked = clicked.transform.parent.gameObject;
        }
        draggedAtom = clicked;
        AtomMovement aMov = clicked.GetComponent<AtomMovement>();
        aMov.StartDragging();
    }
}
