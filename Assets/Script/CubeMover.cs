using System.Collections;
using UnityEngine;

/// <summary>
/// This script moves a cube towards a target game object while considering the velocity of the target.
/// </summary>
public class CubeMover : MonoBehaviour
{
    private Rigidbody rb;
    /// <summary>
    /// The target game object to move towards.
    /// </summary>
    [Tooltip("The target that the cube will follow")]
    public GameObject target;

    /// <summary>
    /// The speed at which the cube moves towards the target.
    /// </summary>
    [Tooltip("The speed at which the cube moves towards the target.")]
    public float speed;

    /// <summary>
    /// The minimum distance from the target that the cube will start predicting its movement.
    /// </summary>
    [Tooltip("The minimum distance from the target that the cube will start predicting its movement.")]
    public float targetDistanceThreshold;

    private Vector3 targetVelocity;

    /// <summary>
    /// The direction the cube is moving in.
    /// 1 is right, -1 is left, 0 is not moving in the X axis.
    /// </summary>
    [HideInInspector] public int moveDirection;

    private Vector3 previousPosition;

    [Header("Rotation Settings")]
    [SerializeField]
    float leanAngle = 5f;
    [SerializeField]
    float leanSpeed = 10f;

    public float targetXPosMin = 6f;
    public float targetXPosMax = 20f;
    public float waitTime = .5f;

    Quaternion targetRotation;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    [SerializeField]
    float timer = 5;
    //bool hasLeft = false;
    void Update()
    {
        if (transform.position.x < 1.5f)
        {
            transform.position = new Vector3 (1.5f,transform.position.y,transform.position.z);
        }
        float distance = Vector3.Distance(transform.position , target.transform.position);
        targetVelocity = (target.transform.position - previousPosition) / Time.fixedDeltaTime;

        if (distance >= targetDistanceThreshold)
        {
            // move towards the target
            Vector3 direction = ((target.transform.position + new Vector3(1, 0, 0)) - transform.position).normalized;
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
    void Rotate()
    {
        if (moveDirection > 0)
        {
            targetRotation = Quaternion.Euler(0f, 0f, -leanAngle);
        }
        else if (moveDirection < 0)
        {
            targetRotation = Quaternion.Euler(0f, 0f, leanAngle);
        }
        else
        {
            targetRotation = Quaternion.Euler(0f, 0f, 0f);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, leanSpeed * Time.deltaTime);
    }
    private void FixedUpdate()
    {
        Rotate();
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
