using UnityEngine;
using UnityEngine.InputSystem;

[AddComponentMenu("Player/Player Controller")]
[RequireComponent(typeof(Rigidbody))]
public class Player2Controller : MonoBehaviour
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

    private Rigidbody rb;
    [Header("Movement Settings")]
    [Tooltip("For acceleration")]
    public float speedAcceleration;
    [Tooltip("For View Only")]
    [SerializeField] float step = 0;
    [Tooltip("The speed at which the player moves")]
    [SerializeField] float speed = 0;
    private Vector2 moveInput; // The input value for movement
    [Tooltip("The variable to keep track of the move direction")]
    [HideInInspector]
    public int player2MoveDirection; // The variable to keep track of the move direction
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
        rb = GetComponent<Rigidbody>();
        this.gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        Move();
        Rotate();
    }

    void OnMove2(InputValue value)
    {
        moveInput = value.Get<Vector2>(); // Get the input value for movement
    }

    void Move()
    {
        // Calculate the player's velocity
        Vector2 playerVelocity = new Vector2(moveInput.x * step, rb.velocity.y);

        // restrict movement to the left border
        if (transform.position.x + playerVelocity.x * Time.fixedDeltaTime < leftBorder && moveInput.x < 0)
        {
            step = speed;
            float distanceToLeftBorder = leftBorder - transform.position.x;
            playerVelocity.x = distanceToLeftBorder / Time.fixedDeltaTime;
        }

        // restrict movement to the right border
        if (transform.position.x + playerVelocity.x * Time.fixedDeltaTime > rightBorder && moveInput.x > 0)
        {
            step = speed;
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
        step += speedAcceleration * (speed + (Time.deltaTime / speed));
        if (moveInput.x == 0)
            step = speed;

        // Cube Move Direction
        if (moveInput.x > 0)
        {
            player2MoveDirection = 1;
        }
        else if (moveInput.x < 0)
        {
            player2MoveDirection = -1;
        }
        else
        {
            player2MoveDirection = 0;
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
