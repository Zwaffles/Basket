using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public enum FramerateCap
{
    FPS30,
    FPS60,
    FPS90,
    FPS120,
    Unlimited,
}

public class VideoMenu : MonoBehaviour
{
    private VisualElement root;

    private VisualElement displayMode;
    private VisualElement resolution;
    private VisualElement framerateCap;
    private VisualElement vsync;

    private TextElement displayModeText;
    private TextElement resolutionText;
    private TextElement framerateCapText;
    private TextElement vsyncText;

    private Button confirmButton;
    private Button defaultButton;

    private float ignoreInputTime;
    private bool inputEnabled;

    private const FullScreenMode defaultScreenMode = FullScreenMode.FullScreenWindow;
    private FullScreenMode currentScreenMode = defaultScreenMode;

    private Resolution[] resolutions;
    private List<string> resolutionOptions = new List<string>();
    private int currentResolutionIndex = 0;

    private const FramerateCap defaultFramerateCap = FramerateCap.Unlimited;
    private FramerateCap currentFramerateCap = defaultFramerateCap;

    private const bool defaultVSyncEnabled = true;
    private bool vSyncEnabled = defaultVSyncEnabled;

    private void OnEnable()
    {
        inputEnabled = false;

        root = GetComponent<UIDocument>().rootVisualElement;

        displayMode = root.Q<VisualElement>("DisplayModeBox");
        resolution = root.Q<VisualElement>("ResolutionBox");
        framerateCap = root.Q<VisualElement>("FramerateCapBox");
        vsync = root.Q<VisualElement>("VSyncBox");

        displayModeText = root.Q<TextElement>("DisplayModeText");
        resolutionText = root.Q<TextElement>("ResolutionText");
        framerateCapText = root.Q<TextElement>("FramerateCapText");
        vsyncText = root.Q<TextElement>("VSyncText");

        confirmButton = root.Q<Button>("Confirm");
        defaultButton = root.Q<Button>("Default");

        FocusFirstElement(displayMode);
        ignoreInputTime = Time.time + .25f;

        InitializeVideoMenu();
    }

    private void InitializeVideoMenu()
    {
        // Get supported resolutions and add them to the list
        resolutions = Screen.resolutions;
        for (int i = 0; i < resolutions.Length; i++)
        {
            Resolution resolution = resolutions[i];

            string resolutionOption = resolution.width + " x " + resolution.height;
            resolutionOptions.Add(resolutionOption);

            if (resolution.width == Screen.currentResolution.width && resolution.height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
                resolutionText.text = resolutionOptions[i];
            }
        }

        switch (currentScreenMode)
        {
            case FullScreenMode.ExclusiveFullScreen:
                displayModeText.style.fontSize = 32;
                displayModeText.text = "Fullscreen";
                break;
            case FullScreenMode.FullScreenWindow:
                displayModeText.style.fontSize = 32;
                displayModeText.text = "Borderless Windowed";
                break;
            case FullScreenMode.Windowed:
                displayModeText.text = "Windowed";
                break;
        }

        resolutionText.text = resolutionOptions[currentResolutionIndex];

        switch (currentFramerateCap)
        {
            case FramerateCap.FPS30:
                framerateCapText.text = "30 fps";
                break;
            case FramerateCap.FPS60:
                framerateCapText.text = "60 fps";
                break;
            case FramerateCap.FPS90:
                framerateCapText.text = "90 fps";
                break;
            case FramerateCap.FPS120:
                framerateCapText.text = "120 fps";
                break;
            case FramerateCap.Unlimited:
                framerateCapText.text = "Unlimited";
                break;
        }

        vsyncText.text = vSyncEnabled ? "Enabled" : "Disabled";
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

        if (focusedElement == confirmButton)
        {
            Screen.SetResolution(resolutions[currentResolutionIndex].width, resolutions[currentResolutionIndex].height, currentScreenMode);

            switch (currentFramerateCap)
            {
                case FramerateCap.FPS30:
                    Application.targetFrameRate = 30;
                    break;
                case FramerateCap.FPS60:
                    Application.targetFrameRate = 60;
                    break;
                case FramerateCap.FPS90:
                    Application.targetFrameRate = 90;
                    break;
                case FramerateCap.FPS120:
                    Application.targetFrameRate = 120;
                    break;
                case FramerateCap.Unlimited:
                    Application.targetFrameRate = -1;
                    break;
            }

            QualitySettings.vSyncCount = vSyncEnabled ? 1 : 0;

            GameManager.instance.uiManager.ToggleOptionsMenu(true);
            gameObject.SetActive(false);
        }

        if (focusedElement == defaultButton)
        {
            currentScreenMode = defaultScreenMode;
            currentResolutionIndex = resolutions.Length - 1;
            currentFramerateCap = defaultFramerateCap;
            vSyncEnabled = defaultVSyncEnabled;

            displayModeText.style.fontSize = 25;
            displayModeText.text = "Borderless Windowed";
            resolutionText.text = resolutionOptions[resolutionOptions.Count - 1];
            framerateCapText.text = "Unlimited";
            vsyncText.text = "Enabled";
        }
    }

