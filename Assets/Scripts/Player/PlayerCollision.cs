using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private PlayerHealth healthSystem;
    private ScoreManager scoreSystem; // We will create this next!

    void Start()
    {
        // Cache the references to other scripts on the player or in the scene
        healthSystem = GetComponent<PlayerHealth>();
        scoreSystem = FindFirstObjectByType<ScoreManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. Obstacles (Requirement 1)
        if (other.CompareTag("Obstacle"))
        {
            healthSystem.TakeDamage(10);
            Debug.Log("Hit Obstacle! Health: " + healthSystem.currentHealth);
            // Player passes through because the Obstacle is a Trigger
        }

        // 2. Score Gaps / Holes (Requirement 2)
        if (other.CompareTag("Goal"))
        {
            if (scoreSystem != null)
            {
                scoreSystem.AddScore(1);
            }
        }

        // 3. Health Packs (Requirement 3)
        if (other.CompareTag("HealthPack"))
        {
            healthSystem.Heal(20);
            Destroy(other.gameObject); // Destroyed upon collision
        }

        // 4. Enemies (Requirement 5)
        if (other.CompareTag("Enemy"))
        {
            healthSystem.TakeDamage(15);
            Destroy(other.gameObject); // Destroyed on collision with player
        }
    }
}