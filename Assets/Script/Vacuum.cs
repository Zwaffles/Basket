using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using static UnityEngine.Rendering.DebugUI;
using Unity.VisualScripting;

public class Vacuum : MonoBehaviour
{
    [Header("Vacuum Properties")]
    [Tooltip("Force applied to the ball towards the vacuum")]
    public float pullForce;
    [Tooltip("Distance at which the ball will be sucked into the vacuum and a score will be added")]
    public float vacuumDistance;
    private Rigidbody rb;
    GameManager gameManager;
    ScoreManager scoreManager;
    [SerializeField] Collider[] blockCollider;
    [SerializeField] bool multiBallsScene = false;
    private void Start()
    {
        gameManager = GameManager.instance;
        scoreManager = GameManager.instance.scoreManager;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            rb = other.GetComponent<Rigidbody>();
            // Apply a force to the rigidbody that pulls it towards the vacuum
            rb.AddForce((transform.position - rb.transform.position).normalized * pullForce, ForceMode.Force);
            if (!multiBallsScene)
            {
                blockCollider[0].enabled = false;
                blockCollider[1].enabled = false;
                Invoke("EnableBlockColliders", 1f);
                Invoke("EnableBlockColliders", 1f);
            }
            if(multiBallsScene)
            StartCoroutine("DeactivateBall", other.gameObject);
        }
    }
    IEnumerator DeactivateBall(GameObject ball)
    {
        yield return new WaitForSeconds(0.1f);
        ball.SetActive(false);
        if (rb != null)
        {
                if (this.gameObject.name == "Vacuum1")
                {

                    scoreManager.PlayerOneScore(1);
                    //gameManager.RespawnBall(1);
                    rb = null;

                }

                if (this.gameObject.name == "Vacuum2")
                {

                    scoreManager.PlayerTwoScore(1);
                    //gameManager.RespawnBall(-1);
                    rb = null;
            }
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
                    
                    scoreManager.PlayerOneScore(1);
                    gameManager.RespawnBall(1);
                    rb = null;
                    
                }

                if (this.gameObject.name == "Vacuum2")
                {
                    
                    scoreManager.PlayerTwoScore(1);
                    gameManager.RespawnBall(-1);
                    rb = null;
                    
                }
            }
        }
    }
    void EnableBlockColliders()
    {
        blockCollider[0].enabled = true;
        blockCollider[1].enabled = true;
    }
    private void ResetShakeFrequency()
    {
        gameManager.shakeFrequency -= 1.7f;
    }
}