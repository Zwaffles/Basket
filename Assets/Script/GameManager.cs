using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;

public class GameManager : MonoBehaviour
{
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

    private void Awake()
    {
        changeCameraColor = FindObjectOfType<ChangeCameraColor>();
    }
    private void Start()
    {
        if (virtualCamera != null)
            virtualCameraNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        ballRigidbody = ball.GetComponent<Rigidbody>();
        playerTwo.SetActive(false);

        originalTimeScale = Time.timeScale;
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
    }
    IEnumerator Respawn(int rightorLeft)
    {
        yield return new WaitForSeconds(.5f);
        shakeElapsedTime = shakeDuration;
        ball.SetActive(false);
        ballRigidbody.velocity = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(.75f);
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
