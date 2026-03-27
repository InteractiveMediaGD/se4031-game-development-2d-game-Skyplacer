using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Speed")]
    public static float GlobalSpeed = 5f;
    public float initialSpeed = 5f;
    public float acceleration = 0.1f;

    [Header("Game Over Settings")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;
    public GameObject playerExplosionPrefab;
    public Transform playerTransform;
    public AudioClip explosionSFX;
    public float delayInRealSeconds = 1.5f; 
    public GameObject healthbar;
    public GameObject scoreboard;
    public bool isGameOver = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        Time.timeScale = 1;
        GlobalSpeed = initialSpeed;
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    void Update()
    {
        GlobalSpeed += acceleration * Time.deltaTime;
    }

    public void TriggerGameOver(GameObject player)
    {
        // 1. Immediately freeze all movement and physics
        isGameOver = true;
        Time.timeScale = 0;

        // 2. Stop all player sounds (Footsteps, Jetpack)
        AudioSource[] allSources = player.GetComponents<AudioSource>();
        foreach (AudioSource source in allSources)
        {
            source.Stop();
        }

        // 3. Disable player functionality
        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponent<SpriteRenderer>().enabled = false;

        // 4. Start the final sequence
        StartCoroutine(GameOverSequence(player.transform.position));
    }

    IEnumerator GameOverSequence(Vector3 deathPosition)
    {
        // Spawn explosion (Make sure Animator is set to 'Unscaled Time')
        if (playerExplosionPrefab != null)
        {
            Instantiate(playerExplosionPrefab, deathPosition, Quaternion.identity);
            AudioSource.PlayClipAtPoint(explosionSFX,playerTransform.position);
        }

        // Use 'Realtime' because Time.timeScale is 0
        yield return new WaitForSecondsRealtime(delayInRealSeconds);

        ScoreManager sm = FindFirstObjectByType<ScoreManager>();
        if (finalScoreText != null && sm != null)
        {
            finalScoreText.text = "FINAL SCORE: " + sm.currentScore.ToString();
        }

        healthbar.SetActive(false);
        scoreboard.SetActive(false);

        if (gameOverPanel != null) gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
}