    public void Navigate(InputAction.CallbackContext context)
    {
        if (!gameObject.activeInHierarchy)
            return;

        var phase = context.phase;
        if (phase != InputActionPhase.Performed)
            return;

        try
        {
            // New
            GameManager.instance.audioManager.PlaySfx("Pop Sound 1");

        }
        catch
        {
            Debug.LogWarning("AudioManager not found. Perhaps you're not using the GameManager prefab?");
        }

        if (context.ReadValue<Vector2>() == Vector2.up)
        {
            var focusedElement = GetFocusedElement();

            if (focusedElement == displayMode)
            {
                confirmButton.Focus();
            }

            if (focusedElement == resolution)
            {
                displayMode.Focus();
            }

            if (focusedElement == framerateCap)
            {
                resolution.Focus();
            }

            if (focusedElement == vsync)
            {
                framerateCap.Focus();
            }

            if (focusedElement == confirmButton)
            {
                vsync.Focus();
            }

            if (focusedElement == defaultButton)
            {
                vsync.Focus();
            }
        }

        if (context.ReadValue<Vector2>() == Vector2.down)
        {
            var focusedElement = GetFocusedElement();

            if (focusedElement == displayMode)
            {
                resolution.Focus();
            }

            if (focusedElement == resolution)
            {
                framerateCap.Focus();
            }

            if (focusedElement == framerateCap)
            {
                vsync.Focus();
            }

            if (focusedElement == vsync)
            {
                confirmButton.Focus();
            }

            if (focusedElement == confirmButton)
            {
                displayMode.Focus();
            }

            if (focusedElement == defaultButton)
            {
                displayMode.Focus();
            }
        }

        if (context.ReadValue<Vector2>() == Vector2.left)
        {
            var focusedElement = GetFocusedElement();

            if (focusedElement == displayMode)
            {
                switch (currentScreenMode)
                {
                    case FullScreenMode.ExclusiveFullScreen:
                        displayModeText.text = "Windowed";
                        currentScreenMode = FullScreenMode.Windowed;
                        return;
                    case FullScreenMode.FullScreenWindow:
                        displayModeText.style.fontSize = 32;
                        displayModeText.text = "Fullscreen";
                        currentScreenMode = FullScreenMode.ExclusiveFullScreen;
                        return;
                    case FullScreenMode.Windowed:
                        displayModeText.style.fontSize = 25;
                        displayModeText.text = "Borderless Windowed";
                        currentScreenMode = FullScreenMode.FullScreenWindow;
                        return;
                }
            }

            if (focusedElement == resolution)
            {
                currentResolutionIndex = (currentResolutionIndex - 1 + resolutions.Length) % resolutions.Length;
                resolutionText.text = resolutionOptions[currentResolutionIndex];
            }

            if (focusedElement == framerateCap)
            {
                switch (currentFramerateCap)
                {
                    case FramerateCap.FPS30:
                        framerateCapText.text = "Unlimited";
                        currentFramerateCap = FramerateCap.Unlimited;
                        return;
                    case FramerateCap.FPS60:
                        framerateCapText.text = "30 fps";
                        currentFramerateCap = FramerateCap.FPS30;
                        return;
                    case FramerateCap.FPS90:
                        framerateCapText.text = "60 fps";
                        currentFramerateCap = FramerateCap.FPS60;
                        return;
                    case FramerateCap.FPS120:
                        framerateCapText.text = "90 fps";
                        currentFramerateCap = FramerateCap.FPS90;
                        return;
                    case FramerateCap.Unlimited:
                        framerateCapText.text = "120 fps";
                        currentFramerateCap = FramerateCap.FPS120;
                        return;
                }
            }

            if (focusedElement == vsync)
            {
                vSyncEnabled = !vSyncEnabled;
                vsyncText.text = vSyncEnabled ? "Enabled" : "Disabled";
            }

            if (focusedElement == defaultButton)
            {
                confirmButton.Focus();
            }
        }

        if (context.ReadValue<Vector2>() == Vector2.right)
        {
            var focusedElement = GetFocusedElement();

            if (focusedElement == displayMode)
            {
                switch (currentScreenMode)
                {
                    case FullScreenMode.ExclusiveFullScreen:
                        displayModeText.style.fontSize = 25;
                        displayModeText.text = "Borderless Windowed";
                        currentScreenMode = FullScreenMode.FullScreenWindow;
                        return;
                    case FullScreenMode.FullScreenWindow:
                        displayModeText.style.fontSize = 32;
                        displayModeText.text = "Windowed";
                        currentScreenMode = FullScreenMode.Windowed;
                        return;
                    case FullScreenMode.Windowed:
                        displayModeText.text = "Fullscreen";
                        currentScreenMode = FullScreenMode.ExclusiveFullScreen;
                        return;
                }
            }

            if (focusedElement == resolution)
            {
                currentResolutionIndex = (currentResolutionIndex + 1) % resolutions.Length;
                resolutionText.text = resolutionOptions[currentResolutionIndex];
            }

            if (focusedElement == framerateCap)
            {
                switch (currentFramerateCap)
                {
                    case FramerateCap.FPS30:
                        framerateCapText.text = "60 fps";
                        currentFramerateCap = FramerateCap.FPS60;
                        return;
                    case FramerateCap.FPS60:
                        framerateCapText.text = "90 fps";
                        currentFramerateCap = FramerateCap.FPS90;
                        return;
                    case FramerateCap.FPS90:
                        framerateCapText.text = "120 fps";
                        currentFramerateCap = FramerateCap.FPS120;
                        return;
                    case FramerateCap.FPS120:
                        framerateCapText.text = "Unlimited";
                        currentFramerateCap = FramerateCap.Unlimited;
                        return;
                    case FramerateCap.Unlimited:
                        framerateCapText.text = "30 fps";
                        currentFramerateCap = FramerateCap.FPS30;
                        return;
                }
            }

            if (focusedElement == vsync)
            {
                vSyncEnabled = !vSyncEnabled;
                vsyncText.text = vSyncEnabled ? "Enabled" : "Disabled";
            }

            if (focusedElement == confirmButton)
            {
                defaultButton.Focus();
            }
        }
    }

    public Focusable GetFocusedElement()
    {
        return root.focusController.focusedElement;
    }
}
