using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextLevelButtons : MonoBehaviour
{
    public Button nextButton = null;
    public Button mainMenuButton = null;

    public int nextLevel;

    private void Start()
    {
        if (nextButton == null || mainMenuButton == null)
        {
            Debug.LogWarning("Aparecedor de botones no seteado");
            return;
        }
        if (GameManagerScript.Manager != null)
        {
            GameManagerScript.Manager.onFinishedGame.AddListener(() => nextButton.gameObject.SetActive(true));
            GameManagerScript.Manager.onFinishedGame.AddListener(() => mainMenuButton.gameObject.SetActive(true));
        }

        nextButton.gameObject.SetActive(false);
        mainMenuButton.gameObject.SetActive(false);

        nextButton.onClick.AddListener(LevelSceneManager.Manager.NextLevel);
        mainMenuButton.onClick.AddListener(LevelSceneManager.Manager.LoadMainMenu);
    }
}
