using UnityEngine;
using UnityEngine.InputSystem;

[AddComponentMenu("Player/Player Controller")]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Bounds")]
    [Tooltip("The left border for player movement")]
    public float leftBorder = -5.0f;
    [Tooltip("The right border for player movement")]
    public float rightBorder = 5.0f;
    [Tooltip("The top border for player movement")]
    public float topBorder = 5.0f;
    [Tooltip("The bottom border for player movement")]
    public float bottomBorder = -5.0f;
    [Tooltip("The radius of border sphere")]
    public float radius = .5f;
    private Rigidbody rb; // Reference to the Rigidbody component
    [Header("Movement Settings")]
    [Tooltip("The speed at which the player moves")]
    public float speed; // The speed at which the player moves
    private Vector2 moveInput; // The input value for movement
    [HideInInspector]
    [Tooltip("The variable to keep track of the move direction")]
    public int moveDirection; // The variable to keep track of the move direction
    [Header("Sliding")]
    [SerializeField]
    [Range(0, 1)]
    float lerpConstant;

    [Header("Rotation Settings")]
    [SerializeField]
    float leanAngle = 5f;
    [SerializeField]
    float leanSpeed = 10f;

    Quaternion targetRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
    }

    void FixedUpdate()
    {
        Move(); // Call the Move() function
        Rotate(); // Call the Rotate() function
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>(); // Get the input value for movement
    }

    void Move()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * speed, rb.velocity.y); // Calculate the player's velocity

        // restrict movement to the left border
        if (transform.position.x + playerVelocity.x * Time.fixedDeltaTime < leftBorder && moveInput.x < 0)
        {
            float distanceToLeftBorder = leftBorder - transform.position.x;
            playerVelocity.x = distanceToLeftBorder / Time.fixedDeltaTime;
        }

        // restrict movement to the right border
        if (transform.position.x + playerVelocity.x * Time.fixedDeltaTime > rightBorder && moveInput.x > 0)
        {
            float distanceToRightBorder = rightBorder - transform.position.x;
            playerVelocity.x = distanceToRightBorder / Time.fixedDeltaTime;
        }
        //rb.velocity = playerVelocity; // Set the player's velocity
        rb.velocity = Vector2.Lerp(rb.velocity, playerVelocity, lerpConstant);
    }

    void Rotate()
    {
        if (moveInput.x > 0)
        {
            targetRotation = Quaternion.Euler(0f, 0f, -leanAngle);
        }
        else if (moveInput.x < 0)
        {
            targetRotation = Quaternion.Euler(0f, 0f, leanAngle);
        }
        else
        {
            targetRotation = Quaternion.Euler(0f, 0f, 0f);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, leanSpeed * Time.deltaTime);
    }

    private void Update()
    {
        // Cube Move Direction
        if (moveInput.x > 0)
        {
            moveDirection = 1;
        }
        else if (moveInput.x < 0)
        {
            moveDirection = -1;
        }
        else
        {
            moveDirection = 0;
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