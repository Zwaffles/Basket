using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InitializeLevel : MonoBehaviour
{
    [SerializeField]
    private Transform[] playerSpawns;
    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField, Tooltip("In case you have a little player for preview, you can destroy it here")]
    private GameObject previewPlayer;

    public float countdownDuration = 3f; // Duration of each countdown step
    public TextMeshProUGUI countdownText; // Reference to the UI text element to display the countdown

    private int currentCountdown; // Current countdown number

    private GameManager gameManager;

    public void Start()
    {
        gameManager = GameManager.instance;

        Destroy(previewPlayer);

        var playerConfigurations = gameManager.playerConfigurationManager.GetPlayerConfigurations().ToArray();
        for(int i = 0; i < playerConfigurations.Length; i++)
        {
            if (gameManager.player1Left)
            {
                var player = Instantiate(playerPrefab, playerSpawns[i].position, playerSpawns[i].rotation, gameObject.transform);
                player.GetComponent<PlayerInputHandler>().InitializePlayer(playerConfigurations[i]);
            }
            else
            {
                var player = Instantiate(playerPrefab, playerSpawns[(i + 1) % 2].position, playerSpawns[(i + 1) % 2].rotation, gameObject.transform);
                player.GetComponent<PlayerInputHandler>().InitializePlayer(playerConfigurations[i]);
            }
        }

        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {

        string goString = "GO!";

        if (PlayerPrefs.GetString("selected-locale") != null)
        {
            switch (PlayerPrefs.GetString("selected-locale"))
            {
                case "en":
                    goString = "GO!";
                    break;
                case "fr":
                    goString = "Aller!";
                    break;
                case "de":
                    goString = "Los!";
                    break;
                case "haw":
                    goString = "E hele!";
                    break;
                case "it":
                    goString = "Vai!";
                    break;
                case "pl":
                    goString = "Start!";
                    break;
                case "pt-BR":
                    goString = "Ir!";
                    break;
                case "ru":
                    goString = "Вперёд!";
                    break;
                case "es":
                    goString = "¡Ir!";
                    break;
                case "tr":
                    goString = "Git!";
                    break;
                case "uk":
                    goString = "Почати!";
                    break;
                case "zh-Hans":
                    goString = "GO!";
                    break;
                case "ja":
                    goString = "GO!";
                    break;
                default:
                    goString = "GO!";
                    break;
            }
        }

        Time.timeScale = 0f; // Pause the game by setting the time scale to 0

        try
        {
            // New
            gameManager.audioManager.PlayVoice("Robot3_Are_you_ready");
            gameManager.audioManager.StopMusic();
        }
        catch
        {
            Debug.LogWarning("AudioManager not found. Perhaps you're not using the GameManager prefab?");
        }

        yield return new WaitForSecondsRealtime(.25f); // Wait for 1 second before starting the countdown

        for (currentCountdown = 3; currentCountdown >= 0; currentCountdown--)
        {
            countdownText.text = currentCountdown == 0 ? goString : currentCountdown.ToString(); // Update the UI text element

            yield return new WaitForSecondsRealtime(countdownDuration / 4);

            if (currentCountdown == 2)
            {
                try
                {
                    // New
                    //GameManager.instance.audioManager.PlayVoice("Set_-Noel_-Deep");

                }
                catch
                {
                    Debug.LogWarning("AudioManager not found. Perhaps you're not using the GameManager prefab?");
                }
            }

            if (currentCountdown == 1)
            {
                Time.timeScale = 1f; // Resume the game by setting the time scale to 1
                try
                {
                    // New
                    gameManager.audioManager.PlayVoice("Robot3_Go");
                    // Alt
                    //GameManager.instance.audioManager.PlayVoice("Start_-_Noel_-_Deep");

                }
                catch
                {
                    Debug.LogWarning("AudioManager not found. Perhaps you're not using the GameManager prefab?");
                }
            }

            if (currentCountdown == 0)
            {
                countdownText.text = ""; // Clear the UI text element
            }
        }

        yield return new WaitForSecondsRealtime(.25f); // Wait for 1 second before starting the countdown

        try
        {
            gameManager.audioManager.PlayMusic("Vonboff_-_Hit_the_bits");
        }
        catch
        {
            Debug.LogWarning("AudioManager not found. Perhaps you're not using the GameManager prefab?");
        }

    }
}
