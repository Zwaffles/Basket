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
    [SerializeField] GameObject AI;
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

    private float originalTimeScale;

    private Scene currentScene;

    private PlayerInput playerInput;
    
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
        
    }

    private void Start()
    {
        if (currentState == GameState.Menu)
            StartMenu();
        if (currentState == GameState.Play)
            InitializeScene(SceneManager.GetActiveScene().name, isMultiplayer: false);
        if (CurrentState == GameState.Multiplayer)
            StartMultiplayer();

        //scoreManager.winnerText = GameObject.FindGameObjectWithTag("WinnerText");
    }

    public void StartMenu()
    {
        uiManager.ToggleMainMenu(true);
        uiManager.ToggleScore(false);

        currentState = GameState.Menu;

        playerConfigurationManager.AllowJoining(false);
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

        originalTimeScale = Time.timeScale;

        currentScene = SceneManager.GetActiveScene();

        try
        {
            audioManager.PlayMusic("Match Game");
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
        if (!isMultiplayer && playerConfigurationManager.GetPlayerConfigurations().Count < 1)
        {
            playerConfigurationManager.AllowJoining(true);
            var playerController = Instantiate(playerControllerPrefab);
            var playerInput = playerController.GetComponent<PlayerInput>();

            playerInput.neverAutoSwitchControlSchemes = false;
            playerConfigurationManager.HandlePlayerJoin(playerInput);
            playerConfigurationManager.SetPlayerColor(playerInput.playerIndex, player1Material);
            playerConfigurationManager.AllowJoining(false);
        }

        StartCoroutine(LoadSceneAsync(scenePath, isMultiplayer));
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

    //public void ChangeScene()
    //{      
    //    if (currentScene.name == "Version 02")
    //    {
    //        SceneManager.LoadScene("Version 03");
    //    }

    //    if (currentScene.name == "Version 03")
    //    {
    //        SceneManager.LoadScene("Version 04");
    //    }

    //    if (currentScene.name == "Version 04")
    //    {
    //        SceneManager.LoadScene("Version 02");
    //    }
    //}
    

    public void RespawnBall(int value)
    {
        timeSlowElapsedTime = timeSlowDuration;
        Time.timeScale = originalTimeScale * 0.25f;
        StartCoroutine("Respawn", value);
        ball.SetActive(false);
    }

    IEnumerator Respawn(int rightorLeft)
    {
        yield return new WaitForSeconds(.17f);
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
        if(virtualCameraNoise != null)
            CameraShake();
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
            Time.timeScale = originalTimeScale;
            timeSlowElapsedTime = 0f;
        }
    }
}
