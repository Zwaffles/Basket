using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class OptionsMenu : MonoBehaviour
{
    private VisualElement root;

    private Button videoButton;
    private Button audioButton;
    private Button creditsButton;
    private Button returnButton;

    private float ignoreInputTime;
    private bool inputEnabled;

    private void OnEnable()
    {
        inputEnabled = false;

        root = GetComponent<UIDocument>().rootVisualElement;

        videoButton = root.Q<Button>("VideoButton");
        audioButton = root.Q<Button>("AudioButton");
        creditsButton = root.Q<Button>("CreditsButton");
        returnButton = root.Q<Button>("ReturnButton");

        FocusFirstElement(videoButton);
        ignoreInputTime = Time.time + .25f;
    }

    private void Update()
    {
        if (Time.time > ignoreInputTime)
        {
            inputEnabled = true;
        }
    }

    public void FocusFirstElement(VisualElement firstElement)
    {
        firstElement.Focus();
    }

    public void Submit(InputAction.CallbackContext context)
    {
        if (!inputEnabled)
            return;

        if (!gameObject.activeInHierarchy)
            return;

        var phase = context.phase;
        if (phase != InputActionPhase.Performed)
            return;

        var focusedElement = GetFocusedElement();

        if (focusedElement == videoButton)
        {
            GameManager.instance.uiManager.ToggleVideoMenu(true);
            gameObject.SetActive(false);
        }

        if (focusedElement == audioButton)
        {
            GameManager.instance.uiManager.ToggleAudioMenu(true);
            gameObject.SetActive(false);
        }

        if (focusedElement == creditsButton)
        {
            GameManager.instance.uiManager.ToggleCreditMenu(true);
            gameObject.SetActive(false);
            //Debug.LogWarning("Credits not yet implemented :(");
        }

        if (focusedElement == returnButton)
        {
            GameManager.instance.uiManager.ToggleMainMenu(true);
            gameObject.SetActive(false);
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

            if (focusedElement == videoButton)
            {
                returnButton.Focus();
            }

            if (focusedElement == audioButton)
            {
                videoButton.Focus();
            }

            if (focusedElement == creditsButton)
            {
                audioButton.Focus();
            }

            if (focusedElement == returnButton)
            {
                creditsButton.Focus();
            }
        }

        if (context.ReadValue<Vector2>() == Vector2.down)
        {
            var focusedElement = GetFocusedElement();

            if (focusedElement == videoButton)
            {
                audioButton.Focus();
            }

            if (focusedElement == audioButton)
            {
                creditsButton.Focus();
            }

            if (focusedElement == creditsButton)
            {
                returnButton.Focus();
            }

            if (focusedElement == returnButton)
            {
                videoButton.Focus();
            }
        }
    }

    public Focusable GetFocusedElement()
    {
        return root.focusController.focusedElement;
    }
}