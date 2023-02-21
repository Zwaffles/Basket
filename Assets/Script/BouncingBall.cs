using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingBall : MonoBehaviour
{
    #region Speed and Bouncing Variables
    [Header("Speed and Bouncing Variables")]
    [Tooltip("Initial speed of the ball when it is first launched")]
    public float initialSpeed = 5.0f;
    [Tooltip("Force added to the ball on each bounce")]
    public float bounceForce = 10.0f;
    #endregion

    #region Border Variables
    [Header("Border Variables")]
    [Tooltip("Left border of the play area")]
    public float leftBorder = -5.0f;
    [Tooltip("Right border of the play area")]
    public float rightBorder = 5.0f;
    [Tooltip("Top border of the play area")]
    public float topBorder = 5.0f;
    [Tooltip("Bottom border of the play area")]
    public float bottomBorder = -5.0f;
    #endregion

    #region Ball Variables
    [Header("Ball Variables")]
    [Tooltip("Radius of the ball collider")]
    public float radius = .5f;
    [Tooltip("Multiplier for the velocity of the ball after it hits a block")]
    [SerializeField] private float blockerBoost = 1.5f;
    #endregion

    #region Inclination Variables
    [Header("Inclination Variables")]
    [Tooltip("The speed at which the ball moves in the direction of the paddle")]
    [SerializeField] float ballInclination = 1f;
    [Tooltip("Multiplier for the velocity of the ball after it hits the edges of the play area")]
    [SerializeField] float edgesBoost = 1f;   
    #endregion
    private Rigidbody rb;
    private PlayerController playerController;
    private Player2Controller player2Controller;
    private CubeMover cubeMover;
    private int playerMoveDirection;
    private int AIMoveDirection;
    private int player2MoveDirection;
    [SerializeField] ParticleSystem fireBallFlash;
    [SerializeField] public ParticleSystem fireBallSmoke;
    [SerializeField] ParticleSystem fireBallSpark;
    float fireBallTimer = 5;
    public float fireBallTime = 12;
    bool onFire;
    float DefaultBouncingForce;
    [Header("Roof")]
    [Tooltip("The down force of the roof when the ball hits it")]
    [SerializeField] float roofBounciness;
    [SerializeField] float roofReflection = 1.5f;

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        player2Controller = FindObjectOfType<Player2Controller>();
        cubeMover = FindObjectOfType<CubeMover>();
    }
    void Start()
    {
        DefaultBouncingForce = bounceForce;
        // Get the rigidbody component
        rb = GetComponent<Rigidbody>();

        // Set the initial velocity of the ball
        rb.velocity = Vector3.down * initialSpeed;
    }

    void FixedUpdate()
    {
        // Check if the ball has crossed the left or right border
        if (transform.position.x < leftBorder || transform.position.x > rightBorder)
        {
            // Reverse the horizontal velocity of the ball
            rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
        }

        // Check if the ball has crossed the top or bottom border
        if (transform.position.y < bottomBorder || transform.position.y > topBorder)
        {
            // Reverse the vertical velocity of the ball
            rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y);
        }

        if (rb.velocity.magnitude >= 70 && fireBallTimer < fireBallTime)
        {
            fireBallFlash.Play();
            fireBallSmoke.Play();
            fireBallSpark.Play();
            onFire = false;
            if (fireBallTime > 7)
                IncreaseForce(true);

        }
        if (!onFire)
        {
            fireBallTime -= Time.deltaTime;
        }
        if (fireBallTime <= 7)
        {
            IncreaseForce(false);
        }
        if (fireBallTimer > fireBallTime)
        {
            fireBallFlash.Stop();
            fireBallSmoke.Stop();
            fireBallSpark.Stop();
            onFire = true;
            fireBallTime = 12;
        }
    }
    [SerializeField] float extraAddedForceToFireball = 4.5f;
    void IncreaseForce(bool on)
    {
        if (bounceForce < DefaultBouncingForce - 0.5f && on)
        {
            bounceForce += extraAddedForceToFireball;
        }
        else if (bounceForce > DefaultBouncingForce + 0.5 && !on)
        {
            bounceForce -= extraAddedForceToFireball;
        }
    }
    private void Update()
    {
        if (playerController != null)
        {
            playerMoveDirection = playerController.moveDirection;
        }
        if (cubeMover != null)
            AIMoveDirection = cubeMover.moveDirection;
        if (player2Controller != null)
        {
            player2MoveDirection = player2Controller.player2MoveDirection;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Block"))
        {
            // Get the normal vector of the collision
            Vector3 normal = collision.contacts[0].normal;

            // Calculate the reflection vector using the normal and the incoming velocity vector
            Vector3 reflection = Vector3.Reflect(rb.velocity, normal);

            // Set the ball's velocity to the reflection vector
            rb.velocity = reflection;
        }

        if (collision.collider.CompareTag("Roof"))
        {
            // Get the normal vector of the collision
            Vector3 normal = collision.contacts[0].normal;

            // Calculate the reflection vector using the normal and the incoming velocity vector
            Vector3 reflection = Vector3.Reflect(rb.velocity, normal);

            // Set the ball's velocity to the reflection vector
            rb.velocity = reflection * roofReflection;

            // Add an upward force to make the ball jump
            rb.AddForce(Vector3.down * (rb.velocity.y * roofBounciness), ForceMode.Impulse);
        }
        // Check if the ball has collided with an object with the "Ground" tag
        if (collision.collider.CompareTag("Ground"))
        {
            // Get the normal vector of the collision
            Vector3 normal = collision.contacts[0].normal;

            // Calculate the reflection vector using the normal and the incoming velocity vector
            Vector3 reflection = Vector3.Reflect(rb.velocity, normal);

            // Set the ball's velocity to the reflection vector
            rb.velocity = reflection;

            // Add an upward force to make the ball jump
            rb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
        }
        if (collision.collider.CompareTag("PlayerTwo"))
        {
            //The inclination of the ball according to the movement of the cube
            if (player2MoveDirection > 0)
            {
                rb.AddForce(Vector3.right * ballInclination, ForceMode.Impulse);
                //Debug.Log("+ " + ballInclination);
            }

            if (player2MoveDirection < 0)
            {
                rb.AddForce(Vector3.left * ballInclination, ForceMode.Impulse);
                //Debug.Log("- " + ballInclination);
            }
            // Get the normal vector of the collision
            //Vector3 normal = collision.contacts[0].normal;

            // Calculate the reflection vector using the normal and the incoming velocity vector
            //Vector3 reflection = Vector3.Reflect(rb.velocity, normal);

            // Set the ball's velocity to the reflection vector multiplied by the blockerBoost value
            //rb.velocity = reflection * blockerBoost;

            // Add an upward force to make the ball jump
            rb.AddForce(Vector3.up * bounceForce * blockerBoost, ForceMode.Impulse);
        }
        // Check if the ball has collided with an object with the "Block" tag
        if (collision.collider.CompareTag("PlayerOne"))
        {
            //The inclination of the ball according to the movement of the cube
            if (playerMoveDirection > 0)
            {
                rb.AddForce(Vector3.right * ballInclination, ForceMode.Impulse);
                //Debug.Log("+ " + ballInclination);
            }

            if (playerMoveDirection < 0)
            {
                rb.AddForce(Vector3.left * ballInclination, ForceMode.Impulse);
                //Debug.Log("- " + ballInclination);
            }
            // Get the normal vector of the collision
            //Vector3 normal = collision.contacts[0].normal;

            // Calculate the reflection vector using the normal and the incoming velocity vector
            //Vector3 reflection = Vector3.Reflect(rb.velocity, normal);

            // Set the ball's velocity to the reflection vector multiplied by the blockerBoost value
            //rb.velocity = reflection * blockerBoost;

            // Add an upward force to make the ball jump
            rb.AddForce(Vector3.up * bounceForce * blockerBoost, ForceMode.Impulse);
        }

        if (collision.collider.CompareTag("AI"))
        {
            //The inclination of the ball according to the movement of the cube
            if (AIMoveDirection > 0)
            {
                rb.AddForce(Vector3.right * ballInclination, ForceMode.Impulse);
                //Debug.Log("+ " + ballInclination);
            }

            if (AIMoveDirection < 0)
            {
                rb.AddForce(Vector3.left * ballInclination, ForceMode.Impulse);
                //Debug.Log("- " + ballInclination);
            }
            // Get the normal vector of the collision
            //Vector3 normal = collision.contacts[0].normal;

            // Calculate the reflection vector using the normal and the incoming velocity vector
            //Vector3 reflection = Vector3.Reflect(rb.velocity, normal);

            // Set the ball's velocity to the reflection vector multiplied by the blockerBoost value
            //rb.velocity = reflection * blockerBoost;

            // Add an upward force to make the ball jump
            rb.AddForce(Vector3.up * bounceForce * blockerBoost, ForceMode.Impulse);
        }

    }

    //HarderHitOnEdges
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerOne" || other.tag == "PlayerTwo" || other.tag == "AI")
        {
            rb.AddForce(Vector3.up * edgesBoost, ForceMode.Impulse);
            Debug.Log("+ edgeBoost" + edgesBoost);
        }

        //}
        void OnDrawGizmosSelected()
        {
            // Draw lines for the borders in the Unity editor
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(new Vector3(leftBorder, topBorder, 0), radius);
            Gizmos.DrawWireSphere(new Vector3(rightBorder, topBorder, 0), radius);
            Gizmos.DrawWireSphere(new Vector3(rightBorder, bottomBorder, 0), radius);
            Gizmos.DrawWireSphere(new Vector3(leftBorder, bottomBorder, 0), radius);
        }
    }
}
