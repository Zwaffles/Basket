using System.Collections.Generic;
using UnityEngine;

public class Vacuum : MonoBehaviour
{
    private Rigidbody rb;
    public float pullForce;
    public float vacuumDistance;
    ScoreManager scoreManager;
    CubeMover cubeMover;
    private void Awake()
    {
        cubeMover = FindObjectOfType<CubeMover>();
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

                    if (cubeMover.speed < 20)
                    {
                        cubeMover.speed += 1f;
                    }
                rb = null;
                }
                if(this.gameObject.name == "Vacuum2")
                {
                    scoreManager.PlayerTwoScore();

                    if (cubeMover.speed > 6)
                    {
                        cubeMover.speed -= 1f;
                    }
                rb = null;
            }
        }
    }
    // Update the average reward and adjust speed
    
}
}