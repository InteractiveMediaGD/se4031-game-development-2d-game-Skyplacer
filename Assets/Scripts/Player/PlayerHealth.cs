using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Slider healthBar;
    public Image fillImage;

    [Header("UI Reference")]
    public GameObject gameOverPanel; // Drag the GameOverPanel here in Inspector

    void Start()
    {
        currentHealth = maxHealth;
        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        // Don't heal if already at max (Assignment Rule 3)
        if (currentHealth >= maxHealth) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateUI();
    }

    void Die()
    {
        // Show the Game Over screen
        if (gameOverPanel != null) gameOverPanel.SetActive(true);

        // Stop the game movement
        Time.timeScale = 0; // Freezes physics and time
        GetComponent<PlayerController>().enabled = false;
    }

    void UpdateUI()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
            float healthPercent = (float)currentHealth / maxHealth;
            fillImage.color = Color.Lerp(Color.red, Color.green, healthPercent);
        }
    }
}