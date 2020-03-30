using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGOButtons : MonoBehaviour
{
    public GameObject pauseGOButtons = null;
    public GameObject nextButtons = null;

    private void Start()
    {
        if (pauseGOButtons == null || nextButtons == null)
        {
            Debug.LogWarning("Aparecedor de botones no seteado");
            return;
        }
        if (GameManagerScript.Manager != null)
        {
            GameManagerScript.Manager.onGameOver.AddListener(() => pauseGOButtons.SetActive(true));
            GameManagerScript.Manager.onPause.AddListener(() => pauseGOButtons.SetActive(true));
            GameManagerScript.Manager.onResume.AddListener(() => pauseGOButtons.SetActive(false));

            GameManagerScript.Manager.onFinishedGame.AddListener(() => nextButtons.SetActive(true));
        }

        pauseGOButtons.SetActive(false);
        nextButtons.SetActive(false);
    }
}
