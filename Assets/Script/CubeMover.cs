using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// This script moves a cube towards a target game object while considering the velocity of the target.
/// Taher comment: The Bot movement needs a lot of tweaking
/// </summary>
public class CubeMover : MonoBehaviour
{
    private Rigidbody rb;
    [Tooltip("The target that the cube will follow")]
    public GameObject target;
    [Tooltip("The speed at which the cube moves towards the target.")]
    public float speed;
    [Tooltip("The direction the cube is moving in, 1 is right, -1 is left, 0 is not moving in the X axis")]
    [HideInInspector] public int moveDirection;

    private Vector3 previousPosition;

    [Header("Rotation Settings")]
    [SerializeField]
    public float leanAngle = 5f;
    [SerializeField]
    public float leanSpeed = 10f;
    [SerializeField] float randomRangeFloat = .3f;
    [SerializeField] public float ballChasing = 0.5f;
    Quaternion targetRotation;

    [SerializeField] bool multiBalls = false;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    bool collidingWithBall = false;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            collidingWithBall = true;
            Invoke("ChangeAIChase", 1.4f);

        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            collidingWithBall = false;
        }
    }
    void ChangeAIChase()
    {
        if (!collidingWithBall)
            randomRangeFloat = Random.Range(-ballChasing, +ballChasing);
    }
    void Update()
    {
        if(multiBalls)
        SwitchTarget();

        if (target != null)
        {
            Vector3 direction = ((target.transform.position) - transform.position + new Vector3(randomRangeFloat, 0, 0)).normalized;
            rb.velocity = direction * speed;
        }    
    }
    void SwitchTarget()
    {
        if (target == null || !target.activeSelf)
        {
            target = FindObjectOfType<BouncingBall>().gameObject;
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
