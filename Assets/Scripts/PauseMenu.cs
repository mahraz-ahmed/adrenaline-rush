using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    // Define initial variables
    public GameObject PauseUI;
    private bool isPaused = false;

    void Start()
    {
        PauseUI.SetActive(false);  // Ensure pause menu is hidden when scene starts
        Time.timeScale = 1f; // Ensure game is resumed
        isPaused = false;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape)) // Check for the pause key (ESC)
        {
            if (isPaused == true)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        PauseUI.SetActive(false); // Hide the pause menu
        Time.timeScale = 1f;          // Resume the game
        isPaused = false;
    }

    public void Pause()
    {
        PauseUI.SetActive(true);  // Show the pause menu
        Time.timeScale = 0f;          // Pause the game
        isPaused = true;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // Resume time before changing scenes
        SceneManager.LoadScene("Main Menu"); 
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
