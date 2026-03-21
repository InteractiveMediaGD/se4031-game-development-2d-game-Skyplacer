using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float upwardForce = 15f;
    private Rigidbody2D rb;

    [Header("Shooting Settings")]
    public GameObject projectilePrefab;
    public Transform muzzlePoint; // Assign a child object of the player here

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 1. Jetpack Logic (Hold to fly)
        if (Input.GetMouseButton(1) || Input.GetKey(KeyCode.Space)) 
        {
            ApplyThrust();
        }

        // 2. Shooting Logic (Requirement 4: Left click to shoot)
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void ApplyThrust()
    {
        // Resets velocity for a "snappier" feel, or just AddForce for momentum
        rb.linearVelocity = new Vector2(0, upwardForce);
    }

    void Shoot()
    {
        Debug.Log("Shoot Button Pressed");

        if (projectilePrefab != null && muzzlePoint != null)
        {
            // Create the projectile at the muzzle position
            Instantiate(projectilePrefab, muzzlePoint.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Hey! You forgot to assign the Prefab or MuzzlePoint in the Inspector!");
        }
    }
}