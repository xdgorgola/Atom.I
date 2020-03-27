using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public enum GameState { Starting, Playing, Paused, GameOver }
public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript Manager { get; private set; }

    public GameState state { get; private set; } = GameState.Starting;

    [HideInInspector]
    public int level { get; private set; } = 0;
    [SerializeField]
    [Range(1, 10)]
    private int maxLevel = 7;

    private float remainingTime = 0;
    public List<float> levelTime = new List<float>();

    [HideInInspector]
    public UnityEvent onGameStarted = new UnityEvent();
    [HideInInspector]
    public UnityEvent onPause = new UnityEvent();
    [HideInInspector]
    public UnityEvent onResume = new UnityEvent();
    [HideInInspector]
    public UnityEvent onFinishedGame = new UnityEvent();

    private void Awake()
    {
        if (Manager != null && Manager != this)
        {
            Debug.LogWarning("Ya existe el game state manager, borrando este...", gameObject);
            Destroy(gameObject);
        }
        Manager = this;
    }

    private void Start()
    {
        level = 0;
        remainingTime = levelTime.Sum();
    }

    private void Update()
    {
        if (remainingTime == 0)
        {
            GameOver();
        }
        if (state == GameState.Playing)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseGame();
                return;
            }
            remainingTime -= Time.deltaTime;
        }
        else if (state == GameState.Paused && Input.GetKeyDown(KeyCode.Escape))
        {
            ResumeGame();
        }
    }
    

    private void PauseGame()
    {
        state = GameState.Paused;
        onPause.Invoke();
    }


    private void ResumeGame()
    {
        state = GameState.Playing;
        onResume.Invoke();
    }

    private void GameOver()
    {
        state = GameState.GameOver;
        onFinishedGame.Invoke();
    }
}
