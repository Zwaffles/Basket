using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements"), SerializeField]
    private MainMenu mainMenu;
    [SerializeField]
    private ScoreManager score;
    [SerializeField]
    private RestartButton restartButton;
    [SerializeField]
    private InputSelection inputSelection;

    private Inputaction uiInput;

    private void Awake()
    {
        uiInput = new Inputaction();

        uiInput.UI.Submit.performed += ctx => mainMenu.Submit(ctx);
        uiInput.UI.Cancel.performed += ctx => ToggleRestart(ctx);
        uiInput.UI.Navigate.performed += ctx => mainMenu.Navigate(ctx);
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

    public void ToggleScore(bool active)
    {
        score.ResetScore();
        score.ResetTimer();
        score.StartTimer();
        score.gameObject.SetActive(active);
    }

    public void ToggleRestart(InputAction.CallbackContext context)
    {
        if (!gameObject.activeInHierarchy)
            return;

        var phase = context.phase;
        if (phase != InputActionPhase.Performed)
            return;

        if (GameManager.instance.CurrentState != GameState.Play)
            return;

        if (!restartButton.gameObject.activeInHierarchy)
        {
            restartButton.gameObject.SetActive(true);
            return;
        }

        if (restartButton.gameObject.activeInHierarchy)
        {
            restartButton.RestartScene();
        }

    }

    public void AddPlayerIndex(int playerIndex)
    {
        inputSelection.AddPlayerIndexToList(playerIndex);
    }
}
