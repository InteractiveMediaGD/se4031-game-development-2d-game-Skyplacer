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
    public AudioClip shootSound;
    public AudioClip jetpackSound;
    public AudioClip footstepSound;
    private AudioSource shootSource;    // For one-off laser sounds
    private AudioSource jetpackSource;  // For looping thrust sound
    private AudioSource footstepSource;
    public float fadeSpeed = 5f; // How fast the sound fades in/out
    private float targetJetpackVolume = 0.5f;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        shootSource = gameObject.AddComponent<AudioSource>();
        
        jetpackSource = gameObject.AddComponent<AudioSource>();
        jetpackSource.clip = jetpackSound;
        jetpackSource.loop = true; // Essential for continuous sounds

        footstepSource = gameObject.AddComponent<AudioSource>();
        footstepSource.clip = footstepSound;
        footstepSource.loop = true;
    }

    void Update()
    {
        if (Time.timeScale == 0)
        {
            if (jetpackSource.isPlaying) jetpackSource.Pause();
            if (footstepSource.isPlaying) footstepSource.Pause();
            return; // Exit early so no movement or shooting happens
        }

        jetpackSource.UnPause();
        footstepSource.UnPause();

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
        HandleContinuousSounds(isThrusting);
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

            if (shootSound != null)
            {
                shootSource.PlayOneShot(shootSound, 0.5f);
            }
        }
    }

    void HandleContinuousSounds(bool isThrusting)
    {
        // 1. Jetpack Sound Logic
        if (isThrusting)
        {
            if (!jetpackSource.isPlaying) jetpackSource.Play();
            
            // Gradually increase volume to max
            jetpackSource.volume = Mathf.MoveTowards(jetpackSource.volume, targetJetpackVolume, fadeSpeed * Time.deltaTime);
        }
        else
        {
            // Gradually decrease volume to 0
            jetpackSource.volume = Mathf.MoveTowards(jetpackSource.volume, 0, fadeSpeed * Time.deltaTime);
            
            // Only stop the source once it's completely silent
            if (jetpackSource.volume <= 0 && jetpackSource.isPlaying)
            {
                jetpackSource.Stop();
            }
        }

        // 2. Footstep Sound Logic 
        // We play footsteps if on ground AND NOT thrusting
        if (isGrounded && !isThrusting && !footstepSource.isPlaying)
        {
            footstepSource.Play();
        }
        else if ((!isGrounded || isThrusting) && footstepSource.isPlaying)
        {
            footstepSource.Stop();
        }
    }

    void UpdateAnimations(bool isThrusting)
    {
        // Parameter names must match your Animator Controller exactly
        anim.SetBool("isFloating", isThrusting);
        anim.SetBool("isGrounded", isGrounded);
    }
}