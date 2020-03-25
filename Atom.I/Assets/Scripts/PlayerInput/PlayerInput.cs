using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState { Box, NotDraggin, Drag }


public class PlayerInput : MonoBehaviour
{
    private PlayerState state = PlayerState.Box;

    private GameObject draggedAtom = null;


    private void Start()
    {
        // esto es pa probar
        state = PlayerState.Drag;
    }
    private void Update()
    {
        switch (state)
        {
            case PlayerState.Box:
                break;
            case PlayerState.Drag:
                ClickInput();
                break;
            default:
                break;
        }
    }

    private void BoxInput()
    {

    }

    private void ClickInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
