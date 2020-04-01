using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class IntEvent : UnityEvent<int> { }

public enum GameState { Starting, Playing, Paused, GameOver, FinishedLevel }
public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript Manager { get; private set; }

    public GameState state { get; private set; } = GameState.Starting;

    [SerializeField]
    private int remainingTime = 0;

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
    [HideInInspector]
    public IntEvent onCounterReduced = new IntEvent();

    private Coroutine counterRoutine = null;

    private void Awake()
    {
        if (Manager != null && Manager != this)
        {
            Debug.LogWarning("Ya existe el game state manager, borrando este...", gameObject);
            Destroy(gameObject);
            return;
        }
        Manager = this;
    }

    private void Start()
    {
        if (AtomSpawnerCounter.Manager != null)
        {
            AtomSpawnerCounter.Manager.onNoMoreAnti.AddListener(FinishedLevel);
        }
        if (BoxManager.Instance != null)
        {
            BoxManager.Life.onLifeDepleted.AddListener(GameOver);
        }
        // para testear
        Invoke("StartGame", 3);
    }

    private void Update()
    {
        if (state == GameState.Playing)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseGame();
                return;
            }
        }
        else if (state == GameState.Paused && Input.GetKeyDown(KeyCode.Escape))
        {
            ResumeGame();
        }
    }
    
    private void StartGame()
    {
        state = GameState.Playing;
        counterRoutine = StartCoroutine(GameCounter());
        onGameStarted.Invoke();
    }

    private void PauseGame()
    {
        state = GameState.Paused;
        Time.timeScale = 0;
        onPause.Invoke();
    }


    private void ResumeGame()
    {
        state = GameState.Playing;
        Time.timeScale = 1;
        onResume.Invoke();
    }


    private void FinishedLevel()
    {
        state = GameState.FinishedLevel;
        onFinishedGame.Invoke();
    }


    private void GameOver()
    {
        state = GameState.GameOver;
        onGameOver.Invoke();
    }

    private IEnumerator GameCounter()
    {
        onCounterReduced.Invoke(remainingTime);
        while (true)
        {
            yield return new WaitForSeconds(1);
            remainingTime -= 1;
            onCounterReduced.Invoke(remainingTime);
            if (remainingTime <= 0 && state != GameState.FinishedLevel)
            {
                GameOver();
                StopCoroutine(counterRoutine);
            }
        }
    }
}
