using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public enum GameState
{
    Menu,
    Play,
    Multiplayer,
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public AudioManager audioManager { get; private set; }
    public UIManager uiManager { get; private set; }
    public ScoreManager scoreManager { get; private set; }
    public PlayerConfigurationManager playerConfigurationManager { get; private set; }
    public AchievementManager achievementManager { get; private set; }

    [SerializeField]
    private GameState currentState = GameState.Play;

    public GameState CurrentState
    {
        get
        {
            return currentState;
        }

        set
        {
            currentState = value;
        }
    }

    [SerializeField] GameObject playerControllerPrefab;
    [SerializeField] Material player1Material;
    [SerializeField] public GameObject AI { get; private set; }
    [SerializeField] GameObject playerTwo;
    [SerializeField] GameObject ball;
    [SerializeField] Transform ballRightSpawn;
    [SerializeField] Transform ballLeftSpawn;  
    Rigidbody ballRigidbody;
    ChangeCameraColor changeCameraColor;
    public CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;
    public float shakeDuration = 2f;
    public float shakeAmplitude = 1.2f;
    public float shakeFrequency = 2.0f;
    private float shakeElapsedTime = 0f;

    public float timeSlowDuration = 0.1f;
    private float timeSlowElapsedTime = 0f;

    private float originalTimeScale = 1f;

    private Scene currentScene;

    private PlayerInput playerInput;

    [SerializeField] public bool multiBallsMode = false;

    [SerializeField] float MaxtimeForWarning = 4;
    [SerializeField] float timeBeforeWarningRespawn = 2; //seconds
    [SerializeField] GameObject[] warningSign;

    bool warningSignIsOn1 = false;
    bool warningSignIsOn2 = false;

    public bool player1Left { get; set; } = true;

    private void Awake()
    {
        
        // Ensure that only one instance of the GameManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Find references to other managers in the scene
        audioManager = FindObjectOfType<AudioManager>();
        uiManager = FindObjectOfType<UIManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
        playerConfigurationManager = FindObjectOfType<PlayerConfigurationManager>();
        playerInput = GetComponent<PlayerInput>();
        achievementManager = FindObjectOfType<AchievementManager>();
        
    }

    private void Start()
    {
        if (currentState == GameState.Menu)
        {
            StartMenu();
            Instantiate(uiManager.fadeToBlack, uiManager.transform);
        }

        if (currentState == GameState.Play)
            InitializeScene(SceneManager.GetActiveScene().name, isMultiplayer: false);
        if (CurrentState == GameState.Multiplayer)
            StartMultiplayer();

        // Hides the cursor...
        Cursor.visible = false;

        currentTimeBeforeRespawningCausedByWarning1 = MaxtimeForWarning;
        currentTimeBeforeRespawningCausedByWarning2 = MaxtimeForWarning;
        warningSign[0].SetActive(false);
        warningSign[1].SetActive(false);
    }

    public void StartMenu()
    {
        StopCoroutine("Respawn");
        ResetWarnings();

        player1Left = true;

        uiManager.ToggleMainMenu(true);
        uiManager.ToggleScore(false);

        currentState = GameState.Menu;

        playerConfigurationManager.AllowJoining(false);

        StartCoroutine(startMenuMusic());

    }

    public void StartMatch(bool isMultiplayer)
    {
        uiManager.ToggleMainMenu(false);
        uiManager.ToggleScore(true);

        //playerConfigurationManager.AllowJoining(false);

        currentState = GameState.Play;

        if (AI == null)
        {
            AI = GameObject.FindWithTag("AI");
        }

        if(playerTwo == null)
        {
            playerTwo = GameObject.FindWithTag("PlayerTwo");
        }

        if(ball == null)
        {
            ball = GameObject.FindWithTag("Ball");
        }

        if(virtualCamera == null)
        {
            virtualCamera = GameObject.FindWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
        }

        if (virtualCamera != null)
            virtualCameraNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        if(!multiBallsMode)
        ballRigidbody = ball.GetComponent<Rigidbody>();

        if (isMultiplayer)
        {
            AI.SetActive(false);
        }

        //else
        //{
        //    var playerController = Instantiate(playerControllerPrefab);
        //    var playerInput = playerController.GetComponent<PlayerInput>();

        //    playerInput.neverAutoSwitchControlSchemes = false;
        //    playerConfigurationManager.HandlePlayerJoin(playerInput);
        //    playerConfigurationManager.SetPlayerColor(playerInput.playerIndex, player1Material);
        //}

        currentScene = SceneManager.GetActiveScene();

        try
        {
            // Old
            //audioManager.PlayMusic("Match Game");
            // New
            //audioManager.PlayMusic("Vonboff_-_Hit_the_bits");
            // Moved to InitializeLevel
        }
        catch
        {
            Debug.LogWarning("AudioManager not found. Perhaps you're not using the GameManager prefab?");
        }
    }

    public void StartMultiplayer()
    {
        if(currentState != GameState.Multiplayer)
        {
            currentState = GameState.Multiplayer;
        }

        uiManager.ToggleMainMenu(false);
        uiManager.ToggleScore(false);

        playerConfigurationManager.AllowJoining(true);
    }

    public void InitializeScene(string scenePath, bool isMultiplayer = false)
    {
        StartCoroutine(LoadSceneAsync(scenePath, isMultiplayer));

        if (!isMultiplayer)
        {
            playerConfigurationManager.ClearPlayerConfigurations();

            playerConfigurationManager.AllowJoining(true);
            var playerController = Instantiate(playerControllerPrefab);
            var playerInput = playerController.GetComponent<PlayerInput>();

            playerInput.neverAutoSwitchControlSchemes = false;
            playerConfigurationManager.HandlePlayerJoin(playerInput);
            playerConfigurationManager.SetPlayerColor(playerInput.playerIndex, player1Material, Color.green);
            playerConfigurationManager.AllowJoining(false);
        }
    }

    private IEnumerator LoadSceneAsync(string scenePath, bool isMultiplayer)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scenePath);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        StartMatch(isMultiplayer);
    }
    
    public void RespawnBall(int value)
    {
        Time.timeScale = originalTimeScale * 0.25f;
        Invoke("ResetTimeScale", timeSlowDuration);

        if(!multiBallsMode)
            StartCoroutine("Respawn", value);

        if(ball != null)
            ball.SetActive(false);

        ResetWarning(-1);
        ResetWarning(1);
    }

    public void RemoveBall()
    {
        StopCoroutine("Respawn");
        ball.SetActive(false);
        ball.transform.position = Vector3.zero;
    }

    IEnumerator Respawn(int rightorLeft)
    {
        yield return new WaitForSeconds(.02f);
        shakeElapsedTime = shakeDuration;
        ballRigidbody.velocity = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(.74f);
        if (rightorLeft == 1)
        {
            ballRightSpawn.position = new Vector3 (Random.Range( 6,11), 16, 0);
            ball.transform.position = ballRightSpawn.position;
            ball.SetActive(true);
        }
        else if (rightorLeft == -1)
        {
            ballLeftSpawn.position = new Vector3(Random.Range(-11, -6), 16, 0);
            ball.transform.position = ballLeftSpawn.position;
            ball.SetActive(true);
        }
    }

    private void Update()
    {
        if (virtualCameraNoise != null)
            CameraShake();

        if(!warningSignIsOn1)
            ShowWarningSign1();

        if (!warningSignIsOn2)
            ShowWarningSign2();

        if (ball != null)
        if(ball.transform.position.x > -0.72f)
        {
            ResetWarning(1);
        }

        if (ball != null)
            if (ball.transform.position.x < -0.72f)
            {
                ResetWarning(-1);
            }
    }
    
    void ResetWarning(int value)
    {
        if(value == 1)
        {
            currentTimeBeforeRespawningCausedByWarning1 = MaxtimeForWarning;
            warningSignIsOn1 = false;
            warningSign[0].SetActive(false);
        }
        if(value == -1)
        {
            currentTimeBeforeRespawningCausedByWarning2 = MaxtimeForWarning;
            warningSignIsOn2 = false;
            warningSign[1].SetActive(false);
        }
    }

    void ResetWarnings()
    {
        currentTimeBeforeRespawningCausedByWarning1 = MaxtimeForWarning;
        warningSignIsOn1 = false;
        warningSign[0].SetActive(false);

        currentTimeBeforeRespawningCausedByWarning2 = MaxtimeForWarning;
        warningSignIsOn2 = false;
        warningSign[1].SetActive(false);
    }

    void ResetTimeScale()
    {
        Time.timeScale = originalTimeScale;
    }

    [SerializeField] float currentTimeBeforeRespawningCausedByWarning1; //Seconds left player
    [SerializeField] float currentTimeBeforeRespawningCausedByWarning2; //Seconds right player
    
    void ShowWarningSign1()
    {
        if (ball != null && !multiBallsMode)
            if (ball.transform.position.x < -0.72f && !warningSignIsOn1)
            {
                currentTimeBeforeRespawningCausedByWarning1 -= Time.deltaTime;
                if (currentTimeBeforeRespawningCausedByWarning1 <= 0)
                {
                    warningSignIsOn1 = true;
                    StartCoroutine("WarningIsOn1",1);
                    warningSign[0].SetActive(true);
                }
            }
    }
    void ShowWarningSign2()
    {
        
        if (ball != null && !multiBallsMode)
            if (ball.transform.position.x > -0.72f && !warningSignIsOn2)
            {
                currentTimeBeforeRespawningCausedByWarning2 -= Time.deltaTime;
                if (currentTimeBeforeRespawningCausedByWarning2 <= 0)
                {
                    warningSignIsOn2 = true;
                    StartCoroutine("WarningIsOn2", -1);
                    warningSign[1].SetActive(true);
                }
            }
    }
    IEnumerator WarningIsOn1(int value)
    {
        yield return new WaitForSeconds(timeBeforeWarningRespawn);
            if (warningSignIsOn1)
            {
                warningSignIsOn1 = false;
                RespawnBall(value);
                currentTimeBeforeRespawningCausedByWarning1 = MaxtimeForWarning;
            }
    }
    IEnumerator WarningIsOn2(int value)
    {
        yield return new WaitForSeconds(timeBeforeWarningRespawn);
        if (warningSignIsOn2)
        {
            warningSignIsOn2 = false;
            RespawnBall(value);
            currentTimeBeforeRespawningCausedByWarning2 = MaxtimeForWarning;
        }
    }
    private void CameraShake()
    {
        if (shakeElapsedTime > 0)
        {
            virtualCameraNoise.m_AmplitudeGain = shakeAmplitude;
            virtualCameraNoise.m_FrequencyGain = shakeFrequency;

            shakeElapsedTime -= Time.deltaTime;
        }
        else
        {
            virtualCameraNoise.m_AmplitudeGain = 0f;
            shakeElapsedTime = 0f;
        }

        if (timeSlowElapsedTime > 0)
        {
            timeSlowElapsedTime -= Time.deltaTime;
        }
        else
        {
            timeSlowElapsedTime = 0f;
        }
    }

    private IEnumerator startMenuMusic()
    {
        yield return new WaitForSeconds(0.5f);

        if (audioManager.GetCurrentMusic().Equals("Von_-_Bits_and_beats")) yield break;

        try
        {
            audioManager.PlayMusic("Von_-_Bits_and_beats");
        }
        catch
        {
            Debug.LogWarning("AudioManager not found. Perhaps you're not using the GameManager prefab?");
        }

    }

}
