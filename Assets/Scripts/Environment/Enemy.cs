using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int scoreValue = 10;
    
    void Update()
    {
        // Movement: Move left based on the global game speed
        transform.Translate(Vector2.left * GameManager.GlobalSpeed * Time.deltaTime);

        // Cleanup: Destroy if it goes off-screen (Requirement 5)
        if (transform.position.x < -15f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Requirement 5: Destroyed on collision with projectiles
        if (other.CompareTag("Projectile")) 
        {
            // Requirement 5: Increase score on destruction by projectile
            FindFirstObjectByType<ScoreManager>().AddScore(scoreValue);
            
            // Destroy both the enemy and the projectile
            Destroy(other.gameObject); 
            Destroy(gameObject);
        }
    }
}