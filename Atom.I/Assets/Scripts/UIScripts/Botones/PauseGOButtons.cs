using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseGOButtons : MonoBehaviour
{
    public Button restartButton = null;
    public Button mainMenuButton = null;
    public GameObject pan = null;

    public int nextLevel;

    private void Start()
    {
        if (restartButton == null || mainMenuButton == null)
        {
            Debug.LogWarning("Aparecedor de botones no seteado");
            return;
        }
        if (GameManagerScript.Manager != null)
        {
            // Panel
            GameManagerScript.Manager.onGameOver.AddListener(() => pan.SetActive(true));

            // Boton restart
            GameManagerScript.Manager.onGameOver.AddListener(() => restartButton.gameObject.SetActive(true));
            GameManagerScript.Manager.onPause.AddListener(() => restartButton.gameObject.SetActive(true));
            GameManagerScript.Manager.onResume.AddListener(() => restartButton.gameObject.SetActive(false));

            // Boton menu principal
            GameManagerScript.Manager.onGameOver.AddListener(() => mainMenuButton.gameObject.SetActive(true));
            GameManagerScript.Manager.onPause.AddListener(() => mainMenuButton.gameObject.SetActive(true));
            GameManagerScript.Manager.onResume.AddListener(() => mainMenuButton.gameObject.SetActive(false));
        }

        restartButton.gameObject.SetActive(false);
        mainMenuButton.gameObject.SetActive(false);
        pan.SetActive(false);

        restartButton.onClick.AddListener(LevelSceneManager.Manager.RestartLevel);
        mainMenuButton.onClick.AddListener(LevelSceneManager.Manager.LoadMainMenu);
    }
}
