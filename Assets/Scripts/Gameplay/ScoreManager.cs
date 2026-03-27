using UnityEngine;
using TMPro; // Use this if you are using TextMeshPro for the UI

public class ScoreManager : MonoBehaviour
{
    public int currentScore = 0;
    public TextMeshProUGUI scoreText; // Drag your UI Text here

    void Start()
    {
        ResetScore();
    }

    public void AddScore(int amount)
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameOver) return;
        
        currentScore += amount;
        UpdateUI();
    }

    public void ResetScore()
    {
        currentScore = 0;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore.ToString();
        }
    }
}