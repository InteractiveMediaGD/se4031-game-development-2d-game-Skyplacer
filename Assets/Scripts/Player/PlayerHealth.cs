using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Slider healthBar;
    public Image fillImage;

    [Header("UI Reference")]
    public GameObject gameOverPanel; // Drag the GameOverPanel here in Inspector
    public TextMeshProUGUI warningText;
    public float flashSpeed = 8f;

    void Start()
    {
        currentHealth = maxHealth;
        if (warningText != null) warningText.gameObject.SetActive(false);
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

    void Update()
    {
        if (currentHealth > 0 && (float)currentHealth / maxHealth < 0.3f)
        {
            FlashWarning();
        }
    }

    void UpdateUI()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
            float healthPercent = (float)currentHealth / maxHealth;
            fillImage.color = Color.Lerp(Color.red, Color.green, healthPercent);

            if (warningText != null)
            {
                warningText.gameObject.SetActive(healthPercent < 0.3f);
            }
        }
    }

    void FlashWarning()
    {
        if (warningText == null) return;

        // Use a Sine wave to oscillate alpha between 0.2 and 1.0
        float alpha = (Mathf.Sin(Time.time * flashSpeed) + 1f) / 2f;
        warningText.color = new Color(1f, 0f, 0f, Mathf.Clamp(alpha, 0.2f, 1f));
    }
}