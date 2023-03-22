using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public AudioManager audioManager { get; private set; }

    [SerializeField] GameObject AI;
    [SerializeField] GameObject playerTwo;
    [SerializeField] TextMeshProUGUI score;
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

    Scene currentScene;
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

        changeCameraColor = FindObjectOfType<ChangeCameraColor>();
    }
    private void Start()
    {
        if (virtualCamera != null)
            virtualCameraNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        ballRigidbody = ball.GetComponent<Rigidbody>();
        playerTwo.SetActive(false);

        originalTimeScale = Time.timeScale;

        currentScene = SceneManager.GetActiveScene();

        audioManager.PlayMusic("taratata");
    }

    public void ChangeScene()
    {      
        if (currentScene.name == "Version 02")
        {
            SceneManager.LoadScene("Version 03");
        }

        if(currentScene.name == "Version 03")
        {
            SceneManager.LoadScene("Version 04");
        }

        if (currentScene.name == "Version 04")
        {
            SceneManager.LoadScene("Version 02");
        }
    }
        public void SwitchPlayers()
        {
        if (AI.activeSelf)
        {
            AI.SetActive(false);
            playerTwo.SetActive(true);
            score.color = Color.blue;
        }else if (!AI.activeSelf)
        {
            AI.SetActive(true);
            playerTwo.SetActive(false);
            score.color = Color.red;
        }
    }

    public void RespawnBall(int value)
    {
        changeCameraColor.changeBGColor = true;
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
            ball.transform.position = ballRightSpawn.position;
            ball.SetActive(true);
        }
        else if(rightorLeft == -1)
        {
            ball.transform.position = ballLeftSpawn.position;
            ball.SetActive(true);
        }
    }

    private void Update()
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
