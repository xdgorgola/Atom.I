using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private bool paused = true;
    private bool canShoot = true;
    public float scrollScale = 0.6f;

    private GameObject draggedAtom = null;

    private void Start()
    {
        if (GameManagerScript.Manager != null)
        {
            GameManagerScript.Manager.onPause.AddListener(() => paused = true);
            GameManagerScript.Manager.onGameOver.AddListener(() => paused = true);
            GameManagerScript.Manager.onFinishedGame.AddListener(() => paused = true);

            GameManagerScript.Manager.onResume.AddListener(() => paused = false);
            GameManagerScript.Manager.onGameStarted.AddListener(() => paused = false);
        }
        if (BoxManager.Container != null)
        {
            BoxManager.Container.onFailedIsolation.AddListener(() => canShoot = true);
            BoxManager.Container.onFailedIsolation.AddListener(StopDrag);
            BoxManager.Container.onSucessfullIsolation.AddListener(() => canShoot = true);
        }

        paused = true;
    }

    private void Update()
    {
        if (paused) return;
        ClickInput();
        WheelInput();
    }

    private void ClickInput()
    {
        if (Input.GetMouseButtonDown(0) && !canShoot)
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
            StopDrag();
        }
        // Se puede repetir, bloquear
        else if (Input.GetMouseButtonDown(1) && canShoot)
        {
            canShoot = false;
            BoxManager.Raybox.ShootBox();
        }
    }

    private void WheelInput()
    {
        if (!canShoot) return;
        float delta = Input.mouseScrollDelta.y;
        if (delta != 0)
        {
            // Multiplicar por Time.deltaTime?
            BoxManager.Raybox.ScaleBox(delta * scrollScale);
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

    private void StopDrag()
    {
        if (draggedAtom == null) return;
        draggedAtom.GetComponent<AtomMovement>().StopDragging();
        draggedAtom = null;
    }
}
