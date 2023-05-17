using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine.UI;
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
    //[SerializeField] int matchTimeBeforeHittingTheScoreTarget = 600; // counted by seconnds
    //[SerializeField] int maxMatchTime = 900; // counted by seconds

    [SerializeField] InputField playerOneName;
    [SerializeField] InputField playerTwoName;
    float timeSpent = 0;
    [SerializeField] GameObject fireWork;
    //[SerializeField] GameObject extraTimeObject;
    private bool isRunning = false;

    [HideInInspector] public int player1Score;
    [HideInInspector] public int player2Score;
    bool weHaveAWinner = false;

    [SerializeField] bool winByScore;
    [SerializeField] bool winByTime;
    [SerializeField] int matchTime = 900; // counted by seconds
    [SerializeField] int scoreTarget = 10;

    float currentBlockerBoost;
    float currentMaxSpeed;
    float currentEdgesBoost;

    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        player1ScoreText = root.Q<TextElement>("UI-ScoreLeft-Text");
        player2ScoreText = root.Q<TextElement>("UI-ScoreRight-Text");
        uiTimer = root.Q<TextElement>("UI-Time-Text");
        bouncingBall = FindObjectOfType<BouncingBall>();
        timeSpent = 0.1f;
        fireWork.SetActive(false);

    }
    private void Update()
    {
        if (isRunning)
        {
            timeSpent += Time.deltaTime;
            UpdateTimerUI(timeSpent);
        }
        if (timeSpent > 0)
        {
            WinCondition();
        }
    }
    void WinCondition()
    {
        if (winByScore)
        {
            if (player1Score >= scoreTarget)
            {
                winnerText.text = "Congratulations " + playerOneName.text;
                GetMatchOverRules();
                weHaveAWinner = true;
            }
            if (player2Score >= scoreTarget)
            {
                winnerText.text = "Congratulations " + playerTwoName.text;
                GetMatchOverRules();
                weHaveAWinner = true;
            }
        }
        if (winByTime)
        {
            if (timeSpent >= matchTime)
            {
                if (player1Score > player2Score)
                {
                    winnerText.text = "Congratulations " + playerOneName.text;
                    GetMatchOverRules();
                    weHaveAWinner = true;
                }
                if (player2Score > player1Score)
                {
                    winnerText.text = "Congratulations " + playerTwoName.text;
                    GetMatchOverRules();
                    weHaveAWinner = true;
                }
                else if (player1Score == player2Score)
                {
                    winnerText.text = "Draw";
                    weHaveAWinner = true;
                    GetMatchOverRules();
                }
            }
        }
    }

    //void winCondition()
    //{
    //    // timespent is counted by seconds >> 600 seconds = 10 minutes
    //    if (winnerText != null && timeSpent <= matchTimeBeforeHittingTheScoreTarget && timeSpent < maxMatchTime)
    //    {
    //        if (player1Score > scoreTarget && player1Score > player2Score + 1 && !weHaveAWinner)
    //        {
    //            winnerText.text = "Congratulations " + playerOneName.text;
    //            GetMatchOverRules();
    //            weHaveAWinner = true;
    //        }
    //        if (player2Score > scoreTarget && player2Score > player1Score + 1 && !weHaveAWinner)
    //        {
    //            winnerText.text = "Congratulations " + playerTwoName.text;
    //            GetMatchOverRules();
    //            weHaveAWinner = true;
    //        }
    //        else if(timeSpent >= matchTimeBeforeHittingTheScoreTarget)
    //        {
    //            extraTimeObject.SetActive(true);
    //        }
    //    }
    //    if (winnerText != null && timeSpent > matchTimeBeforeHittingTheScoreTarget && timeSpent < maxMatchTime)
    //    {
    //        if (player1Score > player2Score + 1 && !weHaveAWinner)
    //        {
    //            winnerText.text = "Congratulations " + playerOneName.text;
    //            weHaveAWinner = true;
    //            GetMatchOverRules();
    //        }
    //        if (player2Score > player1Score + 1 && !weHaveAWinner)
    //        {
    //            winnerText.text = "Congratulations " + playerTwoName.text;
    //            weHaveAWinner = true;
    //            GetMatchOverRules();
    //        }
    //        else if (timeSpent >= matchTimeBeforeHittingTheScoreTarget)
    //        {
    //            extraTimeObject.SetActive(true);
    //        }
    //    }
    //    if (winnerText != null && timeSpent >= maxMatchTime)
    //    {
    //        if (player1Score > player2Score + 1 && !weHaveAWinner)
    //        {
    //            winnerText.text = "Congratulations " + playerOneName.text;
    //            weHaveAWinner = true;
    //            GetMatchOverRules();
    //        }
    //        if (player2Score > player1Score + 1 && !weHaveAWinner)
    //        {
    //            winnerText.text = "Congratulations " + playerTwoName.text;
    //            weHaveAWinner = true;
    //            GetMatchOverRules();
    //        }
    //        else if (!weHaveAWinner)
    //        {
    //            winnerText.text = "Draw";
    //            weHaveAWinner = true;
    //            GetMatchOverRules();
    //        }
    //    }

    //}
    //void SetCurrentRules()
    //{
    //    bouncingBall = FindObjectOfType<BouncingBall>();
    //    currentBlockerBoost = bouncingBall.blockerBoost;
    //    currentMaxSpeed = bouncingBall.maxSpeed;
    //    currentEdgesBoost = bouncingBall.edgesBoost;
    //}
    void GetMatchOverRules()
    {
        if (weHaveAWinner)
        {
            isRunning = false;
            timeSpent = 0;
            weHaveAWinner = false;
            fireWork.SetActive(true);
        }
    }
    //private void AdjustRules()
    //{
    //    bouncingBall = FindObjectOfType<BouncingBall>();
    //    bouncingBall.blockerBoost = 9;
    //    bouncingBall.maxSpeed = 12;
    //    bouncingBall.edgesBoost = 6;
        // This should be linked to later to the escape button

        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //bouncingBall.rulesReset = true;
        //fireWork.SetActive(false);          
        //winnerText.text = "";
        //bouncingBall.blockerBoost = currentBlockerBoost;
        //bouncingBall.maxSpeed = currentMaxSpeed;
        //bouncingBall.edgesBoost = currentEdgesBoost;
    //}
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
            // New
            GameManager.instance.audioManager.PlaySfx("Score_-Noel_-Deep", Random.Range(0.92f, 1.18f));

            float randValue = Random.value;
            if (randValue < .54f) 
            {
                // Old
                GameManager.instance.audioManager.PlaySfx("airhorn", Random.Range(0.92f, 1.18f));
            }
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
            // New
            GameManager.instance.audioManager.PlaySfx("Score_-Noel_-Deep", Random.Range(0.92f, 1.18f));

            float randValue = Random.value;
            if (randValue < .54f)
            {
                // Old
                GameManager.instance.audioManager.PlaySfx("airhorn", Random.Range(0.92f, 1.18f));
            }
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
