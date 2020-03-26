using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MouseFollower))]
public class RayBox : MonoBehaviour
{
    // Componentes
    private MouseFollower mf = null;

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
    private Vector2 min = Vector2.zero;
    private Vector2 max = Vector2.zero;

    /// <summary>
    /// Transformada de hijo que contendra los colliders externos
    /// </summary>
    [SerializeField]
    private Transform colliderContainer = null;
    /// <summary>
    /// Colliders de las paredes de la caja
    /// </summary>
    private List<Collider2D> walls = new List<Collider2D>();

    private void Awake()
    {
        mf = GetComponent<MouseFollower>();
        mf.SetDamp(false);
    }


    private void Start()
    {
        max = Vector2.one * halfSize + (Vector2)transform.position;
        min = -Vector2.one * halfSize + (Vector2)transform.position;

        colliderContainer.localScale = ((Vector3.up + Vector3.right) * halfSize + Vector3.forward) * 2;

        for (int i = 0; i < colliderContainer.childCount; i++)
        {
            Collider2D coll = colliderContainer.GetChild(i).GetComponent<Collider2D>();
            coll.isTrigger = false;
            walls.Add(coll);
        }
        SetWallsStatus(false);
    }


    /// <summary>
    /// Escala la caja una cantidad
    /// </summary>
    /// <param name="scaleAmount">Cantidad a escalar la caja</param>
    public void ScaleBox(float scaleAmount)
    {
        halfSize = Mathf.Clamp(halfSize + scaleAmount, minHalfSize, maxHalfSize);
        max = Vector2.one * halfSize + (Vector2)transform.position;
        min = -Vector2.one * halfSize + (Vector2)transform.position;

        colliderContainer.localScale = ((Vector3.up + Vector3.right) * halfSize + Vector3.forward) * 2;
    }


    public void ShootBox()
    {
        mf.SetMFStatus(false);
        SetWallsStatus(true);
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
        Gizmos.DrawLine((Vector2)transform.position + Vector2.right * min.x + Vector2.up * max.y, (Vector2)transform.position + Vector2.right * max.x + Vector2.up * max.y);
        Gizmos.DrawLine((Vector2)transform.position + Vector2.right * min.x + Vector2.up * max.y, (Vector2)transform.position + Vector2.right * min.x + Vector2.up * min.y);
        Gizmos.DrawLine((Vector2)transform.position + Vector2.right * min.x + Vector2.up * min.y, (Vector2)transform.position + Vector2.right * max.x + Vector2.up * min.y);
        Gizmos.DrawLine((Vector2)transform.position + Vector2.right * max.x + Vector2.up * min.y, (Vector2)transform.position + Vector2.right * max.x + Vector2.up * max.y);
    }
#endif
}