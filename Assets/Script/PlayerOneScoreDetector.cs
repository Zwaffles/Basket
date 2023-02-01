using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerOneScoreDetector : MonoBehaviour
{
    ScoreManager scoreManager;
    TextMeshPro player1Score;

    void Awake()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        scoreManager.PlayerOneScore();
    }
}
