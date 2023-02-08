using UnityEngine;

public class Vacuum : MonoBehaviour
{
    private Rigidbody rb;
    public float pullForce;
    public float vacuumDistance;
    ScoreManager scoreManager;
    private void Awake()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            rb = other.GetComponent<Rigidbody>();
            // Apply a force to the rigidbody that pulls it towards the vacuum
            rb.AddForce((transform.position - rb.transform.position).normalized * pullForce, ForceMode.Force);
        }
    }

    private void FixedUpdate()
    {
        if (rb != null)
        {
            float distance = Vector3.Distance(transform.position, rb.transform.position);
            if (distance < vacuumDistance)
            {
                if(this.gameObject.name == "Vacuum1")
                {
                    scoreManager.PlayerOneScore();
                }
                if(this.gameObject.name == "Vacuum2")
                {
                    scoreManager.PlayerTwoScore();
                }
                rb = null;
            }
        }
    }
}