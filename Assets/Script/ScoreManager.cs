using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEditor.Rendering.LookDev;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    BouncingBall bouncingBall;

    private VisualElement root;
    private TextElement player1ScoreText;
    private TextElement player2ScoreText;
    private TextElement uiTimer;
    [SerializeField] TextMeshProUGUI winnerText;
    [SerializeField] int matchTimeBeforeHittingTheScoreTarget = 600; // counted by seconnds
    [SerializeField] int maxMatchTime = 900; // counted by seconds
    [SerializeField] int scoreTarget = 10;
    [SerializeField] InputField playerOneName;
    [SerializeField] InputField playerTwoName;
    [SerializeField] float timeSpent = 0;
    [SerializeField] GameObject fireWork;
    [SerializeField] GameObject extraTimeObject;
    private bool isRunning = false;

    public int player1Score;
    public int player2Score;

    float currentBlockerBoost;
    float currentMaxSpeed;
    float currentEdgesBoost;
    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        DontDestroyOnLoad(winnerText);
        player1ScoreText = root.Q<TextElement>("UI-ScoreLeft-Text");
        player2ScoreText = root.Q<TextElement>("UI-ScoreRight-Text");
        uiTimer = root.Q<TextElement>("UI-Time-Text");
        bouncingBall = FindObjectOfType<BouncingBall>();
        timeSpent = 0.1f;
        
    }
    private void Update()
    {
        if (isRunning)
        {
            timeSpent += Time.deltaTime;
            UpdateTimerUI(timeSpent);
        }
        if (timeSpent != 0)
        {
            winCondition();
        }

    }
    bool weHaveAWinner = false;
    void winCondition()
    {
        // timespent is counted by seconds >> 600 seconds = 10 minutes
        if (winnerText != null && timeSpent <= matchTimeBeforeHittingTheScoreTarget && timeSpent < maxMatchTime)
        {
            if (player1Score > scoreTarget && player1Score > player2Score + 1 && !weHaveAWinner)
            {
                winnerText.text = "Congratulations " + playerOneName.text;
                GetMatchOverRules();
                weHaveAWinner = true;
            }
            if (player2Score > scoreTarget && player2Score > player1Score + 1 && !weHaveAWinner)
            {
                winnerText.text = "Congratulations " + playerTwoName.text;
                GetMatchOverRules();
                weHaveAWinner = true;
            }
            else if(timeSpent >= matchTimeBeforeHittingTheScoreTarget)
            {
                extraTimeObject.SetActive(true);
            }
        }
        if (winnerText != null && timeSpent > matchTimeBeforeHittingTheScoreTarget && timeSpent < maxMatchTime)
        {
            if (player1Score > player2Score + 1 && !weHaveAWinner)
            {
                winnerText.text = "Congratulations " + playerOneName.text;
                weHaveAWinner = true;
                GetMatchOverRules();
            }
            if (player2Score > player1Score + 1 && !weHaveAWinner)
            {
                winnerText.text = "Congratulations " + playerTwoName.text;
                weHaveAWinner = true;
                GetMatchOverRules();
            }
            else if (timeSpent >= matchTimeBeforeHittingTheScoreTarget)
            {
                extraTimeObject.SetActive(true);
            }
        }
        if (winnerText != null && timeSpent >= maxMatchTime)
        {
            if (player1Score > player2Score + 1 && !weHaveAWinner)
            {
                winnerText.text = "Congratulations " + playerOneName.text;
                weHaveAWinner = true;
                GetMatchOverRules();
            }
            if (player2Score > player1Score + 1 && !weHaveAWinner)
            {
                winnerText.text = "Congratulations " + playerTwoName.text;
                weHaveAWinner = true;
                GetMatchOverRules();
            }
            else if (!weHaveAWinner)
            {
                winnerText.text = "Draw";
                weHaveAWinner = true;
                GetMatchOverRules();
            }
        }

    }
    //void SetCurrentRules()
    //{
    //    bouncingBall = FindObjectOfType<BouncingBall>();
    //    currentBlockerBoost = bouncingBall.blockerBoost;
    //    currentMaxSpeed = bouncingBall.maxSpeed;
    //    currentEdgesBoost = bouncingBall.edgesBoost;
    //}
    void GetMatchOverRules()
    {
        Invoke("ResetRules", 7);
        bouncingBall = FindObjectOfType<BouncingBall>();
        bouncingBall.blockerBoost = 9;
        bouncingBall.maxSpeed = 12;
        bouncingBall.edgesBoost = 6;
        fireWork.SetActive(true);
        // This should be linked to later to the escape button

    }
    private void ResetRules()
    {
        Debug.Log("ResetRules");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //bouncingBall.rulesReset = true;
        fireWork.SetActive(false);
        timeSpent = 0;
        weHaveAWinner = false;
        winnerText.text = "";
        //bouncingBall.blockerBoost = currentBlockerBoost;
        //bouncingBall.maxSpeed = currentMaxSpeed;
        //bouncingBall.edgesBoost = currentEdgesBoost;

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

    public void ResetScore()
    {
        player1Score = 0;
        player2Score = 0;

        player1ScoreText.text = "00";
        player2ScoreText.text = "00";
    }

    public void PlayerOneScore(int value)
    {
        try
        {
            GameManager.instance.audioManager.PlaySfx("Goal_ClapClapClap", Random.Range(0.94f, 1.24f));

            float randValue = Random.value;
            if (randValue < .54f)
                GameManager.instance.audioManager.PlaySfx("airhorn", Random.Range(0.92f, 1.18f));
        }
        catch
        {
            Debug.LogWarning("AudioManager not found. Perhaps you're not using the GameManager prefab?");
        }

        player1Score += value;

        player1ScoreText.text = player1Score.ToString().PadLeft(2, '0');

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

        player2ScoreText.text = player2Score.ToString().PadLeft(2, '0');

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
