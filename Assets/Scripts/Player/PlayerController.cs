using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float upwardForce = 15f;
    private Rigidbody2D rb;
    private Animator anim;

    [Header("Ground Detection")]
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask whatIsGround;
    private bool isGrounded;

    [Header("Shooting Settings")]
    public GameObject projectilePrefab; // Assign your laser prefab here
    public Transform muzzlePoint;      // A child object at the player's gun tip

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // 1. Ground Detection
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        // 2. Jetpack Input (Thrusting)
        bool isThrusting = Input.GetKey(KeyCode.Space) || Input.GetMouseButton(1);
        if (isThrusting)
        {
            ApplyThrust();
        }

        // 3. Shooting Input (Requirement 4: Left Mouse Click)
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        // 4. Animation Logic
        UpdateAnimations(isThrusting);
    }

    void ApplyThrust()
    {
        // Upward movement logic
        rb.linearVelocity = new Vector2(0, upwardForce);
    }

    void Shoot()
    {
        if (projectilePrefab != null && muzzlePoint != null)
        {
            // Create the laser at the gun tip's position and rotation
            Instantiate(projectilePrefab, muzzlePoint.position, muzzlePoint.rotation);
        }
    }

    void UpdateAnimations(bool isThrusting)
    {
        // Parameter names must match your Animator Controller exactly
        anim.SetBool("isFloating", isThrusting);
        anim.SetBool("isGrounded", isGrounded);
    }
}