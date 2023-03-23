using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MultiplayerHandler : MonoBehaviour
{

    private PlayerInput playerInput;

    private void Awake()
    {

        //this.transform.parent = GameObject.Find("PlayerManager").transform;

        playerInput = GetComponent<PlayerInput>();
        int index = playerInput.playerIndex;

        switch (index)
        {

            case 1:
                GameObject.Find("Player 1").transform.parent = this.transform;
                break;
            case 2:
                GameObject.Find("Player 2").transform.parent = this.transform;
                break;
            default:
                break;

        }

    }

}
