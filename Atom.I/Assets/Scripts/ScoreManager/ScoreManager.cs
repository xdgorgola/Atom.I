using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScoreEvents : UnityEvent<float> { }
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Manager { get; private set; }

    public float score { get; private set; } = 0f;

    public ScoreEvents onScoreChange = new ScoreEvents();

    private void Awake()
    {
        if (Manager != null && Manager != this)
        {
            Debug.LogWarning("Ya hay un score manager, borrando este...", gameObject);
            Destroy(gameObject);
        }
        Manager = this;
    }

    public void ProcessAtomIsolated(int captured, float remainingTime)
    {
        // Hacer alguna funcionsita linda
        onScoreChange.Invoke(score);
    }
    

}
