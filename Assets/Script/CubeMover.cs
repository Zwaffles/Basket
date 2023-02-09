using System.Collections.Generic;
using UnityEngine;

public class CubeMover : MonoBehaviour
{
    private Rigidbody rb;
    public GameObject target;
    public float speed;
    public float targetDistanceThreshold;
    private Vector3 targetVelocity;
    [HideInInspector] public int moveDirection;
    private Vector3 previousPosition;
    public Vector3 targetGoalPosition;
    


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float distance = Vector3.Distance(transform.position, target.transform.position);
        targetVelocity = (target.transform.position - previousPosition) / Time.fixedDeltaTime;

        if (distance >= targetDistanceThreshold)
        {
            // move towards the target
            Vector3 direction = (target.transform.position - transform.position).normalized;
            rb.velocity = direction * speed;
        }
        else
        {
            // predict where the target will be in the future and move towards that position
            Vector3 targetFuturePosition = target.transform.position + targetVelocity * Time.fixedDeltaTime;
            Vector3 direction = (targetFuturePosition - transform.position).normalized;
            rb.velocity = direction * speed;
        }
    }

    private void Update()
    {
        // Cube move direction
        if (transform.position.x > previousPosition.x)
        {
            moveDirection = 1;
        }
        else if (transform.position.x < previousPosition.x)
        {
            moveDirection = -1;
        }
        else
        {
            moveDirection = 0;
        }
        previousPosition = transform.position;
    }
}
