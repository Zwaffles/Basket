using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PauseMenu : MonoBehaviour
{
    private VisualElement root;

    private Button resumeButton;
    private Button restartButton;
    private Button leaveButton;

    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        resumeButton = root.Q<Button>("Resume");
        restartButton = root.Q<Button>("Restart");
        leaveButton = root.Q<Button>("LeaveMatch");

        FocusFirstElement(resumeButton);
    }

    public void FocusFirstElement(VisualElement firstElement)
    {
        firstElement.Focus();
    }

    public void Submit(InputAction.CallbackContext context)
    {
        if (!gameObject.activeInHierarchy)
            return;

        var phase = context.phase;
        if (phase != InputActionPhase.Performed)
            return;

        var focusedElement = GetFocusedElement();

        if (focusedElement == resumeButton)
        {
            Resume();
        }

        if (focusedElement == restartButton)
        {
            Restart();
        }

        if (focusedElement == leaveButton)
        {
            LeaveMatch();
        }
    }

    public void Navigate(InputAction.CallbackContext context)
    {
        if (!gameObject.activeInHierarchy)
            return;

        var phase = context.phase;
        if (phase != InputActionPhase.Performed)
            return;

        if (context.ReadValue<Vector2>() == Vector2.up)
        {
            var focusedElement = GetFocusedElement();

            if (focusedElement == resumeButton)
            {
                leaveButton.Focus();
            }

            if (focusedElement == restartButton)
            {
                resumeButton.Focus();
            }

            if (focusedElement == leaveButton)
            {
                restartButton.Focus();
            }

            try
            {
                // New
                GameManager.instance.audioManager.PlaySfx("Pop Sound 1");

            }
            catch
            {
                Debug.LogWarning("AudioManager not found. Perhaps you're not using the GameManager prefab?");
            }

        }

        if (context.ReadValue<Vector2>() == Vector2.down)
        {
            var focusedElement = GetFocusedElement();

            if (focusedElement == resumeButton)
            {
                restartButton.Focus();
            }

            if (focusedElement == restartButton)
            {
                leaveButton.Focus();
            }

            if (focusedElement == leaveButton)
            {
                resumeButton.Focus();
            }

            try
            {
                // New
                GameManager.instance.audioManager.PlaySfx("Pop Sound 1");

            }
            catch
            {
                Debug.LogWarning("AudioManager not found. Perhaps you're not using the GameManager prefab?");
            }

        }
    }

    public Focusable GetFocusedElement()
    {
        return root.focusController.focusedElement;
    }

    public void Resume()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Cancel(InputAction.CallbackContext context)
    {

        gameObject.SetActive(false);
        Time.timeScale = 1f;

    }

    public void Restart()
    {

        GameManager.instance.uiManager.ToggleScore(true);
        GameManager.instance.audioManager.StopMusic();

        if (GameManager.instance.playerConfigurationManager.GetPlayerConfigurations().Count > 1)
            GameManager.instance.InitializeScene(SceneManager.GetActiveScene().name, isMultiplayer: true);
        else
            GameManager.instance.InitializeScene(SceneManager.GetActiveScene().name, isMultiplayer: false);

        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void LeaveMatch()
    {
        GameManager.instance.audioManager.StopMusic();
        gameObject.SetActive(false);
        Time.timeScale = 1f;
        GameManager.instance.StartMenu();
        SceneManager.LoadScene("NewMainMenu");
        Instantiate(GameManager.instance.uiManager.fadeToBlack, GameManager.instance.uiManager.transform);
    }
}
