using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerConfigurationManager : MonoBehaviour
{
    private List<PlayerConfiguration> playerConfigurations;

    [SerializeField]
    private int maxPlayers = 2;

    private void Awake()
    {
        playerConfigurations = new List<PlayerConfiguration>();
    }

    public List<PlayerConfiguration> GetPlayerConfigurations()
    {
        return playerConfigurations;
    }

    public void SetPlayerColor(int index, Material color)
    {
        playerConfigurations[index].PlayerMaterial = color;
    }

    public void ReadyPlayer(int index)
    {
        playerConfigurations[index].IsReady = true;
        if(playerConfigurations.Count == maxPlayers && playerConfigurations.All(p => p.IsReady == true))
        {
            GameManager.instance.InitializeScene("Multiplayer", isMultiplayer: true);
        }
    }

    public void HandlePlayerJoin(PlayerInput playerInput)
    {
        Debug.Log("Player Joined " + playerInput.playerIndex);
        if(!playerConfigurations.Any(p => p.PlayerIndex == playerInput.playerIndex))
        {
            playerInput.transform.SetParent(transform);
            playerConfigurations.Add(new PlayerConfiguration(playerInput));
        }
    }

    public void AllowJoining(bool condition)
    {
        if(condition)
            GetComponent<PlayerInputManager>().EnableJoining();
        else
            GetComponent<PlayerInputManager>().DisableJoining();

    }
}

public class PlayerConfiguration
{
    public PlayerConfiguration(PlayerInput playerInput)
    {
        PlayerIndex = playerInput.playerIndex;
        Input = playerInput;
    }

    public PlayerInput Input { get; set; }
    public Material PlayerMaterial { get; set; }
    public int PlayerIndex { get; set; }
    public bool IsReady { get; set; }
}
