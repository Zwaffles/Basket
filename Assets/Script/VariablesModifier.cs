using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VariablesModifier : MonoBehaviour
{
    GameManager gameManager;
    BouncingBall bouncingBall;
    PlayerController playerController;
    Player2Controller player2Controller;
    CubeMover cubeMover;
    [SerializeField] GameObject menuBackgrounds;

    //Camera Varibales
    [SerializeField] Slider shakeDurationVar;
    [SerializeField] Slider shakeAmplitudeVar;
    [SerializeField] Slider shakeFrequencyVar;
    [SerializeField] Slider timeSlowDurationVar;

    //Ball Varibales
    [SerializeField] Slider BounceForceVar;
    [SerializeField] Slider PaddleBoostVar;
    [SerializeField] Slider PaddleEdgesBoostVar;
    [SerializeField] Slider BallMaxSpeedVar;
    [SerializeField] Slider BallTurnsByPaddleSpeedVar;
    [SerializeField] Slider SlowFactorOverHoopWallVar;
    [SerializeField] Slider SlowFactorUnderHoopBlockerVar;

    //PlayerOne Variables
    [SerializeField] Slider speed1Var;
    [SerializeField] Slider speedAcceleration1Var;
    [SerializeField] Slider lerping1Var;
    [SerializeField] Slider leanAngel1Var;
    [SerializeField] Slider leanSpeed1Var;

    //PlayerTwo Variables
    [SerializeField] Slider speed2Var;
    [SerializeField] Slider speedAcceleration2Var;
    [SerializeField] Slider lerping2Var;
    [SerializeField] Slider leanAngel2Var;
    [SerializeField] Slider leanSpeed2Var;

    //Bot Variables
    [SerializeField] Slider speedAIVar;
    [SerializeField] Slider leanAngelAIVar;
    [SerializeField] Slider leanSpeedAIVar;
    [SerializeField] Slider ballChasingAIVar;
    //Transforms
    [SerializeField] Slider paddleSize;
    [SerializeField] Slider paddleYPosition;
    [SerializeField] Slider ballSize;
    [SerializeField] Slider hoopSize;
    [SerializeField] Slider hoop1XPos;
    [SerializeField] Slider hoop1YPos;
    [SerializeField] Slider hoop1ZPos;
    [SerializeField] Slider hoop2XPos;
    [SerializeField] Slider hoop2YPos;
    [SerializeField] Slider hoop2ZPos;

    [SerializeField] GameObject applyButton;

    InputField[] inputfield = new InputField[35];

    Transform ballTransform;
    [SerializeField] Transform[] hoopTransform;
    Transform playerOneTransform;
    Transform playerTwoTransform;
    Transform botTransform;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        bouncingBall = FindObjectOfType<BouncingBall>();
        playerController = FindObjectOfType<PlayerController>();
        player2Controller = FindObjectOfType<Player2Controller>();
        cubeMover = FindObjectOfType<CubeMover>();
    }
    private void OnEnable()
    {
        ballTransform = bouncingBall.gameObject.GetComponent<Transform>();
        playerOneTransform = playerController.gameObject.GetComponent<Transform>();
        //playerTwoTransform = player2Controller.gameObject.GetComponent<Transform>();
        botTransform = cubeMover.gameObject.GetComponent<Transform>();
    }
    void Start()
    {
        

        paddleSize.value = 0;
        paddleYPosition.value = 0;
        ballSize.value = 0;
        hoopSize.value = 0;
        hoop1XPos.value = 0;
        hoop1YPos.value = 0;
        hoop1ZPos.value = 0;
        hoop2XPos.value = 0;
        hoop2YPos.value = 0;
        hoop2ZPos.value = 0;

        shakeDurationVar.value = 0.75f;
        shakeAmplitudeVar.value = 1.2f;
        shakeFrequencyVar.value = 2f;
        timeSlowDurationVar.value = 0.04f;
        //
        BounceForceVar.value = 10;
        PaddleBoostVar.value = 70;
        PaddleEdgesBoostVar.value = 26;
        BallMaxSpeedVar.value = 35;
        BallTurnsByPaddleSpeedVar.value = 0.2f;
        SlowFactorOverHoopWallVar.value = 0.24f;
        SlowFactorUnderHoopBlockerVar.value = 1;
        //
        speed1Var.value = 18;
        speedAcceleration1Var.value = 0.0005f;
        lerping1Var.value = 0.939f;
        leanAngel1Var.value = 1;
        leanSpeed1Var.value = 10;
        //
        speed2Var.value = 18;
        speedAcceleration2Var.value = 0.0005f;
        lerping2Var.value = 0.939f;
        leanAngel2Var.value = 1;
        leanSpeed2Var.value = 10;
        //
        speedAIVar.value = 15.5f;
        leanAngelAIVar.value = 0.5f;
        leanSpeedAIVar.value = 1.5f;
        ballChasingAIVar.value = 0.5f;

        inputfield[0] = shakeDurationVar.GetComponentInChildren<InputField>();
        inputfield[1] = shakeAmplitudeVar.GetComponentInChildren<InputField>();
        inputfield[2] = shakeFrequencyVar.GetComponentInChildren<InputField>();
        inputfield[3] = timeSlowDurationVar.GetComponentInChildren<InputField>();
        //
        inputfield[4] = BounceForceVar.GetComponentInChildren<InputField>();
        inputfield[5] = PaddleBoostVar.GetComponentInChildren<InputField>();
        inputfield[6] = PaddleEdgesBoostVar.GetComponentInChildren<InputField>();
        inputfield[7] = BallMaxSpeedVar.GetComponentInChildren<InputField>();
        inputfield[8] = BallTurnsByPaddleSpeedVar.GetComponentInChildren<InputField>();
        inputfield[9] = SlowFactorOverHoopWallVar.GetComponentInChildren<InputField>();
        inputfield[10] = SlowFactorUnderHoopBlockerVar.GetComponentInChildren<InputField>();
        //
        inputfield[11] = speed1Var.GetComponentInChildren<InputField>();
        inputfield[12] = speedAcceleration1Var.GetComponentInChildren<InputField>();
        inputfield[13] = lerping1Var.GetComponentInChildren<InputField>();
        inputfield[14] = leanAngel1Var.GetComponentInChildren<InputField>();
        inputfield[15] = leanSpeed1Var.GetComponentInChildren<InputField>();
        //
        inputfield[16] = speed2Var.GetComponentInChildren<InputField>();
        inputfield[17] = speedAcceleration2Var.GetComponentInChildren<InputField>();
        inputfield[18] = lerping2Var.GetComponentInChildren<InputField>();
        inputfield[19] = leanAngel2Var.GetComponentInChildren<InputField>();
        inputfield[20] = leanSpeed2Var.GetComponentInChildren<InputField>();
        //
        inputfield[21] = speedAIVar.GetComponentInChildren<InputField>();
        inputfield[22] = leanAngelAIVar.GetComponentInChildren<InputField>();
        inputfield[23] = leanSpeedAIVar.GetComponentInChildren<InputField>();
        inputfield[24] = ballChasingAIVar.GetComponentInChildren<InputField>();
        //
        inputfield[25] = paddleSize.GetComponentInChildren<InputField>();
        inputfield[26] = paddleYPosition.GetComponentInChildren<InputField>();
        inputfield[27] = ballSize.GetComponentInChildren<InputField>();
        inputfield[28] = hoopSize.GetComponentInChildren<InputField>();
        inputfield[29] = hoop1XPos.GetComponentInChildren<InputField>();
        inputfield[30] = hoop1YPos.GetComponentInChildren<InputField>();
        inputfield[31] = hoop1ZPos.GetComponentInChildren<InputField>();
        inputfield[32] = hoop2XPos.GetComponentInChildren<InputField>();
        inputfield[33] = hoop2YPos.GetComponentInChildren<InputField>();
        inputfield[34] = hoop2ZPos.GetComponentInChildren<InputField>();

        MenuButton();
    }
    void Update()
    {
        gameManager.shakeDuration = shakeDurationVar.value;
        gameManager.shakeAmplitude = shakeAmplitudeVar.value;
        gameManager.shakeFrequency = shakeFrequencyVar.value;
        gameManager.timeSlowDuration = timeSlowDurationVar.value;

        bouncingBall.bounceForce = BounceForceVar.value;
        bouncingBall.blockerBoost = PaddleBoostVar.value;
        bouncingBall.edgesBoost = PaddleEdgesBoostVar.value;
        bouncingBall.maxSpeed = BallMaxSpeedVar.value;
        bouncingBall.turningBySpeed = BallTurnsByPaddleSpeedVar.value;
        bouncingBall.slowdownFactorTop = SlowFactorOverHoopWallVar.value;
        bouncingBall.slowdownFactorBottom = SlowFactorUnderHoopBlockerVar.value;

        playerController.speed = speed1Var.value;
        playerController.speedAcceleration = speedAcceleration1Var.value;
        playerController.lerpConstant = lerping1Var.value;
        playerController.leanAngle = leanAngel1Var.value;
        playerController.leanSpeed = leanSpeed1Var.value;

        //player2Controller.speed = speed2Var.value;
        //player2Controller.speedAcceleration = speedAcceleration2Var.value;
        //player2Controller.lerpConstant = lerping2Var.value;
        //player2Controller.leanAngle = leanAngel2Var.value;
        //player2Controller.leanSpeed = leanSpeed2Var.value;

        cubeMover.speed = speedAIVar.value;
        cubeMover.leanAngle = leanAngelAIVar.value;
        cubeMover.leanSpeed = leanSpeedAIVar.value;
        cubeMover.ballChasing = ballChasingAIVar.value;

        OnSliderValueChanged();
    }

    public void OnSliderTransformValueChanged()
    {

        playerOneTransform.localScale = new Vector3(playerOneTransform.localScale.x + paddleSize.value, playerOneTransform.localScale.y, playerOneTransform.localScale.z);
        //playerTwoTransform.localScale = new Vector3(playerTwoTransform.localScale.x + paddleSize.value, playerTwoTransform.localScale.y, playerTwoTransform.localScale.z);
        botTransform.localScale = new Vector3(botTransform.localScale.x + paddleSize.value, botTransform.localScale.y, botTransform.localScale.z);

        playerOneTransform.localPosition = new Vector3(playerOneTransform.localPosition.x, playerOneTransform.localPosition.y + paddleYPosition.value, playerOneTransform.localPosition.z);
        //playerTwoTransform.localPosition = new Vector3(playerTwoTransform.localPosition.x, playerTwoTransform.localPosition.y * paddleYPosition.value, playerTwoTransform.localPosition.z);
        botTransform.localPosition = new Vector3(botTransform.localPosition.x, botTransform.localPosition.y + paddleYPosition.value, botTransform.localPosition.z);

        ballTransform.localScale = new Vector3(ballTransform.localScale.x + ballSize.value, ballTransform.localScale.y + ballSize.value, ballTransform.localScale.z + ballSize.value) ;

        hoopTransform[0].position = new Vector3(hoopTransform[0].position.x + hoop1XPos.value, hoopTransform[0].position.y + hoop1YPos.value, hoopTransform[0].position.z + hoop1ZPos.value);
        hoopTransform[1].position = new Vector3(hoopTransform[1].position.x + hoop2XPos.value, hoopTransform[1].position.y + hoop2YPos.value, hoopTransform[1].position.z + hoop2ZPos.value);

        hoopTransform[0].localScale = new Vector3(hoopTransform[0].localScale.x + hoopSize.value, hoopTransform[0].localScale.y + hoopSize.value, hoopTransform[0].localScale.z + hoopSize.value);
        hoopTransform[1].localScale = new Vector3(hoopTransform[1].localScale.x + hoopSize.value, hoopTransform[1].localScale.y + hoopSize.value, hoopTransform[1].localScale.z + hoopSize.value);
    }
    public void OnSliderValueChanged()
    {
        inputfield[0].text = shakeDurationVar.value.ToString();
        inputfield[1].text = shakeAmplitudeVar.value.ToString();
        inputfield[2].text = shakeFrequencyVar.value.ToString();
        inputfield[3].text = timeSlowDurationVar.value.ToString();

        inputfield[4].text = BounceForceVar.value.ToString();
        inputfield[5].text = PaddleBoostVar.value.ToString();
        inputfield[6].text = PaddleEdgesBoostVar.value.ToString();
        inputfield[7].text = BallMaxSpeedVar.value.ToString();
        inputfield[8].text = BallTurnsByPaddleSpeedVar.value.ToString();
        inputfield[9].text = SlowFactorOverHoopWallVar.value.ToString();
        inputfield[10].text = SlowFactorUnderHoopBlockerVar.value.ToString();

        inputfield[11].text = speed1Var.value.ToString();
        inputfield[12].text = speedAcceleration1Var.value.ToString();
        inputfield[13].text = lerping1Var.value.ToString();
        inputfield[14].text = leanAngel1Var.value.ToString();
        inputfield[15].text = leanSpeed1Var.value.ToString();

        inputfield[16].text = speed2Var.value.ToString();
        inputfield[17].text = speedAcceleration2Var.value.ToString();
        inputfield[18].text = lerping2Var.value.ToString();
        inputfield[19].text = leanAngel2Var.value.ToString();
        inputfield[20].text = leanSpeed2Var.value.ToString();

        inputfield[21].text = speedAIVar.value.ToString();
        inputfield[22].text = leanAngelAIVar.value.ToString();
        inputfield[23].text = leanSpeedAIVar.value.ToString();
        inputfield[24].text = ballChasingAIVar.value.ToString();

        inputfield[25].text = paddleSize.value.ToString();
        inputfield[26].text = paddleYPosition.value.ToString();
        inputfield[27].text = ballSize.value.ToString();
        inputfield[28].text = hoopSize.value.ToString();
        inputfield[29].text = hoop1XPos.value.ToString();
        inputfield[30].text = hoop1YPos.value.ToString();
        inputfield[31].text = hoop1ZPos.value.ToString();
        inputfield[32].text = hoop2XPos.value.ToString();
        inputfield[33].text = hoop2YPos.value.ToString();
        inputfield[34].text = hoop2ZPos.value.ToString();
    }
    public void MenuButton()
    {
        if (this.gameObject.activeSelf)
        {
            this.gameObject.SetActive(false);
            menuBackgrounds.SetActive(false);
            applyButton.SetActive(false);
        }
        else if (!this.gameObject.activeSelf)
        {
            this.gameObject.SetActive(true);
            menuBackgrounds.SetActive(true);
            applyButton.SetActive(true);
        }
    }
}
