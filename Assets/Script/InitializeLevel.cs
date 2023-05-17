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


    public void Start()
    {
        Destroy(previewPlayer);

        var playerConfigurations = GameManager.instance.playerConfigurationManager.GetPlayerConfigurations().ToArray();
        for(int i = 0; i < playerConfigurations.Length; i++)
        {
            var player = Instantiate(playerPrefab, playerSpawns[i].position, playerSpawns[i].rotation, gameObject.transform);
            player.GetComponent<PlayerInputHandler>().InitializePlayer(playerConfigurations[i]);
        }

        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {


        Time.timeScale = 0f; // Pause the game by setting the time scale to 0

        try
        {
            // New
            GameManager.instance.audioManager.PlayVoice("Ready_-Noel_-Deep2");
        }
        catch
        {
            Debug.LogWarning("AudioManager not found. Perhaps you're not using the GameManager prefab?");
        }

        yield return new WaitForSecondsRealtime(.25f); // Wait for 1 second before starting the countdown

        for (currentCountdown = 3; currentCountdown >= 0; currentCountdown--)
        {
            countdownText.text = currentCountdown == 0 ? "GO!" : currentCountdown.ToString(); // Update the UI text element

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
                    GameManager.instance.audioManager.PlayVoice("Go_-_Noel_-_Deep");
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
            GameManager.instance.audioManager.PlayMusic("Vonboff_-_Hit_the_bits");
        }
        catch
        {
            Debug.LogWarning("AudioManager not found. Perhaps you're not using the GameManager prefab?");
        }

    }
}
