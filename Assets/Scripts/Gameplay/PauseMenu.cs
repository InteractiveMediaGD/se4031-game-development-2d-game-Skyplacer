using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [Header("UI Reference")]
    public GameObject pausePanel; // Drag your Pause Panel here
    private bool isPaused = false;

    void Update()
    {
        // Check if Escape is pressed AND the game isn't already over
        if (Input.GetKeyDown(KeyCode.Escape) && !GameManager.Instance.isGameOver)
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f; // Freeze the world
        isPaused = true;
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f; // Unfreeze the world
        isPaused = false;
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1f; // Always reset time before changing scenes
        GameManager.Instance.GoToMenu();
    }
}