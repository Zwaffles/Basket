using System;
using System.Collections;
using UnityEngine;

public class AIController : MonoBehaviour
{
    // Bouncing Ball targets
    public GameObject[] balls;

    // Movement speed of the AI object
    public float movementSpeed = 5.0f;

    // The currently targeted ball
    private GameObject targetBall;

    // Counter for switching between balls
    private int counter = 0;

    // Number of frames to wait before switching targets
    public int switchInterval = 60;

    public int moveDirection; // The variable to keep track of the move direction

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        // Set the initial target ball
        targetBall = balls[0];
    }

    void Update()
    {
        // Increment the counter
        counter++;

        // If the counter has reached the switch interval, reset it and switch targets
        if (counter >= switchInterval)
        {
            counter = 0;
            // Switch the target ball
            int targetIndex = (Array.IndexOf(balls, targetBall) + 1) % balls.Length;
            targetBall = balls[targetIndex];
        }

        // Calculate the direction to move towards the targeted ball
        Vector3 direction = targetBall.transform.position - transform.position;
        direction.y = 0; // remove the vertical component
        direction.Normalize();

        // Move towards the targeted ball
        transform.position = Vector3.MoveTowards(transform.position, targetBall.transform.position, Time.deltaTime * movementSpeed);

        //Cube Move Direction
        if (rb.velocity.x > 0)
        {
            moveDirection = 1;
            Debug.Log("Moving Right");
        }
        else if (rb.velocity.x < 0)
        {
             moveDirection = -1;
            Debug.Log("Moving Left");
        }
        else
        {
            moveDirection = 0;
        }
    }
}
