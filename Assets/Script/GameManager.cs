using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject AI;
    [SerializeField] GameObject playerTwo;

    public void SwitchPlayers()
    {
        if (AI.activeSelf)
        {
            AI.SetActive(false);
            playerTwo.SetActive(true);
        }else if (!AI.activeSelf)
        {
            AI.SetActive(true);
            playerTwo.SetActive(false);
        }
    }
}
