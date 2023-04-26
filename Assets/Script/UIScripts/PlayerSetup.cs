using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class PlayerSetup : MonoBehaviour
{
    public PlayerInput input;

    private void Awake()
    {
        GameManager.instance.uiManager.AddPlayerIndex(input.playerIndex);
        //input.onActionTriggered
    }
}
