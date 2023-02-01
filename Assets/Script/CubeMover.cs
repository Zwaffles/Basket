using UnityEngine;

public class CubeMover : MonoBehaviour
{
    private Rigidbody rb;
    public GameObject target;
    public float speed;
    private float deviation;
    [SerializeField] private float deviationMin;
    [SerializeField] private float deviationMax;
    [HideInInspector] public int AImoveDirection; // The variable to keep track of the move direction
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        deviation = Random.Range(deviationMin, deviationMax);
        rb.velocity = new Vector3((direction.x + deviation) * speed, rb.velocity.y, rb.velocity.y);
    }

    private void Update()
    {
        //Cube Move Direction
        if (rb.velocity.x > 0)
        {
            AImoveDirection = 1;
            Debug.Log("Moving Right");
        }
        else if (rb.velocity.x < 0)
        {
            AImoveDirection = -1;
            Debug.Log("Moving Left");
        }
        else
        {
            AImoveDirection = 0;
        }
    }
}
