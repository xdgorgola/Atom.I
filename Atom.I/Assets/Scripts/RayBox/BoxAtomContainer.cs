using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(RayBox))]
public class BoxAtomContainer : MonoBehaviour
{
    private RayBox box = null;

    public Vector2 min { get; private set; } = Vector2.zero;
    public Vector2 max { get; private set; } = Vector2.zero;

    private List<GameObject> atomsInside = new List<GameObject>();

    private void Awake()
    {
        box = GetComponent<RayBox>();
        box.onBoxShoot.AddListener(CaptureAtoms);
    }

    public void CaptureAtoms()
    {
        Debug.Log("!!!!!!!!!!!!!!1");
        min = box.min;
        max = box.max;

        Vector2 cornerTL = (Vector2)transform.position + Vector2.up * box.max.y + Vector2.right * box.min.x;
        Vector2 cornerBR = (Vector2)transform.position + Vector2.up * box.min.y + Vector2.right * box.max.x;

        Debug.DrawLine(cornerTL, cornerBR, Color.black, 4);
        Debug.Log(cornerTL);
        Debug.Log(cornerBR);
        Collider2D[] atomsCaptured = Physics2D.OverlapAreaAll(cornerTL, cornerBR);
        foreach (Transform atoms in atomsCaptured.Select(a => a.transform).Where(b => b.childCount != 0))
        {
            Debug.Log(atoms.name);
            atoms.gameObject.SetActive(false);
        }
    }

}
