using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

[RequireComponent(typeof(MouseFollower), typeof(BoxAtomContainer))]
public class RayBox : MonoBehaviour
{
    // Componentes
    private MouseFollower mf = null;
    private BoxAtomContainer ct = null;


    /// <summary>
    /// Half Size maximo de la caja
    /// </summary>
    [SerializeField]
    private float maxHalfSize = 16f;
    /// <summary>
    /// Half Size minimo de la caja
    /// </summary>
    [SerializeField]
    private float minHalfSize = 2f;
    /// <summary>
    /// Half Size actual de la caja
    /// </summary>
    [SerializeField]
    private float halfSize = 4f;

    // NOTA: PODRIAMOS USAR UN BOUNDING BOX DE UNITY QUE YA VIENE CON
    // POINT IS INSIDE.
    /// <summary>
    /// Bounding Box de la caja minimos y maximos. 
    /// </summary>
    public Vector2 min { get; private set; } = Vector2.zero;
    public Vector2 max { get; private set; } = Vector2.zero;

    /// <summary>
    /// Transformada de hijo que contendra los colliders externos
    /// </summary>
    [SerializeField]
    private Transform colliderContainer = null;
    /// <summary>
    /// Colliders de las paredes de la caja
    /// </summary>
    private List<Collider2D> walls = new List<Collider2D>();

    public UnityEvent onBoxShoot = new UnityEvent();
    public UnityEvent onBoxReActivate = new UnityEvent();

    private void Awake()
    {
        mf = GetComponent<MouseFollower>();
        mf.SetDamp(false);

        ct = GetComponent<BoxAtomContainer>();
    }


    private void Start()
    {
        max = Vector2.one * halfSize;
        min = -Vector2.one * halfSize;

        colliderContainer.localScale = ((Vector3.up + Vector3.right) * halfSize + Vector3.forward) * 2;

        for (int i = 0; i < colliderContainer.childCount; i++)
        {
            Collider2D coll = colliderContainer.GetChild(i).GetComponent<Collider2D>();
            coll.isTrigger = false;
            walls.Add(coll);
        }
        SetWallsStatus(false);

        // Eventos para activar movimiento
        ct.onSucessfullIsolation.AddListener(ActivateMovementAgain);
        ct.onFailedIsolation.AddListener(ActivateMovementAgain);
    }


    /// <summary>
    /// Escala la caja una cantidad
    /// </summary>
    /// <param name="scaleAmount">Cantidad a escalar la caja</param>
    public void ScaleBox(float scaleAmount)
    {
        halfSize = Mathf.Clamp(halfSize + scaleAmount, minHalfSize, maxHalfSize);
        max = Vector2.one * halfSize;
        min = -Vector2.one * halfSize;

        colliderContainer.localScale = ((Vector3.up + Vector3.right) * halfSize + Vector3.forward) * 2;
        //foreach (Transform coll in walls.Select(wallColl => wallColl.transform))
        //{
        //    coll.localScale = Vector3.right * 0.52f + Vector3.up * (0.77f / colliderContainer.localScale.y) + Vector3.forward;
        //}
        // El scale de los hijos debe de ser compensado en Y para que no aumenten su grosor.
        // dividirlo entre la escala del padre creo que seria suficiente.
        // Colliders a 0.75 no estarian mal
    }


    public void ShootBox()
    {
        ScaleBox(0);
        mf.SetMFStatus(false);
        SetWallsStatus(true);
        onBoxShoot.Invoke();
    }

    public void ActivateMovementAgain()
    {
        mf.SetMFStatus(true);
        SetWallsStatus(false);
    }

    public void SetWallsStatus(bool status)
    {
        foreach (Collider2D coll in walls)
        {
            coll.enabled = status;
            //coll.isTrigger = !status;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Vector2 origin = (Vector2)transform.position;
        Gizmos.DrawLine(origin + Vector2.right * min.x + Vector2.up * max.y, origin + Vector2.right * max.x + Vector2.up * max.y);
        Gizmos.DrawLine(origin + Vector2.right * min.x + Vector2.up * max.y, origin + Vector2.right * min.x + Vector2.up * min.y);
        Gizmos.DrawLine(origin + Vector2.right * min.x + Vector2.up * min.y, origin + Vector2.right * max.x + Vector2.up * min.y);
        Gizmos.DrawLine(origin + Vector2.right * max.x + Vector2.up * min.y, origin + Vector2.right * max.x + Vector2.up * max.y);
    }
#endif
}