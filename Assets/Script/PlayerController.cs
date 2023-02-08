using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float leftBorder = -5.0f;
    public float rightBorder = 5.0f;
    public float topBorder = 5.0f;
    public float bottomBorder = -5.0f;
    public float radius = .5f;

    private Rigidbody rb; // Reference to the Rigidbody component
    public float speed; // The speed at which the player moves
    private Vector2 moveInput; // The input value for movement
    [HideInInspector]
    public int moveDirection; // The variable to keep track of the move direction
    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
    }

    void FixedUpdate()
    {
        Move(); // Call the Move() function
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
        rb.velocity = playerVelocity; // Set the player's velocity
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

