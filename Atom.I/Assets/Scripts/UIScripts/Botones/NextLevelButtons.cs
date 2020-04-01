using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextLevelButtons : MonoBehaviour
{
    public Button nextButton = null;
    public Button mainMenuButton = null;
    public GameObject pan = null;

    public int nextLevel;

    private void Start()
    {
        if (nextButton == null || mainMenuButton == null || pan == null)
        {
            Debug.LogWarning("Aparecedor de botones no seteado");
            return;
        }
        if (GameManagerScript.Manager != null)
        {
            GameManagerScript.Manager.onFinishedGame.AddListener(() => nextButton.gameObject.SetActive(true));
            GameManagerScript.Manager.onFinishedGame.AddListener(() => mainMenuButton.gameObject.SetActive(true));
            GameManagerScript.Manager.onFinishedGame.AddListener(() => pan.SetActive(true));
        }

        nextButton.gameObject.SetActive(false);
        mainMenuButton.gameObject.SetActive(false);
        pan.SetActive(false);

        nextButton.onClick.AddListener(LevelSceneManager.Manager.NextLevel);
        mainMenuButton.onClick.AddListener(LevelSceneManager.Manager.LoadMainMenu);
    }

}
