using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject AI;
    [SerializeField] GameObject playerTwo;
    [SerializeField] TextMeshProUGUI score;
    [SerializeField] GameObject ball;
    [SerializeField] Transform ballRightSpawn;
    [SerializeField] Transform ballLeftSpawn;
    Rigidbody ballRigidbody;
    private void Start()
    {
        ballRigidbody = ball.GetComponent<Rigidbody>();
        playerTwo.SetActive(false);
    }

    public void SwitchPlayers()
    {
        if (AI.activeSelf)
        {
            AI.SetActive(false);
            playerTwo.SetActive(true);
            score.color = Color.blue;
        }else if (!AI.activeSelf)
        {
            AI.SetActive(true);
            playerTwo.SetActive(false);
            score.color = Color.red;
        }
    }

    public void RespawnBall(int value)
    {
        StartCoroutine("Respawn", value);
    }
    IEnumerator Respawn(int rightorLeft)
    {
        yield return new WaitForSeconds(.4f);
        ball.SetActive(false);
        ballRigidbody.velocity = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(.2f);
        if (rightorLeft == 1)
        {
            ball.transform.position = ballRightSpawn.position;
            ball.SetActive(true);
        }
        else if(rightorLeft == -1)
        {
            ball.transform.position = ballLeftSpawn.position;
            ball.SetActive(true);
        }
    }
}
