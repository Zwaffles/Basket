using UnityEngine;

public class CubeMover : MonoBehaviour
{
    private Rigidbody rb;
    public GameObject target;
    public float speed;
    private float deviation;
    [SerializeField] private float deviationMin;
    [SerializeField] private float deviationFarTargetMin;
    [SerializeField] private float deviationFarTargetMax;
    [SerializeField] private float deviationMax;
    [HideInInspector] public int moveDirection; // The variable to keep track of the move direction
    private Vector3 previousPosition;
    public float targetDistanceThreshold;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance >= targetDistanceThreshold)
        {
            deviation = Random.Range(deviationFarTargetMin, deviationFarTargetMax);
            // when the target is far from the cube, add random deviation along x, y and z axes
            rb.velocity = new Vector3((deviation) * speed, (deviation) * speed, (deviation) * speed);
        }
        else
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            deviation = Random.Range(deviationMin, deviationMax);
            // to make movement more realistic, we add the deviation along x, y and z axes
            rb.velocity = new Vector3((direction.x + deviation) * speed, (direction.y + deviation) * speed, (direction.z + deviation) * speed);
        }
    }

    private void Update()
    {
        //Cube Move Direction
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
