using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum GameState { Starting, Playing, Paused, GameOver }
public class GameManagerScript : MonoBehaviour
{
    public GameState state { get; private set; } = GameState.Starting;

    [HideInInspector]
    public int level { get; private set; } = 0;
    [SerializeField]
    [Range(1, 10)]
    private int maxLevel = 7;

    private float remainingTime = 0;
    public List<float> levelTime = new List<float>();

    private void Start()
    {
        level = 0;
        remainingTime = levelTime.Sum();
    }

    private IEnumerator LevelTime()
    {
        yield return new WaitForSeconds(0);
    }
}
