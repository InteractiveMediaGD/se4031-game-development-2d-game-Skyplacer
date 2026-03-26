using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int scoreValue = 5;
    public GameObject explosionPrefab;
    public AudioClip explosionSound;
    
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
            if (explosionPrefab != null)
            {
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            }

            if (explosionSound != null)
            {
                AudioSource.PlayClipAtPoint(explosionSound, transform.position, 1.0f);
            }
            // Requirement 5: Increase score on destruction by projectile
            FindFirstObjectByType<ScoreManager>().AddScore(scoreValue);
            
            // Destroy both the enemy and the projectile
            Destroy(other.gameObject); 
            Destroy(gameObject);
        }
    }
}