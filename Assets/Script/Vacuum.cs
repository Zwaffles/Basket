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
    CubeMover cubeMover;
    GameManager gameManager;
    BouncingBall bouncingBall;
    

    private void Awake()
    {
        bouncingBall = FindObjectOfType<BouncingBall>();
        gameManager = FindObjectOfType<GameManager>();
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

        //if (this.gameObject.tag == "Vacuum1")
        //{
        //    rb = other.GetComponent<Rigidbody>();
        //    rb.AddForce(Vector3.right * pullForce, ForceMode.Impulse);
        //}
        //if (this.gameObject.tag == "Vacuum2")
        //{
        //    rb = other.GetComponent<Rigidbody>();
        //    rb.AddForce(Vector3.left * pullForce, ForceMode.Impulse);
        //}
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
                    if (!bouncingBall.fireBallSmoke.isPlaying)
                    {
                        scoreManager.PlayerOneScore(1);
                    }
                        
                    else if (bouncingBall.fireBallSmoke.isPlaying)
                    { 
                        scoreManager.PlayerOneScore(2);
                        scoreManager.ShowDoublePointsText();
                        gameManager.shakeFrequency += 1.7f;
                        Invoke("ResetShakeFrequency", 1);
                    }
                    gameManager.RespawnBall(1);
                    rb = null;
                }

                if(this.gameObject.name == "Vacuum2")
                {                   
                    if (!bouncingBall.fireBallSmoke.isPlaying)
                    {
                        scoreManager.PlayerTwoScore(1);
                    }
                        
                    else if (bouncingBall.fireBallSmoke.isPlaying)
                    {
                        scoreManager.PlayerTwoScore(2);
                        scoreManager.ShowDoublePointsText();
                        gameManager.shakeFrequency += 1.7f;
                        Invoke("ResetShakeFrequency", 1);
                    }
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