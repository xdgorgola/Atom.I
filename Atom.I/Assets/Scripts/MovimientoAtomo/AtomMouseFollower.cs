using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomMouseFollower : MonoBehaviour
{
    [SerializeField]
    private bool isActive = false;
    private Vector2 offset = Vector2.zero;

    private Vector2 dampVel = Vector2.zero;

    private void Update()
    {
        if (!isActive) return;
        Vector2 target = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
        transform.position = Vector2.SmoothDamp(transform.position, target, ref dampVel, 0.08f);
    }

    public void SetMFStatus(bool toSet)
    {
        isActive = toSet;
    }

    public Vector2 GetDampVel()
    {
        return dampVel;
    }
}
