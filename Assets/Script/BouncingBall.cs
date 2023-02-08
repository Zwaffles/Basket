using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingBall : MonoBehaviour
{
    // Variables for ball speed and bouncing force
    public float initialSpeed = 5.0f;
    public float bounceForce = 10.0f;

    // Variables for the borders
    public float leftBorder = -5.0f;
    public float rightBorder = 5.0f;
    public float topBorder = 5.0f;
    public float bottomBorder = -5.0f;

    public float radius = .5f;

    [SerializeField] private float blockerBoost = 1.5f;
    // Rigidbody component of the ball
    private Rigidbody rb;
    [SerializeField] float ballInclination = 1f;
    [SerializeField] float edgesBoost = 1f;
    PlayerController playerController;
    Player2Controller player2Controller;
    CubeMover cubeMover;
    private int playerMoveDirection;
    private int AIMoveDirection;
    private int player2MoveDirection;
    
    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        player2Controller = FindObjectOfType<Player2Controller>();
        cubeMover = FindObjectOfType<CubeMover>();
    }
    void Start()
    {        
        
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
    }
    private void Update()
    {
        if(playerController != null)
        {
            playerMoveDirection = playerController.moveDirection;
        }
        if(cubeMover != null)
        AIMoveDirection = cubeMover.moveDirection;
        if(player2Controller != null)
        {
            player2MoveDirection = player2Controller.player2MoveDirection;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
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
        // Check if the ball has collided with an object with the "Block" tag
        if (collision.collider.CompareTag("Block"))
        {
            //The inclination of the ball according to the movement of the cube
            if (playerMoveDirection > 0 || AIMoveDirection > 0 || player2MoveDirection > 0)
            {
                rb.AddForce(Vector3.right * ballInclination, ForceMode.Impulse);
                //Debug.Log("+ " + ballInclination);
            }
            else if (playerMoveDirection < 0 || AIMoveDirection < 0 || player2MoveDirection < 0)
            {
                rb.AddForce(Vector3.left * ballInclination, ForceMode.Impulse);
                //Debug.Log("- " + ballInclination);
            }
            // Get the normal vector of the collision
            Vector3 normal = collision.contacts[0].normal;

            // Calculate the reflection vector using the normal and the incoming velocity vector
            Vector3 reflection = Vector3.Reflect(rb.velocity, normal);

            // Set the ball's velocity to the reflection vector multiplied by the blockerBoost value
            rb.velocity = reflection * blockerBoost;

            // Add an upward force to make the ball jump
            rb.AddForce(Vector3.up * bounceForce * blockerBoost, ForceMode.Impulse);
        }
    }

    //HarderHitOnEdges
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Block")
        {
            rb.AddForce(Vector3.up * edgesBoost, ForceMode.Impulse);
            Debug.Log("+ edgeBoost" + edgesBoost);
        }
        
    }
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
