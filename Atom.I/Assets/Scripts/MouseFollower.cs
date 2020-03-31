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

    private float x = 0;
    private float y = 0;
    private float yq = 0;

    private void Start()
    {
        if (Camera.main.gameObject.TryGetComponent(out CameraSizeSetter css))
        {
            x = css.x;
            y = css.y;
            float quarter = Screen.height * 0.13f;
            Debug.Log(quarter);
            yq = Camera.main.ScreenToWorldPoint(Vector3.up * quarter + Vector3.right * (Screen.width / 2)).y;
            Debug.Log(yq);
        }
    }

    private void Update()
    {
        if (!isActive) return;
        Vector3 final;
        Vector2 target = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
        if (isDamped) final = Vector2.SmoothDamp(transform.position, target, ref dampVel, 0.08f);
        else final = target;

        final.x = Mathf.Clamp(final.x, -x / 2 + 0.5f, x / 2 - 0.5f);
        final.y = Mathf.Clamp(final.y, yq + 0.5f, y / 2 - 0.5f);
        transform.position = final;
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
