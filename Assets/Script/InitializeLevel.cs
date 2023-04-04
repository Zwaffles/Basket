using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeLevel : MonoBehaviour
{
    [SerializeField]
    private Transform[] playerSpawns;
    [SerializeField]
    private GameObject playerPrefab;

    void Start()
    {
        var playerConfigurations = GameManager.instance.playerConfigurationManager.GetPlayerConfigurations().ToArray();
        for(int i = 0; i < playerConfigurations.Length; i++)
        {
            var player = Instantiate(playerPrefab, playerSpawns[i].position, playerSpawns[i].rotation, gameObject.transform);
            player.GetComponent<PlayerInputHandler>().InitializePlayer(playerConfigurations[i]);
        }
    }
}
