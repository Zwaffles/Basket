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
                displayModeText.text = GetLocalizedVariant("Fullscreen");
                break;
            case FullScreenMode.FullScreenWindow:
                displayModeText.style.fontSize = 32;
                displayModeText.text = GetLocalizedVariant("Borderless Windowed");
                break;
            case FullScreenMode.Windowed:
                displayModeText.text = GetLocalizedVariant("Windowed");
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
                framerateCapText.text = GetLocalizedVariant("Unlimited");
                break;
        }

        vsyncText.text = vSyncEnabled ? GetLocalizedVariant("Enabled") : GetLocalizedVariant("Disabled");
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
            displayModeText.text = GetLocalizedVariant("Borderless Windowed");
            resolutionText.text = resolutionOptions[resolutionOptions.Count - 1];
            framerateCapText.text = GetLocalizedVariant("Unlimited");
            vsyncText.text = GetLocalizedVariant("Enabled");
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
                        displayModeText.text = GetLocalizedVariant("Windowed");
                        currentScreenMode = FullScreenMode.Windowed;
                        return;
                    case FullScreenMode.FullScreenWindow:
                        displayModeText.style.fontSize = 32;
                        displayModeText.text = GetLocalizedVariant("Fullscreen");
                        currentScreenMode = FullScreenMode.ExclusiveFullScreen;
                        return;
                    case FullScreenMode.Windowed:
                        displayModeText.style.fontSize = 25;
                        displayModeText.text = GetLocalizedVariant("Borderless Windowed");
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
                        framerateCapText.text = GetLocalizedVariant("Unlimited");
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
                vsyncText.text = vSyncEnabled ? GetLocalizedVariant("Enabled") : GetLocalizedVariant("Disabled");
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
                        displayModeText.text = GetLocalizedVariant("Borderless Windowed");
                        currentScreenMode = FullScreenMode.FullScreenWindow;
                        return;
                    case FullScreenMode.FullScreenWindow:
                        displayModeText.style.fontSize = 32;
                        displayModeText.text = GetLocalizedVariant("Windowed");
                        currentScreenMode = FullScreenMode.Windowed;
                        return;
                    case FullScreenMode.Windowed:
                        displayModeText.text = GetLocalizedVariant("Fullscreen");
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
                        framerateCapText.text = GetLocalizedVariant("Unlimited");
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
                vsyncText.text = vSyncEnabled ? GetLocalizedVariant("Enabled") : GetLocalizedVariant("Disabled");
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

    #region Localization

    private string GetLocalizedVariant(string english)
    {

        string localizedString = "UNDEFINED";

        switch (GetCurrentLanguage())
        {

            case Language.English:
                if (english == "Unlimited") localizedString = "Unlimited";
                if (english == "Enabled") localizedString = "Enabled";
                if (english == "Disabled") localizedString = "Disabled";
                if (english == "Borderless Windowed") localizedString = "Borderless Windowed";
                if (english == "Windowed") localizedString = "Windowed";
                if (english == "Fullscreen") localizedString = "Fullscreen";
                break;
            case Language.French:
                if (english == "Unlimited") localizedString = "Illimité";
                if (english == "Enabled") localizedString = "Activé";
                if (english == "Disabled") localizedString = "Désactivé";
                if (english == "Borderless Windowed") localizedString = "Fenêtré sans Bordure";
                if (english == "Windowed") localizedString = "En Fenêtre";
                if (english == "Fullscreen") localizedString = "Plein Écran";
                break;
            case Language.German:
                if (english == "Unlimited") localizedString = "Unbegrenzt";
                if (english == "Enabled") localizedString = "Aktiviert";
                if (english == "Disabled") localizedString = "Deaktiviert";
                if (english == "Borderless Windowed") localizedString = "Randloser Fenstermodus";
                if (english == "Windowed") localizedString = "Fenstermodus";
                if (english == "Fullscreen") localizedString = "Vollbild";
                break;
            case Language.Hawaiian:
                if (english == "Unlimited") localizedString = "'A'ole i kūpono";
                if (english == "Enabled") localizedString = "Ho'āla";
                if (english == "Disabled") localizedString = "Hō'ole";
                if (english == "Borderless Windowed") localizedString = "Ke a'o ana i ka puka 'ole 'ia";
                if (english == "Windowed") localizedString = "Ke a'o ana i ka puka";
                if (english == "Fullscreen") localizedString = "Ka maka nui";
                break;
            case Language.Italian:
                if (english == "Unlimited") localizedString = "Illimitato";
                if (english == "Enabled") localizedString = "Abilitato";
                if (english == "Disabled") localizedString = "Disabilitato";
                if (english == "Borderless Windowed") localizedString = "Finestra senza Bordi";
                if (english == "Windowed") localizedString = "Finestra";
                if (english == "Fullscreen") localizedString = "Schermo Intero";
                break;
            case Language.Polish:
                if (english == "Unlimited") localizedString = "Bez Limitu";
                if (english == "Enabled") localizedString = "Włączone";
                if (english == "Disabled") localizedString = "Wyłączone";
                if (english == "Borderless Windowed") localizedString = "Okno bez Obramowania";
                if (english == "Windowed") localizedString = "Okno";
                if (english == "Fullscreen") localizedString = "Pełny Ekran";
                break;
            case Language.Portuguese:
                if (english == "Unlimited") localizedString = "Ilimitado";
                if (english == "Enabled") localizedString = "Ativado";
                if (english == "Disabled") localizedString = "Desativado";
                if (english == "Borderless Windowed") localizedString = "Janela sem Bordas";
                if (english == "Windowed") localizedString = "Em Janela";
                if (english == "Fullscreen") localizedString = "Tela Cheia";
                break;
            case Language.Russian:
                if (english == "Unlimited") localizedString = "Без ограничений";
                if (english == "Enabled") localizedString = "Включено";
                if (english == "Disabled") localizedString = "Отключено";
                if (english == "Borderless Windowed") localizedString = "Оконный режим без границ";
                if (english == "Windowed") localizedString = "Оконный режим";
                if (english == "Fullscreen") localizedString = "Полноэкранный режим";
                break;
            case Language.Spanish:
                if (english == "Unlimited") localizedString = "Ilimitado";
                if (english == "Enabled") localizedString = "Habilitado";
                if (english == "Disabled") localizedString = "Deshabilitado";
                if (english == "Borderless Windowed") localizedString = "Ventana sin Bordes";
                if (english == "Windowed") localizedString = "En Ventana";
                if (english == "Fullscreen") localizedString = "Pantalla Completa";
                break;
            case Language.Turkish:
                if (english == "Unlimited") localizedString = "Sınırsız";
                if (english == "Enabled") localizedString = "Etkin";
                if (english == "Disabled") localizedString = "Devre dışı";
                if (english == "Borderless Windowed") localizedString = "Kenarsız Pencere Modu";
                if (english == "Windowed") localizedString = "Pencere Modu";
                if (english == "Fullscreen") localizedString = "Tam Ekran";
                break;
            case Language.Ukrainian:
                if (english == "Unlimited") localizedString = "Необмежено";
                if (english == "Enabled") localizedString = "Увімкнено";
                if (english == "Disabled") localizedString = "Вимкнено";
                if (english == "Borderless Windowed") localizedString = "Режим вікна без меж";
                if (english == "Windowed") localizedString = "Віконний режим";
                if (english == "Fullscreen") localizedString = "Повноекранний режим";
                break;
            case Language.Chinese:
                if (english == "Unlimited") localizedString = "无限制";
                if (english == "Enabled") localizedString = "已启用";
                if (english == "Disabled") localizedString = "已禁用";
                if (english == "Borderless Windowed") localizedString = "无边框窗口化";
                if (english == "Windowed") localizedString = "窗口化";
                if (english == "Fullscreen") localizedString = "全屏";
                break;
            case Language.Japanese:
                if (english == "Unlimited") localizedString = "無制限";
                if (english == "Enabled") localizedString = "有効";
                if (english == "Disabled") localizedString = "無効";
                if (english == "Borderless Windowed") localizedString = "枠のないウィンドウモード";
                if (english == "Windowed") localizedString = "ウィンドウモード";
                if (english == "Fullscreen") localizedString = "フルスクリーン";
                break;
            case Language.Icelandic:
                if (english == "Unlimited") localizedString = "Takmörklaus";
                if (english == "Enabled") localizedString = "Virkt";
                if (english == "Disabled") localizedString = "Óvirkur";
                if (english == "Borderless Windowed") localizedString = "Skjárglas";
                if (english == "Windowed") localizedString = "Gluggastjórn";
                if (english == "Fullscreen") localizedString = "Fullskjár";
                break;
            default:
                if (english == "Unlimited") localizedString = "Unlimited";
                if (english == "Enabled") localizedString = "Enabled";
                if (english == "Disabled") localizedString = "Disabled";
                if (english == "Borderless Windowed") localizedString = "Borderless Windowed";
                if (english == "Windowed") localizedString = "Windowed";
                if (english == "Fullscreen") localizedString = "Fullscreen";
                break;
        }

        return localizedString;

    }

    private Language GetCurrentLanguage()
    {

        Language returnLanguage = Language.English;

        if (PlayerPrefs.GetString("selected-locale") != null)
        {
            switch (PlayerPrefs.GetString("selected-locale"))
            {
                case "en":
                    returnLanguage = Language.English;
                    break;
                case "fr":
                    returnLanguage = Language.French;
                    break;
                case "de":
                    returnLanguage = Language.German;
                    break;
                case "haw":
                    returnLanguage = Language.Hawaiian;
                    break;
                case "it":
                    returnLanguage = Language.Italian;
                    break;
                case "pl":
                    returnLanguage = Language.Polish;
                    break;
                case "pt-BR":
                    returnLanguage = Language.Portuguese;
                    break;
                case "ru":
                    returnLanguage = Language.Russian;
                    break;
                case "es":
                    returnLanguage = Language.Spanish;
                    break;
                case "tr":
                    returnLanguage = Language.Turkish;
                    break;
                case "uk":
                    returnLanguage = Language.Ukrainian;
                    break;
                case "zh-Hans":
                    returnLanguage = Language.Chinese;
                    break;
                case "ja":
                    returnLanguage = Language.Japanese;
                    break;
                case "is":
                    returnLanguage = Language.Icelandic;
                    break;
                default:
                    returnLanguage = Language.English;
                    break;
            }
        }

        return returnLanguage;

    }

#endregion

}
