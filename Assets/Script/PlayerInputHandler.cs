using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerConfiguration playerConfiguration;
    private PlayerController player;

    [SerializeField]
    private MeshRenderer playerMesh;

    private Inputaction controls;

    [SerializeField]
    private Light playerLight;

    private void Awake()
    {
        player = GetComponent<PlayerController>();
        controls = new Inputaction();
    }

    public void InitializePlayer(PlayerConfiguration _playerConfiguration)
    {
        playerConfiguration = _playerConfiguration;
        playerMesh.material = _playerConfiguration.PlayerMaterial;
        playerLight.color = _playerConfiguration.PlayerColor;
        //Debug.Log("setting up player " + playerConfiguration.PlayerIndex + " using the input scheme " + playerConfiguration.Input.currentControlScheme + " on the action map " + playerConfiguration.Input.currentActionMap + " with the device " + playerConfiguration.Input.devices[0].name);
        playerConfiguration.Input.onActionTriggered += Input_onActionTriggered;
    }

    private void Input_onActionTriggered(CallbackContext obj)
    {
       // Debug.Log("is this getting triggered? the objects action name is " + obj.action.name + "and the controllers action name is" + controls.Player.Move.name);

        if(obj.action.name == controls.Player.Move.name)
        {
           /// Debug.Log("i should be moving");
            OnMove(obj);
        }
    }

    private void OnMove(CallbackContext context)
    {
        if (player == null)
            return;

       // Debug.Log("literally moving rn because my movement value is " + context.ReadValue<Vector2>());
        player.SetInputVector(context.ReadValue<Vector2>());
    }
}
