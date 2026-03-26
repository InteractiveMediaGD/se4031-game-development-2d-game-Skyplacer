using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int scoreValue = 5;
    public bool isTracer = false; // Toggle this in the Inspector for specific prefabs
    public float tracerSpeedMultiplier = 1.2f; // Tracers are usually faster/aggressive

    [Header("Effects")]
    public GameObject explosionPrefab;
    public AudioClip explosionSound;

    private Transform playerTransform;

    void Start()
    {
        // Find the player automatically using the "Player" tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void Update()
    {
        if (isTracer && playerTransform != null)
        {
            HandleTracerMovement();
        }
        else
        {
            HandleStandardMovement();
        }

        // Cleanup: Destroy if it goes too far off-screen
        if (transform.position.x < -15f || transform.position.x > 20f || Mathf.Abs(transform.position.y) > 10f)
        {
            Destroy(gameObject);
        }
    }

    void HandleStandardMovement()
    {
        // Original logic: Move left based on global speed
        transform.Translate(Vector2.left * GameManager.GlobalSpeed * Time.deltaTime);
    }

    void HandleTracerMovement()
    {
        // 1. Calculate the direction toward the player
        // Direction = (Target Position - Current Position) normalized
        Vector2 direction = (playerTransform.position - transform.position).normalized;

        // 2. Move toward the player
        // We use Space.World to ensure the movement is global, not relative to enemy rotation
        transform.Translate(direction * GameManager.GlobalSpeed * tracerSpeedMultiplier * Time.deltaTime, Space.World);

        // 3. Optional: Rotate to face the player (for that "locked-on" look)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 180f); // Adjust 180f depending on sprite orientation
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
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

            ScoreManager scoreMan = FindFirstObjectByType<ScoreManager>();
            if (scoreMan != null) scoreMan.AddScore(scoreValue);
            
            Destroy(other.gameObject); 
            Destroy(gameObject);
        }
    }
}