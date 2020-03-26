using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RayDeformer : MonoBehaviour
{
    public LineRenderer lr { get; private set; } = null;

    [SerializeField]
    [Range(2, 30)]
    private int pointsInLine = 10;
    [SerializeField]
    private float defRate = 0.2f;

    private Vector2[] straightPoints = null;

    [SerializeField]
    private Vector2 from = Vector2.zero;
    [SerializeField]
    private Vector2 to = Vector2.right;
    private Vector2 perp = Vector2.up;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = pointsInLine;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            StopAllCoroutines();
            StartDeformation(from, to, pointsInLine);
        }
    }

    public void SetUpLine(Vector2 from, Vector2 to, int pointsInLine = 10)
    {
        this.from = from;
        this.to = to;
        this.pointsInLine = pointsInLine;

        float mag = (from - to).magnitude;
        float sep = mag / (pointsInLine - 1);
        Vector2 dir = (to - from).normalized;

        straightPoints = new Vector2[pointsInLine];

        lr.SetPosition(0, from);
        straightPoints[0] = from;
        for (int i = 1; i <= pointsInLine - 2; i++)
        {
            lr.SetPosition(i, dir * sep * i);
            straightPoints[i] = dir * sep * i;
        }
        lr.SetPosition(pointsInLine - 1, to);
        straightPoints[pointsInLine - 1] = to;

        perp = Vector2.Perpendicular(dir);
    }

    public void StartDeformation(Vector2 from, Vector2 to, int pointsInLine = 10)
    {
        SetUpLine(from, to, pointsInLine);
        StartCoroutine(DeformationRoutine());
    }

    public void StopDeformation()
    {
        StopCoroutine(DeformationRoutine());
    }

    public IEnumerator DeformationRoutine()
    {
        for (int i = 0; true; i = i % 2)
        {
            for (int j = 0; j <= pointsInLine - 2; j++)
            {
                Vector2 point = straightPoints[j];
                // Si es it par
                // Estos random estan dando igual al duplicarlos :(
                if (i == 0)
                {
                    if (j % 2 == 0)
                    {
                        lr.SetPosition(j, point + perp * Random.Range(0.2f, 0.35f));
                    }
                    else
                    {
                        lr.SetPosition(j, point - perp * Random.Range(0.2f, 0.35f));
                    }
                }
                else
                {
                    if (j % 2 == 0)
                    {
                        lr.SetPosition(j, point - perp * Random.Range(0.2f, 0.35f));
                    }
                    else
                    {
                        lr.SetPosition(j, point + perp * Random.Range(0.2f, 0.35f));
                    }
                }
            }
            yield return new WaitForSeconds(defRate);
            i++;
        }
    }
}
