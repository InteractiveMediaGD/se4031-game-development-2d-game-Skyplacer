using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Slider healthBar;
    public Image fillImage;

    [Header("Feedback UI")]
    public TextMeshProUGUI warningText;
    public float flashSpeed = 8f;
    public AudioClip damageSFX;
    public AudioClip healSFX;

    void Start()
    {
        currentHealth = maxHealth;
        if (warningText != null) warningText.gameObject.SetActive(false);
        UpdateUI();
    }

    void Update()
    {
        if (currentHealth > 0 && (float)currentHealth / maxHealth < 0.3f)
        {
            FlashWarning();
        }
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        AudioSource.PlayClipAtPoint(damageSFX, transform.position, 1.0f);
        UpdateUI();

        if (currentHealth <= 0)
        {
            int finalScore = 0;
            ScoreManager sm = FindFirstObjectByType<ScoreManager>();
            if (sm != null) finalScore = sm.currentScore;

            // Hand everything over to the GameManager
            GameManager.Instance.TriggerGameOver(gameObject);
        }
    }

    public void Heal(int amount)
    {
        if (currentHealth >= maxHealth) return;
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        AudioSource.PlayClipAtPoint(healSFX, transform.position, 1.0f);
        UpdateUI();
    }

    void UpdateUI()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
            float healthPercent = (float)currentHealth / maxHealth;
            fillImage.color = Color.Lerp(Color.red, Color.green, healthPercent);

            if (warningText != null)
                warningText.gameObject.SetActive(healthPercent < 0.3f);
        }
    }

    void FlashWarning()
    {
        if (warningText == null) return;
        float alpha = (Mathf.Sin(Time.unscaledTime * flashSpeed) + 1f) / 2f;
        warningText.color = new Color(1f, 0f, 0f, Mathf.Clamp(alpha, 0.2f, 1f));
    }
}