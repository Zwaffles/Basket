using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private Button resumeButton;

    private void OnEnable()
    {
        resumeButton.Select();
    }

    public void Resume()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Restart()
    {
        GameManager.instance.uiManager.ToggleScore(true);

        if (GameManager.instance.playerConfigurationManager.GetPlayerConfigurations().Count > 1)
            GameManager.instance.InitializeScene(SceneManager.GetActiveScene().name, isMultiplayer: true);
        else
            GameManager.instance.InitializeScene(SceneManager.GetActiveScene().name, isMultiplayer: false);

        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void LeaveMatch()
    {
        GameManager.instance.audioManager.StopMusic();
        gameObject.SetActive(false);
        Time.timeScale = 1f;
        GameManager.instance.StartMenu();
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
