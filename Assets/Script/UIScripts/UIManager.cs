using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements"), SerializeField]
    private MainMenu mainMenu;
    [SerializeField]
    private ScoreManager score;
    //[SerializeField]
    //private RestartButton restartButton;
    [SerializeField]
    private PauseMenu pauseMenu;
    [SerializeField]
    private OptionsMenu optionsMenu;
    [SerializeField]
    private VideoMenu videoMenu;
    [SerializeField]
    private AudioMenu audioMenu;
    [SerializeField]
    private LanguageMenu languageMenu;
    [SerializeField]
    private ModeMenu modeMenu;
    [SerializeField]
    private CreditScroll creditScroll;

    private Inputaction uiInput;

    public WinscreenAnim player1Wins;
    public WinscreenAnim player2Wins;
    public WinscreenAnim draw;

    public Fadetoblack fadeToBlack;

    private void Awake()
    {
        uiInput = new Inputaction();

        uiInput.UI.Submit.performed += ctx => mainMenu.Submit(ctx);
        uiInput.UI.Submit.performed += ctx => optionsMenu.Submit(ctx);
        uiInput.UI.Submit.performed += ctx => videoMenu.Submit(ctx);
        uiInput.UI.Submit.performed += ctx => audioMenu.Submit(ctx);
        uiInput.UI.Submit.performed += ctx => creditScroll.Submit(ctx);
        uiInput.UI.Submit.performed += ctx => pauseMenu.Submit(ctx);
        uiInput.UI.Submit.performed += ctx => languageMenu.Submit(ctx);
        uiInput.UI.Submit.performed += ctx => modeMenu.Submit(ctx);

        uiInput.UI.Navigate.performed += ctx => mainMenu.Navigate(ctx);
        uiInput.UI.Navigate.performed += ctx => optionsMenu.Navigate(ctx);
        uiInput.UI.Navigate.performed += ctx => videoMenu.Navigate(ctx);
        uiInput.UI.Navigate.performed += ctx => audioMenu.Navigate(ctx);
        uiInput.UI.Navigate.performed += ctx => pauseMenu.Navigate(ctx);
        uiInput.UI.Navigate.performed += ctx => languageMenu.Navigate(ctx);
        uiInput.UI.Navigate.performed += ctx => modeMenu.Navigate(ctx);

        uiInput.UI.Navigate.started += ctx => creditScroll.Navigate(ctx);
        uiInput.UI.Navigate.canceled += ctx => creditScroll.Navigate(ctx);

        uiInput.UI.Cancel.performed += ctx => modeMenu.Cancel(ctx);
        uiInput.UI.Cancel.performed += ctx => optionsMenu.Cancel(ctx);
        uiInput.UI.Cancel.performed += ctx => videoMenu.Cancel(ctx);
        uiInput.UI.Cancel.performed += ctx => audioMenu.Cancel(ctx);
        uiInput.UI.Cancel.performed += ctx => pauseMenu.Cancel(ctx);
        uiInput.UI.Cancel.performed += ctx => languageMenu.Cancel(ctx);
        uiInput.UI.Cancel.performed += ctx => creditScroll.Cancel(ctx);

        uiInput.UI.Pause.performed += ctx => TogglePause(ctx);

    }

    private void OnEnable()
    {
        if (uiInput != null)
        {
            uiInput.Enable();
        }
    }

    private void OnDisable()
    {
        if (uiInput != null)
        {
            uiInput.Disable();
        }
    }

    public void ToggleMainMenu(bool active)
    {
        mainMenu.gameObject.SetActive(active);
    }

    public void ToggleOptionsMenu(bool active)
    {
        optionsMenu.gameObject.SetActive(active);
    }

    public void ToggleVideoMenu(bool active)
    {
        videoMenu.gameObject.SetActive(active);
    }

    public void ToggleAudioMenu(bool active)
    {
        audioMenu.gameObject.SetActive(active);
    }

    public void ToggleCreditMenu(bool active)
    {
        creditScroll.gameObject.SetActive(active);
    }

    public void ToggleLanguageMenu(bool active)
    {
        languageMenu.gameObject.SetActive(active);
    }

    public void ToggleModeMenu(bool active, bool isMultiplayer = false)
    {
        modeMenu.ToggleMultiplayer(isMultiplayer);
        modeMenu.gameObject.SetActive(active);
    }

    public void ToggleScore(bool active)
    {
        score.ResetScore();
        score.ResetTimer();
        score.StartTimer();
        score.gameObject.SetActive(active);
    }

    public void TogglePause(InputAction.CallbackContext context)
    {
        if (!gameObject.activeInHierarchy)
            return;

        var phase = context.phase;
        if (phase != InputActionPhase.Performed)
            return;

        if (GameManager.instance.CurrentState != GameState.Play)
            return;

        try
        {
            if (FindObjectOfType<WinscreenAnim>().gameObject.activeInHierarchy)
                return;
        }
        catch
        {

        }

        if (!pauseMenu.gameObject.activeInHierarchy)
        {
            if (Time.timeScale == 0)
                return;

            pauseMenu.gameObject.SetActive(true);
            Time.timeScale = 0f;
            return;
        }

        if (pauseMenu.gameObject.activeInHierarchy)
        {
            pauseMenu.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void TogglePause(bool shouldPause)
    {
        if (!gameObject.activeInHierarchy)
            return;

        if (GameManager.instance.CurrentState != GameState.Play)
            return;

        try
        {
            if (FindObjectOfType<WinscreenAnim>().gameObject.activeInHierarchy)
                return;
        }
        catch
        {

        }

        if (shouldPause)
        {
            if (Time.timeScale == 0)
                return;

            pauseMenu.gameObject.SetActive(true);
            Time.timeScale = 0f;
            return;
        }
        else
        {
            pauseMenu.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }

    }

    public void Pause()
    {

        if (!gameObject.activeInHierarchy)
            return;

        if (GameManager.instance.CurrentState != GameState.Play)
            return;

        try
        {
            if (FindObjectOfType<WinscreenAnim>().gameObject.activeInHierarchy)
                return;
        }
        catch
        {

        }

        if (Time.timeScale == 0)
            return;

        pauseMenu.gameObject.SetActive(true);
        Time.timeScale = 0f;

    }

    //public void ToggleRestart(InputAction.CallbackContext context)
    //{
    //    if (!gameObject.activeInHierarchy)
    //        return;

    //    var phase = context.phase;
    //    if (phase != InputActionPhase.Performed)
    //        return;

    //    if (GameManager.instance.CurrentState != GameState.Play)
    //        return;

    //    if (!restartButton.gameObject.activeInHierarchy)
    //    {
    //        restartButton.gameObject.SetActive(true);
    //        return;
    //    }

    //    if (restartButton.gameObject.activeInHierarchy)
    //    {
    //        restartButton.RestartScene();
    //    }
    //}
}
