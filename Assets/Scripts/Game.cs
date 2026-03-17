using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    private const string MainMenuSceneName = "MainMenu";
    private const string TestWorldSceneName = "SampleScene";

    void Start()
    {
        Time.timeScale = 1f;

        if (SceneManager.GetActiveScene().name == MainMenuSceneName)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void LoadTestWorld()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(TestWorldSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Quit requested from game menu.");
        Application.Quit();
    }
}
