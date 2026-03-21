using UnityEngine;

public class HealthPack : MonoBehaviour
{
    void Update()
    {
        // Movement: Match the game speed
        transform.Translate(Vector2.left * GameManager.GlobalSpeed * Time.deltaTime);

        // Requirement 3: Destroy shortly after the player goes past
        if (transform.position.x < -15f)
        {
            Destroy(gameObject);
        }
    }
}