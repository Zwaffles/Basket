using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization.Settings;
using UnityEngine.UIElements;

public enum Language
{
    English,
    Icelandic
}

public class LanguageMenu : MonoBehaviour
{

    private VisualElement root;

    private VisualElement menuLanguage;

    private TextElement menuLanguageText;

    private Button confirmButton;
    private Button defaultButton;

    private float ignoreInputTime;
    private bool inputEnabled;

    private const Language defaultLanguage = Language.English;
    private Language currentLanguage = defaultLanguage;

    private void OnEnable()
    {
        inputEnabled = false;

        root = GetComponent<UIDocument>().rootVisualElement;

        menuLanguage = root.Q<VisualElement>("DisplayModeBox");

        menuLanguageText = root.Q<TextElement>("DisplayModeText");

        confirmButton = root.Q<Button>("Confirm");
        defaultButton = root.Q<Button>("Default");

        FocusFirstElement(menuLanguage);
        ignoreInputTime = Time.time + .25f;

        InitializeVideoMenu();
    }

    private void InitializeVideoMenu()
    {

        currentLanguage = GetLanguageFromLocale(PlayerPrefs.GetString("selected-locale"));

        switch (currentLanguage)
        {
            case Language.English:
                menuLanguageText.text = "English";
                break;
            case Language.Icelandic:
                menuLanguageText.text = "Íslenska";
                break;
        }

    }

    public void FocusFirstElement(VisualElement firstElement)
    {
        firstElement.Focus();
    }

    private void Update()
    {
        if (Time.time > ignoreInputTime)
        {
            inputEnabled = true;
        }
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

            PlayerPrefs.SetString("selected-locale", GetLocaleFromLanguage(currentLanguage));
            SetLocaleFromLanguage(currentLanguage);

            GameManager.instance.uiManager.ToggleOptionsMenu(true);
            gameObject.SetActive(false);
        }

        if (focusedElement == defaultButton)
        {
            currentLanguage = defaultLanguage;

            menuLanguageText.text = "English";

            PlayerPrefs.SetString("selected-locale", GetLocaleFromLanguage(currentLanguage));
            SetLocaleFromLanguage(currentLanguage);
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

            if (focusedElement == menuLanguage)
            {
                confirmButton.Focus();
            }

            if (focusedElement == confirmButton)
            {
                menuLanguage.Focus();
            }

            if (focusedElement == defaultButton)
            {
                menuLanguage.Focus();
            }
        }

        if (context.ReadValue<Vector2>() == Vector2.down)
        {
            var focusedElement = GetFocusedElement();

            if (focusedElement == menuLanguage)
            {
                confirmButton.Focus();
            }

            if (focusedElement == confirmButton)
            {
                menuLanguage.Focus();
            }

            if (focusedElement == defaultButton)
            {
                menuLanguage.Focus();
            }
        }

        if (context.ReadValue<Vector2>() == Vector2.left)
        {
            var focusedElement = GetFocusedElement();

            if (focusedElement == menuLanguage)
            {
                switch (currentLanguage)
                {
                    case Language.English:
                        menuLanguageText.text = "Íslenska";
                        currentLanguage = Language.Icelandic;
                        return;
                    case Language.Icelandic:
                        menuLanguageText.text = "English";
                        currentLanguage = Language.English;
                        return;
                }

                PlayerPrefs.SetString("selected-locale", GetLocaleFromLanguage(currentLanguage));
                SetLocaleFromLanguage(currentLanguage);

            }

            if (focusedElement == defaultButton)
            {
                confirmButton.Focus();
            }
        }

        if (context.ReadValue<Vector2>() == Vector2.right)
        {
            var focusedElement = GetFocusedElement();

            if (focusedElement == menuLanguage)
            {
                switch (currentLanguage)
                {
                    case Language.English:
                        menuLanguageText.text = "Íslenska";
                        currentLanguage = Language.Icelandic;
                        return;
                    case Language.Icelandic:
                        menuLanguageText.text = "English";
                        currentLanguage = Language.English;
                        return;
                }

                PlayerPrefs.SetString("selected-locale", GetLocaleFromLanguage(currentLanguage));
                SetLocaleFromLanguage(currentLanguage);

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

    private string GetLocaleFromLanguage(Language language)
    {
        switch (language)
        {
            case Language.English:
                return "en";
            case Language.Icelandic:
                return "is";
            default:
                return "en";
        }
    }

    private Language GetLanguageFromLocale(string locale)
    {
        switch (locale)
        {
            case "en":
                return Language.English;
            case "is":
                return Language.Icelandic;
            default:
                return Language.English;
        }
    }

    private void SetLocaleFromLanguage(Language language)
    {
        switch (language)
        {
            case Language.English:
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
                return;
            case Language.Icelandic:
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
                return;
            default:

                return;
        }
    }

}
