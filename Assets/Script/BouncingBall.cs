using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingBall : MonoBehaviour
{
    #region Speed and Bouncing Variables
    [Header("Speed and Bouncing Variables")]
    [Tooltip("Initial speed of the ball when it is first launched")]
    public float initialSpeed = 10.0f;
    [Tooltip("Force added to the ball on each bounce")]
    public float bounceForce = 10.0f;
    #endregion

    #region Ball Variables
    [Header("Ball Variables")]
    [Tooltip("Multiplier for the velocity of the ball after it hits a paddle")]
    [SerializeField] private float blockerBoost = 51f;
    [Tooltip("Ball max speed")]
    [SerializeField] float maxSpeed = 36;
    #endregion

    #region Inclination Variables
    [Header("Inclination Variables")]
    [Tooltip("The speed at which the ball moves in the direction of the paddle")]
    [SerializeField] float turningBySpeed = .15f;
    [Tooltip("Boost for the velocity of the ball after it hits the edges of the paddle")]
    [SerializeField] float edgesBoost = 1f;
    #endregion

    #region General Variables
    Rigidbody rb;
    [Header("General Variables")]
    [SerializeField] Rigidbody player1Rb;
    [SerializeField] Rigidbody player2Rb;
    [SerializeField] Rigidbody AIRb;
    PlayerController playerController;
    Player2Controller player2Controller;
    // Bot script
    private CubeMover cubeMover;
    // right and left edges of the paddles
    bool enteredRightEdgeTrigger = false;
    bool enteredLeftEdgeTrigger = false;
    int playerMoveDirection;
    int player2MoveDirection;
    int AIMoveDirection;
    #endregion

    #region Walls and Blockers
    [Header("Walls and Blockers")]
    [Tooltip("Added force to the side wall over the baskets")]
    [SerializeField] float slowdownFactorTop = 0.1085f;
    [Tooltip("Added force to the bottom of each basket")]
    [SerializeField] float slowdownFactorBottom = .7f;
    #endregion

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
        // Get the current velocity of the Rigidbody
        Vector3 velocity = rb.velocity;

        // Clamp the magnitude of the velocity vector to the maximum speed
        Vector3 clampedVelocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        // Apply the clamped velocity to the Rigidbody
        rb.velocity = clampedVelocity;
    }
    private void Update()
    {
        if (playerController != null)
        {
            playerMoveDirection = playerController.moveDirection;
        }
        if (cubeMover != null)
        {
            AIMoveDirection = cubeMover.moveDirection;
        }
        if (player2Controller != null)
        {
            player2MoveDirection = player2Controller.player2MoveDirection;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        try
        {
            GameManager.instance.audioManager.PlaySfx("Basket_Bounce_1-SFX", Random.Range(0.64f, 1.3f));
        }
        catch
        {
            Debug.LogWarning("AudioManager not found. Perhaps you're not using the GameManager prefab?");
        }

        if (collision.collider.CompareTag("PlayerOne"))
        {
            //rb.AddForce(Vector3.up * blockerBoost, ForceMode.Impulse);
        }

        if (collision.collider.CompareTag("PlayerTwo"))
        {
            //rb.AddForce(Vector3.up * blockerBoost, ForceMode.Impulse);
        }

        if (collision.collider.CompareTag("AI"))
        {
            //rb.AddForce(Vector3.up * blockerBoost, ForceMode.Impulse);
        }
        // objects with edges tags are the side walls over the baskets
        if (collision.collider.CompareTag("Edges"))
        {
            rb.velocity *= slowdownFactorTop;
        }
        // objects with edges tags are the bottoms of the baskets
        if (collision.collider.CompareTag("DownSides"))
        {
            rb.velocity *= slowdownFactorBottom;
        }
        // object with Ground tag is the ground
        if (collision.collider.CompareTag("Ground"))
        {
            rb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "RightEdge" && !enteredLeftEdgeTrigger)
        {
            rb.AddForce(Vector3.up * blockerBoost, ForceMode.Impulse);
            enteredRightEdgeTrigger = true;
            if (other.gameObject.name == "RightForce1")
            {
                rb.AddForce(Vector3.up * blockerBoost, ForceMode.Impulse);
                if (player1Rb.velocity.x < 0)
                    rb.AddForce(Vector3.left * edgesBoost * Mathf.Abs(player1Rb.velocity.x * turningBySpeed), ForceMode.Impulse);
                else if (player1Rb.velocity.x >= 0)
                {
                    rb.AddForce(Vector3.left * edgesBoost, ForceMode.Impulse);
                }
            }
            if (other.gameObject.name == "RightForce2")
            {
                if (player2Rb.velocity.x < 0)
                    rb.AddForce(Vector3.left * edgesBoost * Mathf.Abs(player2Rb.velocity.x * turningBySpeed), ForceMode.Impulse);
                else if (player2Rb.velocity.x >= 0)
                {
                    rb.AddForce(Vector3.left * edgesBoost, ForceMode.Impulse);
                }
            }
            if (other.gameObject.name == "RightForceAI")
            {
                if (AIRb.velocity.x < 0)
                    rb.AddForce(Vector3.left * edgesBoost * Mathf.Abs(AIRb.velocity.x * turningBySpeed), ForceMode.Impulse);
                else if (AIRb.velocity.x >= 0)
                {
                    rb.AddForce(Vector3.left * edgesBoost, ForceMode.Impulse);
                }
            }
        }
        else if (other.tag == "LeftEdge" && !enteredRightEdgeTrigger)
        {
            rb.AddForce(Vector3.up * blockerBoost, ForceMode.Impulse);
            enteredLeftEdgeTrigger = true;
            if (other.gameObject.name == "LeftForce1")
            {
                if (player1Rb.velocity.x > 0)
                    rb.AddForce(Vector3.right * edgesBoost * Mathf.Abs(player1Rb.velocity.x * turningBySpeed), ForceMode.Impulse);
                else if (player1Rb.velocity.x <= 0)
                {
                    rb.AddForce(Vector3.right * edgesBoost, ForceMode.Impulse);
                }
            }
            if (other.gameObject.name == "LeftForce2")
            {
                if (player2Rb.velocity.x > 0)
                    rb.AddForce(Vector3.right * edgesBoost * Mathf.Abs(player2Rb.velocity.x * turningBySpeed), ForceMode.Impulse);
                else if (player2Rb.velocity.x <= 0)
                {
                    rb.AddForce(Vector3.right * edgesBoost, ForceMode.Impulse);
                }
            }
            if (other.gameObject.name == "LeftForceAI")
            {
                if (AIRb.velocity.x > 0)
                    rb.AddForce(Vector3.right * edgesBoost * Mathf.Abs(AIRb.velocity.x * turningBySpeed), ForceMode.Impulse);
                else if (AIRb.velocity.x <= 0)
                {
                    rb.AddForce(Vector3.right * edgesBoost, ForceMode.Impulse);
                }
            }

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "RightEdge")
        {
            enteredRightEdgeTrigger = false;
        }
        if (other.tag == "LeftEdge")
        {
            enteredLeftEdgeTrigger = false;
        }
    }
}
