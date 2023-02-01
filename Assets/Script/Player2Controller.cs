using UnityEngine;
using UnityEngine.InputSystem;

public class Player2Controller : MonoBehaviour
{
    private Rigidbody rb; // Reference to the Rigidbody component
    public float speed; // The speed at which the player moves
    private Vector2 moveInput; // The input value for movement
    [HideInInspector]
    public int player2MoveDirection; // The variable to keep track of the move direction
    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
    }

    void FixedUpdate()
    {
        Move(); // Call the Move() function
    }

    void OnMoveSecondPlayer(InputValue value)
    {
        moveInput = value.Get<Vector2>(); // Get the input value for movement
    }

    void Move()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * speed, rb.velocity.y); // Calculate the player's velocity
        rb.velocity = playerVelocity; // Set the player's velocity
    }
    private void Update()
    {
        //Cube Move Direction
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

}
