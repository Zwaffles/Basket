using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class ScoreManager : MonoBehaviour
{
    private VisualElement root;
    private TextElement player1ScoreText;
    private TextElement player2ScoreText;
    private TextElement uiTimer;
    [SerializeField] GameObject winnerText;

    private float timeSpent = 500;
    private bool isRunning = false;

    public int player1Score;
    public int player2Score;

    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        player1ScoreText = root.Q<TextElement>("UI-ScoreLeft-Text");
        player2ScoreText = root.Q<TextElement>("UI-ScoreRight-Text");
        uiTimer = root.Q<TextElement>("UI-Time-Text");
    }

    private void Update()
    {
        if (isRunning)
        {
            timeSpent += Time.deltaTime;
            UpdateTimerUI(timeSpent);
        }

        if(timeSpent >= 200)
        {
            if(winnerText != null)
            {
                
                if(player1Score > player2Score)
                {
                    winnerText.SetActive(true);
                    winnerText.GetComponent<TextMeshPro>().text = "Congratulations " + "Player One"; 
                }
                if (player1Score < player2Score)
                    {
                        winnerText.SetActive(true);
                        winnerText.GetComponent<TextMeshProUGUI>().text = "Congratulations " + "Player Two";
                    }
                else if(player1Score == player2Score)
                {
                    winnerText.SetActive(true);
                    winnerText.GetComponent<TextMeshProUGUI>().text = "Draw";
                }
            }
        }      
    }
public void UpdateTimerUI(float timeSpent)
    {
        if (!gameObject.activeInHierarchy)
            return;

        uiTimer.text = DisplayTime(timeSpent);
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        timeSpent = 0f;
        UpdateTimerUI(timeSpent);
    }

    public void PlayerOneScore(int value)
    {
        try
        {
            GameManager.instance.audioManager.PlaySfx("Goal_ClapClapClap", Random.Range(0.94f, 1.24f));

            float randValue = Random.value;
            if(randValue < .54f)
                GameManager.instance.audioManager.PlaySfx("airhorn", Random.Range(0.92f, 1.18f));
        }
        catch
        {
            Debug.LogWarning("AudioManager not found. Perhaps you're not using the GameManager prefab?");
        }

        player1Score += value;

        player1ScoreText.text = player1Score.ToString();

        //add condition for player win/loss ?
    }

    public void PlayerTwoScore(int value)
    {
        try
        {
            GameManager.instance.audioManager.PlaySfx("Goal_ClapClapClap", Random.Range(0.94f, 1.24f));

            float randValue = Random.value;
            if (randValue < .54f)
                GameManager.instance.audioManager.PlaySfx("airhorn", Random.Range(0.62f, 0.92f));       
        }
        catch
        {
            Debug.LogWarning("AudioManager not found. Perhaps you're not using the GameManager prefab?");
        }

        player2Score += value;

        player2ScoreText.text = player2Score.ToString();

        //add condition for player win/loss ?
    }

    // Converts a float "time in seconds" to a 00:00 formatted string
    private string DisplayTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        return $"{minutes:00}:{seconds:00}";
    }
}
