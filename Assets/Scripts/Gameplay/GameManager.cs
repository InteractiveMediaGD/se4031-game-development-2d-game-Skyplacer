using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static float GlobalSpeed = 5f;
    public float initialSpeed = 5f;
    public float acceleration = 0.1f;

    void Start()
    {
        Time.timeScale = 1;
        GlobalSpeed = initialSpeed;
    }

    void Update()
    {
        // Gradually increase speed (Requirement 6)
        GlobalSpeed += acceleration * Time.deltaTime;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
