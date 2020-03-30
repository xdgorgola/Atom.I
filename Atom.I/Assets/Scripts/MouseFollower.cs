using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    [SerializeField]
    private bool isActive = false;
    [SerializeField]
    private bool isDamped = true;
    private Vector2 offset = Vector2.zero;
    private Vector2 dampVel = Vector2.zero;


    private void Update()
    {
        if (!isActive) return;
        Vector2 target = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
        if (isDamped) transform.position = Vector2.SmoothDamp(transform.position, target, ref dampVel, 0.08f);
        else transform.position = target;
    }


    public void SetMFStatus(bool toSet)
    {
        isActive = toSet;
    }

    public void SetDamp(bool toSet)
    {
        isDamped = toSet;
    }

    public Vector2 GetDampVel()
    {
        return dampVel;
    }
}
