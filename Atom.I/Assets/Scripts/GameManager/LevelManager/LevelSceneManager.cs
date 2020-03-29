using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSceneManager : MonoBehaviour
{
    public static LevelSceneManager Manager { get; private set; }

    private void Awake()
    {
        if (Manager != null && Manager != this)
        {
            Debug.LogWarning("Ya hay un LevenSceneManager, borrando este...", gameObject);
            Destroy(gameObject);
        }
        Manager = this;
    }


    /// <summary>
    /// Carga el menu principal
    /// </summary>
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Carga los creditos
    /// </summary>
    public void LoadCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    /// <summary>
    /// Reinicia un nivel
    /// </summary>
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Carga el siguiente nivel
    /// </summary>
    /// <param name="nextScene"></param>
    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    /// <summary>
    /// Sale del juego
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }
}
