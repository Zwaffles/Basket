using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSetupMenuController : MonoBehaviour
{
    private int playerIndex;

    [SerializeField]
    private TextMeshProUGUI titleText;
    [SerializeField]
    private GameObject readyPanel;
    [SerializeField]
    private GameObject menuPanel;
    [SerializeField]
    private Button readyButton;

    private float ignoreInputTime = .5f;
    private bool inputEnabled;

    private PlayerConfigurationManager playerConfigurationManager;

    private void OnEnable()
    {
        playerConfigurationManager = GameManager.instance.playerConfigurationManager;
    }

    public void SetPlayerIndex(int _playerIndex)
    {
        playerIndex = _playerIndex;
        titleText.SetText("Player " + (playerIndex + 1).ToString());
        ignoreInputTime = Time.time + ignoreInputTime;
    }

    private void Update()
    {
        if(Time.time > ignoreInputTime)
        {
            inputEnabled = true;
        }
    }

    public void SetColor(Material color)
    {
        if (!inputEnabled) { return; }

        playerConfigurationManager.SetPlayerColor(playerIndex, color);
        readyPanel.SetActive(true);
        readyButton.Select();
        menuPanel.SetActive(false);
    }

    public void ReadyPlayer()
    {
        if(!inputEnabled) { return; }

        playerConfigurationManager.ReadyPlayer(playerIndex);
        readyButton.gameObject.SetActive(false);
    }
}
