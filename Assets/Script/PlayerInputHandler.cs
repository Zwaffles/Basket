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

    private void Awake()
    {
        player = GetComponent<PlayerController>();
        controls = new Inputaction();
    }

    public void InitializePlayer(PlayerConfiguration _playerConfiguration)
    {
        playerConfiguration = _playerConfiguration;
        playerMesh.material = _playerConfiguration.PlayerMaterial;
        playerConfiguration.Input.onActionTriggered += Input_onActionTriggered;
    }

    private void Input_onActionTriggered(CallbackContext obj)
    {
        if(obj.action.name == controls.Player.Move.name)
        {
            OnMove(obj);
        }
    }

    private void OnMove(CallbackContext context)
    {
        if (player != null)
            player.SetInputVector(context.ReadValue<Vector2>());
    }
}
