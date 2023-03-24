using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SwitchPlayers : MonoBehaviour
{
    [SerializeField] GameObject AI;
    [SerializeField] GameObject playerTwo;
    [SerializeField] TextMeshProUGUI score;

    public void SwitchPlayer()
    {
        if (AI.activeSelf)
        {
            AI.SetActive(false);
            playerTwo.SetActive(true);
            score.color = Color.blue;
        }
        else if (!AI.activeSelf)
        {
            AI.SetActive(true);
            playerTwo.SetActive(false);
            score.color = Color.red;
        }
    }
}
