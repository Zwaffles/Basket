using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTwoScoreDetector : MonoBehaviour
{
    ScoreManager scoreManager;

    void Awake()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        scoreManager.PlayerTwoScore();
    }
}
