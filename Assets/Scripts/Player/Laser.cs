using UnityEngine;

public class Laser : MonoBehaviour
{
    public float speed = 20f;
    public float lifeSpan = 2f; // Requirement 4: Destroy after short duration

    void Start()
    {
        Destroy(gameObject, lifeSpan);
    }

    void Update()
    {
        // Move the laser forward (to the right)
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Requirement 4: Destroyed upon collision with anything
        // (We will add enemy damage logic here later!)
        if (!collision.CompareTag("Player")) 
        {
            Destroy(gameObject);
        }
    }
}