using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timerText;

    private float timeLeft = 5.0f;
    private bool countingDown = true;

    private void OnEnable()
    {
        countingDown = true;
    }

    private void Update()
    {
        if (countingDown)
        {
            timeLeft -= Time.deltaTime;
            timerText.text = "Press again to confirm in " + Mathf.Round(timeLeft).ToString() + "s";

            if (timeLeft <= 0)
            {
                timeLeft = 5.0f;
                countingDown = false;
                gameObject.SetActive(false);
            }
        }
    }

    public void RestartScene()
    {
        GameManager.instance.uiManager.ToggleScore(true);

        if (GameManager.instance.playerConfigurationManager.GetPlayerConfigurations().Count > 1)
            GameManager.instance.InitializeScene(SceneManager.GetActiveScene().name, isMultiplayer: true);
        else
            GameManager.instance.InitializeScene(SceneManager.GetActiveScene().name, isMultiplayer: false);

        timeLeft = 5.0f;
        countingDown = false;
        gameObject.SetActive(false);
    }
}
