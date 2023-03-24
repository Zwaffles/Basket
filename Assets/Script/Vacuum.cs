using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Vacuum : MonoBehaviour
{
    [Header("Vacuum Properties")]
    [Tooltip("Force applied to the ball towards the vacuum")]
    public float pullForce;
    [Tooltip("Distance at which the ball will be sucked into the vacuum and a score will be added")]
    public float vacuumDistance;
    private Rigidbody rb;
    ScoreManager scoreManager;
    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance;
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
                if (this.gameObject.name == "Vacuum1")
                {

                    scoreManager.PlayerOneScore(2);

                    gameManager.RespawnBall(1);
                    rb = null;
                }

                if (this.gameObject.name == "Vacuum2")
                {
                    scoreManager.PlayerTwoScore(2);
                    gameManager.RespawnBall(-1);
                    rb = null;
                }
            }
        }
    }
    private void ResetShakeFrequency()
    {
        gameManager.shakeFrequency -= 1.7f;
    }
}