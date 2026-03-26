using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour
{
    public float flashDuration = 1f;
    private PlayerHealth healthSystem;
    private ScoreManager scoreSystem; // We will create this next!
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Coroutine colorCoroutine;

    void Start()
    {
        // Cache the references to other scripts on the player or in the scene
        healthSystem = GetComponent<PlayerHealth>();
        scoreSystem = FindFirstObjectByType<ScoreManager>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. Obstacles (Requirement 1)
        if (other.CompareTag("Obstacle"))
        {
            healthSystem.TakeDamage(10);
            Debug.Log("Hit Obstacle! Health: " + healthSystem.currentHealth);
            // Player passes through because the Obstacle is a Trigger
            TriggerFlash(Color.red);
        }

        // 2. Score Gaps / Holes (Requirement 2)
        if (other.CompareTag("Goal"))
        {
            if (scoreSystem != null)
            {
                scoreSystem.AddScore(10);
            }
        }

        // 3. Health Packs (Requirement 3)
        if (other.CompareTag("HealthPack"))
        {
            // Only pick up if health is NOT full
            if (healthSystem.currentHealth < healthSystem.maxHealth)
            {
                healthSystem.Heal(25);
                TriggerFlash(Color.green);
                Destroy(other.gameObject); 
                Debug.Log("Health Restored!");
            }
            else
            {
                Debug.Log("Health is already full! Leaving health pack.");
            }
        }

        // 4. Enemies (Requirement 5)
        if (other.CompareTag("Enemy"))
        {
            healthSystem.TakeDamage(15);
            Destroy(other.gameObject); // Destroyed on collision with player
            TriggerFlash(Color.red);
        }
    }

    void TriggerFlash(Color flashColor)
    {
        // If a flash is already happening, stop it so we can start a new one
        if (colorCoroutine != null) StopCoroutine(colorCoroutine);
        
        colorCoroutine = StartCoroutine(FlashSpriteColor(flashColor));
    }

    IEnumerator FlashSpriteColor(Color targetColor)
    {
        if (spriteRenderer == null) yield break;

        // Change to the feedback color
        spriteRenderer.color = targetColor;

        yield return new WaitForSeconds(flashDuration);

        // Change back to original color
        spriteRenderer.color = originalColor;
        
        colorCoroutine = null;
    }
}