using UnityEngine;
using UnityEngine.UI; // Required for the Slider/UI

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI Reference")]
    public Slider healthBar; // Drag your UI Slider here in the Inspector

    private PlayerController playerMovement;

    void Start()
    {
        playerMovement = GetComponent<PlayerController>();
        ResetHealth();
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        UpdateUI();
        
        // Ensure player can move again if they were stopped
        if(playerMovement != null) playerMovement.enabled = true;
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
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateUI();
    }

    void UpdateUI()
    {
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    void Die()
    {
        Debug.Log("Player is out of health!");
        // Requirement 1: Stop the player
        playerMovement.enabled = false;
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        GetComponent<Rigidbody2D>().simulated = false; // "Freezes" them in place
        
        // You would typically trigger a Game Over screen here
    }
}