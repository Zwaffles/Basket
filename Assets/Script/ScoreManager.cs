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

    [HideInInspector] public bool lookForNoTarget = false;

    private UIManager uiManager;

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

    private void Start()
    {
        uiManager = GameManager.instance.uiManager;
        
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
        if (!GameManager.instance.multiBallsMode)
        {
            if (winByScore)
            {
                if (player1Score >= scoreTarget)
                {
                    HandleEndOfMatchAchievements(false);

                    Instantiate(uiManager.player1Wins);
                    GetMatchOverRules();
                    weHaveAWinner = true;
                }
                if (player2Score >= scoreTarget)
                {
                    HandleEndOfMatchAchievements(true);

                    Instantiate(uiManager.player2Wins);
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
                        HandleEndOfMatchAchievements(false);

                        Instantiate(uiManager.player1Wins);
                        GetMatchOverRules();
                        weHaveAWinner = true;
                    }
                    if (player2Score > player1Score)
                    {
                        HandleEndOfMatchAchievements(true);

                        Instantiate(uiManager.player2Wins);
                        GetMatchOverRules();
                        weHaveAWinner = true;
                    }
                    else if (player1Score == player2Score)
                    {
                        HandleEndOfMatchAchievements(true, true);

                        Instantiate(uiManager.draw);
                        weHaveAWinner = true;
                        GetMatchOverRules();
                    }
                }
            }
        }
        if(GameManager.instance.multiBallsMode)
        {
            if(player1Score + player2Score > 14)
            {
                lookForNoTarget = true;
                if (player1Score > player2Score)
                {
                    HandleEndOfMatchAchievements(false);

                    Instantiate(uiManager.player1Wins);
                    GetMatchOverRules();
                    weHaveAWinner = true;
                    //FindObjectOfType<CubeMover>().multiBalls = false;
                }
                if (player2Score > player1Score)
                {
                    HandleEndOfMatchAchievements(true);

                    Instantiate(uiManager.player2Wins);
                    GetMatchOverRules();
                    weHaveAWinner = true;
                    //FindObjectOfType<CubeMover>().multiBalls = false;
                }
            }
            if(player1Score + player2Score < 15)
            {
                lookForNoTarget = false;
            }
        }
        if (winByTime)
        {
            if (timeSpent >= matchTime)
            {
                if (player1Score > player2Score)
                {
                    HandleEndOfMatchAchievements(false);

                    Instantiate(uiManager.player1Wins);
                    GetMatchOverRules();
                    weHaveAWinner = true;
                }
                if (player2Score > player1Score)
                {
                    HandleEndOfMatchAchievements(true);

                    Instantiate(uiManager.player2Wins);
                    GetMatchOverRules();
                    weHaveAWinner = true;
                }
                else if (player1Score == player2Score)
                {
                    HandleEndOfMatchAchievements(true, true);

                    Instantiate(uiManager.draw);
                    weHaveAWinner = true;
                    GetMatchOverRules();
                }
            }
        }
    }

    #region Achievements

    private bool player1hasAchievedFivePointLead = false;
    private bool player2hasAchievedFivePointLead = false;

    private int player1ConsecutiveGoals = 0;
    private int player2ConsecutiveGoals = 0;
    private bool player1hasAchievedThreeConsecutiveGoals = false;
    private bool player2hasAchievedThreeConsecutiveGoals = false;

    private bool player1hasAchievedFiveGoals = false;
    private bool player2hasAchievedFiveGoals = false;

    private bool player1hasScoredSelfGoal = false;
    private bool player2hasScoredSelfGoal = false;

    private void HandleEndOfMatchAchievements(bool player2Won, bool draw = false)
    {

        HandleSeeYouInTheCourtAchievement();

        if (!draw)
        {
            HandleComebackTimeAchievement(player2Won);
            HandleFirstOfManyAchievement(player2Won);
        }

    }

    private void HandleFirstOfManyAchievement(bool player2Won)
    {

        if (player2Won)
        {
            if (!GameManager.instance.AI.activeInHierarchy)
                GameManager.instance.achievementManager.GiveAchievement(Achievement.FirstOfMany);
        }
        else
        {
            GameManager.instance.achievementManager.GiveAchievement(Achievement.FirstOfMany);
        }

    }

    private void HandleComebackTimeAchievement(bool player2Won)
    {

        if (player2Won)
        {
            
            if (!player1hasAchievedFivePointLead) return;

            if (!GameManager.instance.AI.activeInHierarchy)
                GameManager.instance.achievementManager.GiveAchievement(Achievement.ComebackTime);
        }
        else
        {

            if (!player2hasAchievedFivePointLead) return;
            
            GameManager.instance.achievementManager.GiveAchievement(Achievement.ComebackTime);
        }

    }

    private void HandleSeeYouInTheCourtAchievement()
    {
        if (!GameManager.instance.AI.activeInHierarchy)
            GameManager.instance.achievementManager.GiveAchievement(Achievement.SeeYouInCourt);
    }

    private void HandleWeOnlyShoot3PointersAchievement()
    {

        if (player1hasAchievedThreeConsecutiveGoals) 
            GameManager.instance.achievementManager.GiveAchievement(Achievement.WeOnlyShoot3Pointers);

        if (player2hasAchievedThreeConsecutiveGoals && !GameManager.instance.AI.activeInHierarchy)
            GameManager.instance.achievementManager.GiveAchievement(Achievement.WeOnlyShoot3Pointers);

    }

    private void HandleKeepTheShotsComingAchievement()
    {
        if (player1hasAchievedFiveGoals)
            GameManager.instance.achievementManager.GiveAchievement(Achievement.KeepTheShotsComing);

        if (player2hasAchievedFiveGoals && !GameManager.instance.AI.activeInHierarchy)
            GameManager.instance.achievementManager.GiveAchievement(Achievement.KeepTheShotsComing);
    }

    private void HandleWhoseHoopAchievement()
    {
        if (player1hasScoredSelfGoal)
            GameManager.instance.achievementManager.GiveAchievement(Achievement.WhoseHoop);

        if (player2hasScoredSelfGoal && !GameManager.instance.AI.activeInHierarchy)
            GameManager.instance.achievementManager.GiveAchievement(Achievement.WhoseHoop);
    }

    #endregion

    void GetMatchOverRules()
    {
        if (weHaveAWinner)
        {
            isRunning = false;
            timeSpent = 0;
            weHaveAWinner = false;
            fireWork.SetActive(true);
            GameManager.instance.RemoveBall();
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

    public void ResetScore()
    {
        player1Score = 0;
        player2Score = 0;

        player1ScoreText.text = "00";
        player2ScoreText.text = "00";
    }

    public void PlayerOneScore(int value, GameObject ball)
    {
        try
        {
            GameManager.instance.audioManager.PlaySfx("Goal_ClapClapClap", Random.Range(0.94f, 1.24f));
            // New
            GameManager.instance.audioManager.PlayVoice("Score_-Noel_-Deep", Random.Range(0.92f, 1.18f));

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

        // Achievement-stuff

        if (player2Score + 4 < player1Score) player1hasAchievedFivePointLead = true;

        player1ConsecutiveGoals += value;
        player2ConsecutiveGoals = 0;

        if (player1ConsecutiveGoals > 2) player1hasAchievedThreeConsecutiveGoals = true;
        HandleWeOnlyShoot3PointersAchievement();

        if (player1Score > 4) player1hasAchievedFiveGoals = true;
        HandleKeepTheShotsComingAchievement();

        try
        {
            BouncingBall bouncingBall = ball.GetComponent<BouncingBall>();
            if (bouncingBall.lastTouchedPlayer2) player2hasScoredSelfGoal = true;
            HandleWhoseHoopAchievement();
        }
        catch
        {
            Debug.LogWarning("Gameobject sent is not a bouncing ball (anymore at least)");
        }

    }

    public void PlayerTwoScore(int value, GameObject ball)
    {
        try
        {
            GameManager.instance.audioManager.PlaySfx("Goal_ClapClapClap", Random.Range(0.94f, 1.24f));
            // New
            GameManager.instance.audioManager.PlayVoice("Score_-Noel_-Deep", Random.Range(0.92f, 1.18f));

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

        // Achievement-stuff

        if (player1Score + 4 < player2Score) player2hasAchievedFivePointLead = true;

        player1ConsecutiveGoals = 0;
        player2ConsecutiveGoals += value;

        if (player2ConsecutiveGoals > 2) player2hasAchievedThreeConsecutiveGoals = true;
        HandleWeOnlyShoot3PointersAchievement();

        if (player2Score > 4) player2hasAchievedFiveGoals = true;
        HandleKeepTheShotsComingAchievement();

        try
        {
            BouncingBall bouncingBall = ball.GetComponent<BouncingBall>();
            if (bouncingBall.lastTouchedPlayer1) player1hasScoredSelfGoal = true;
            HandleWhoseHoopAchievement();
        }
        catch
        {
            Debug.LogWarning("Gameobject sent is not a bouncing ball (anymore at least)");
        }

    }

    // Converts a float "time in seconds" to a 00:00 formatted string
    private string DisplayTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        return $"{minutes:00}:{seconds:00}";
    }
}
