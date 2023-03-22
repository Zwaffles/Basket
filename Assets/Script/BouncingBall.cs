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
    [Tooltip("Force added to the ball on side bounces")]
    [SerializeField] float sidesBouncingForce;
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
    [Tooltip("Ball max speed")]
    [SerializeField] float maxSpeed = 50;
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
    [SerializeField] float extraAddedForceToFireball = 4.5f;
    bool onFire;
    float DefaultBouncingForce;
    [Header("Roof")]
    [Tooltip("The down force of the roof when the ball hits it")]
    //[SerializeField] float roofBounciness;
    //[SerializeField] float roofReflection = 1.5f;
    [SerializeField] float slowdownFactorTop = .5f;
    [SerializeField] float slowdownFactorBottom = .7f;

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
        // Get the current velocity of the Rigidbody
        Vector3 velocity = rb.velocity;

        // Clamp the magnitude of the velocity vector to the maximum speed
        Vector3 clampedVelocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        // Apply the clamped velocity to the Rigidbody
        rb.velocity = clampedVelocity;

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
        //it was 70 so the fire ball will be triggered
        if (rb.velocity.magnitude >= 700 && fireBallTimer < fireBallTime)
        {
            //fireBallFlash.Play();
            //fireBallSmoke.Play();
            //fireBallSpark.Play();
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
            //fireBallFlash.Stop();
            //fireBallSmoke.Stop();
            //fireBallSpark.Stop();
            onFire = true;
            fireBallTime = 12;
        }
    }
    
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
    [SerializeField] bool collidingWithPlayerOne = false;
    [SerializeField] bool isReversed = false;
    void OnCollisionEnter(Collision collision)
    {
        GameManager.instance.audioManager.PlaySfx("Basket_Bounce_1-SFX", Random.Range(0.64f, 1.3f));

        if (collision.collider.CompareTag("PlayerOne"))
        {
            collidingWithPlayerOne = true;
            //The inclination of the ball according to the movement of the cube
            if (!isReversed)
            {
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
            }
            else if (isReversed)
            {
                if (playerMoveDirection > 0)
                {
                    rb.AddForce(Vector3.left * ballInclination, ForceMode.Impulse);
                    //Debug.Log("+ " + ballInclination);
                }

                if (playerMoveDirection < 0)
                {
                    rb.AddForce(Vector3.right * ballInclination, ForceMode.Impulse);
                    //Debug.Log("- " + ballInclination);
                }
            }
            
            // Get the normal vector of the collision
            //Vector3 normal = collision.contacts[0].normal;

            // Calculate the reflection vector using the normal and the incoming velocity vector
            //Vector3 reflection = Vector3.Reflect(rb.velocity, normal);

            // Set the ball's velocity to the reflection vector multiplied by the blockerBoost value
            //rb.velocity = reflection * blockerBoost;

            // Add an upward force to make the ball jump
            rb.AddForce(Vector3.up * blockerBoost, ForceMode.Impulse);
        }
        if (collision.collider.CompareTag("Block"))
        {
            // Get the normal vector of the collision
            //Vector3 normal = collision.contacts[0].normal;

            //// Calculate the reflection vector using the normal and the incoming velocity vector
            //Vector3 reflection = Vector3.Reflect(rb.velocity, normal);

            //// Set the ball's velocity to the reflection vector
            //rb.velocity = reflection;
            rb.AddForce(Vector3.down * bounceForce, ForceMode.Impulse);
        }
        if (collision.collider.CompareTag("Edges"))
        {
            rb.velocity *= slowdownFactorTop;
        }
        if (collision.collider.CompareTag("DownSides"))
        {
            rb.velocity *= slowdownFactorBottom;
        }
        if (collision.collider.CompareTag("Roof"))
        {
            // Get the normal vector of the collision
            Vector3 normal = collision.contacts[0].normal;

            // Calculate the reflection vector using the normal and the incoming velocity vector
            Vector3 reflection = Vector3.Reflect(rb.velocity, normal);

            // Set the ball's velocity to the reflection vector
            //rb.velocity = reflection * roofReflection;

            // Add an upward force to make the ball jump
            //rb.AddForce(Vector3.down * (rb.velocity.y * roofBounciness), ForceMode.Impulse);
        }
        // Check if the ball has collided with an object with the "Ground" tag
        if (collision.collider.CompareTag("Ground"))
        {
            // Get the normal vector of the collision
            //Vector3 normal = collision.contacts[0].normal;

            //// Calculate the reflection vector using the normal and the incoming velocity vector
            //Vector3 reflection = Vector3.Reflect(rb.velocity, normal);

            //// Set the ball's velocity to the reflection vector
            //rb.velocity = reflection;

            // Add an upward force to make the ball jump
            rb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
        }
        
        // Check if the ball has collided with an object with the "Ground" tag
        if (collision.collider.CompareTag("RightSide"))
        {
            // Get the normal vector of the collision
            Vector3 normal = collision.contacts[0].normal;

            // Calculate the reflection vector using the normal and the incoming velocity vector
            Vector3 reflection = Vector3.Reflect(rb.velocity, normal);

            // Set the ball's velocity to the reflection vector
            rb.velocity = reflection;

            // Add a turn force to make the ball jump
            rb.AddForce(Vector3.left * sidesBouncingForce, ForceMode.Impulse);
        }
        if (collision.collider.CompareTag("LeftSide"))
        {
            // Get the normal vector of the collision
            Vector3 normal = collision.contacts[0].normal;

            // Calculate the reflection vector using the normal and the incoming velocity vector
            Vector3 reflection = Vector3.Reflect(rb.velocity, normal);

            // Set the ball's velocity to the reflection vector
            rb.velocity = reflection;

            // Add a turn force to make the ball jump
            rb.AddForce(Vector3.right * sidesBouncingForce, ForceMode.Impulse);
        }
        if (collision.collider.CompareTag("PlayerTwo"))
        {
            if (!isReversed)
            {
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
            }
            else if (isReversed)
            {
                if (player2MoveDirection > 0)
                {
                    rb.AddForce(Vector3.left * ballInclination, ForceMode.Impulse);
                    //Debug.Log("+ " + ballInclination);
                }

                if (player2MoveDirection < 0)
                {
                    rb.AddForce(Vector3.right * ballInclination, ForceMode.Impulse);
                    //Debug.Log("- " + ballInclination);
                }
            }
            // Get the normal vector of the collision
            //Vector3 normal = collision.contacts[0].normal;

            // Calculate the reflection vector using the normal and the incoming velocity vector
            //Vector3 reflection = Vector3.Reflect(rb.velocity, normal);

            // Set the ball's velocity to the reflection vector multiplied by the blockerBoost value
            //rb.velocity = reflection * blockerBoost;

            // Add an upward force to make the ball jump
            if (!collidingWithPlayerOne)
            rb.AddForce(Vector3.up * blockerBoost, ForceMode.Impulse);
        }
        // Check if the ball has collided with an object with the "Block" tag
        

        if (collision.collider.CompareTag("AI"))
        {
            if (!isReversed)
            {
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
            }
            else if (isReversed)
            {
                if (AIMoveDirection > 0)
                {
                    rb.AddForce(Vector3.left * ballInclination, ForceMode.Impulse);
                    //Debug.Log("+ " + ballInclination);
                }

                if (AIMoveDirection < 0)
                {
                    rb.AddForce(Vector3.right * ballInclination, ForceMode.Impulse);
                    //Debug.Log("- " + ballInclination);
                }
            }
            // Get the normal vector of the collision
            //Vector3 normal = collision.contacts[0].normal;

            // Calculate the reflection vector using the normal and the incoming velocity vector
            //Vector3 reflection = Vector3.Reflect(rb.velocity, normal);

            // Set the ball's velocity to the reflection vector multiplied by the blockerBoost value
            //rb.velocity = reflection * blockerBoost;

            // Add an upward force to make the ball jump
            if (!collidingWithPlayerOne)
            rb.AddForce(Vector3.up * blockerBoost, ForceMode.Impulse);
        }

    }
    
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("PlayerOne"))
        {
            collidingWithPlayerOne = false;
        }
    }
    [SerializeField] float forcePower = 5;
    bool enteredRightEdgeTrigger = false;
    bool enteredLeftEdgeTrigger = false;
    //HarderHitOnEdges

    [SerializeField] Rigidbody player1Rb;
    [SerializeField] Rigidbody player2Rb;
    [SerializeField] Rigidbody AIRb;

    [SerializeField] float multiplyerValue = 8;
    [SerializeField] float turningBySpeed = .15f;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "RightEdge" && rb.velocity.y < 0 && !enteredLeftEdgeTrigger)
        {
            enteredRightEdgeTrigger = true;
            if (other.gameObject.name == "RightForce1")
            {
                if(player1Rb.velocity.x < 0)
                rb.AddForce(Vector3.left * edgesBoost * Mathf.Abs(player1Rb.velocity.x * turningBySpeed), ForceMode.Impulse);
                else if(player1Rb.velocity.x > 0)
                {
                    rb.AddForce(Vector3.left * edgesBoost * multiplyerValue, ForceMode.Impulse);
                }
            }
            if (other.gameObject.name == "RightForce2")
            {
                if (player2Rb.velocity.x < 0)
                    rb.AddForce(Vector3.left * edgesBoost * Mathf.Abs(player2Rb.velocity.x * turningBySpeed), ForceMode.Impulse);
                else if (player2Rb.velocity.x > 0)
                {
                    rb.AddForce(Vector3.left * edgesBoost * multiplyerValue, ForceMode.Impulse);
                }
            }
            if (other.gameObject.name == "RightForceAI")
            {
                if (AIRb.velocity.x < 0)
                    rb.AddForce(Vector3.left * edgesBoost * Mathf.Abs(AIRb.velocity.x * turningBySpeed), ForceMode.Impulse);
                else if (player1Rb.velocity.x > 0)
                {
                    rb.AddForce(Vector3.left * edgesBoost * multiplyerValue, ForceMode.Impulse);
                }
            }
        }
        if (other.tag == "LeftEdge" && rb.velocity.y < 0 && !enteredRightEdgeTrigger)
        {
            enteredLeftEdgeTrigger = true;
            if (other.gameObject.name == "LeftForce1")
            {
                if (player1Rb.velocity.x > 0)
                    rb.AddForce(Vector3.right * edgesBoost * player1Rb.velocity.x, ForceMode.Impulse);
                else if (player1Rb.velocity.x < 0)
                {
                    rb.AddForce(Vector3.right * edgesBoost * multiplyerValue, ForceMode.Impulse);
                }
            }
            if (other.gameObject.name == "LeftForce2")
            {
                if (player2Rb.velocity.x > 0)
                    rb.AddForce(Vector3.right * edgesBoost * player2Rb.velocity.x, ForceMode.Impulse);
                else if (player2Rb.velocity.x < 0)
                {
                    rb.AddForce(Vector3.right * edgesBoost * 10, ForceMode.Impulse);
                }
            }
            if (other.gameObject.name == "LeftForceAI")
            {
                if (AIRb.velocity.x > 0)
                    rb.AddForce(Vector3.right * edgesBoost * AIRb.velocity.x, ForceMode.Impulse);
                else if (AIRb.velocity.x < 0)
                {
                    rb.AddForce(Vector3.right * edgesBoost * 10, ForceMode.Impulse);
                }
            }

        }
        if (other.tag == "RightForce" && rb.velocity.y < 0)
        {          
            rb.AddForce(Vector3.left * forcePower, ForceMode.Impulse);
        }
        if (other.tag == "LeftForce" && rb.velocity.y < 0)
        {
            
            rb.AddForce(Vector3.right * forcePower, ForceMode.Impulse);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "RightEdge")
        {
            enteredRightEdgeTrigger = false;
        }
        if(other.tag == "LeftEdge")
        {
            enteredLeftEdgeTrigger = false;
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
