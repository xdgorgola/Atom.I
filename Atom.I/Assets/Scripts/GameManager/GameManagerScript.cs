using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public enum GameState { Starting, Playing, Paused, GameOver, FinishedLevel }
public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript Manager { get; private set; }

    public GameState state { get; private set; } = GameState.Starting;

    [SerializeField]
    private float remainingTime = 0;

    [HideInInspector]
    public UnityEvent onGameStarted = new UnityEvent();
    [HideInInspector]
    public UnityEvent onPause = new UnityEvent();
    [HideInInspector]
    public UnityEvent onResume = new UnityEvent();
    [HideInInspector]
    public UnityEvent onGameOver = new UnityEvent();
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
        if (AtomSpawnerCounter.Manager != null)
        {
            AtomSpawnerCounter.Manager.onNoMoreAnti.AddListener(FinishedLevel);
        }
        // para testear
        state = GameState.Playing;
    }

    private void Update()
    {
        if (remainingTime <= 0)
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


    private void FinishedLevel()
    {
        Debug.Log("Juego terminado");
        state = GameState.FinishedLevel;
        onFinishedGame.Invoke();
    }


    private void GameOver()
    {
        state = GameState.GameOver;
        onGameOver.Invoke();
    }
}
