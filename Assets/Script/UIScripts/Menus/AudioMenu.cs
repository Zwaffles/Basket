using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class AudioMenu : MonoBehaviour
{
    private VisualElement root;

    private SliderInt masterSlider;
    private SliderInt musicSlider;
    private SliderInt sfxSlider;
    private SliderInt voiceSlider;

    private TextElement masterAmount;
    private TextElement musicAmount;
    private TextElement sfxAmount;
    private TextElement voiceAmount;

    private Button confirmButton;
    private Button defaultButton;

    private float ignoreInputTime;
    private bool inputEnabled;

    private const int defaultSliderValue = 50;
    private int masterValue;
    private int musicValue;
    private int sfxValue;
    private int voiceValue;

    private void OnEnable()
    {
        inputEnabled = false;

        root = GetComponent<UIDocument>().rootVisualElement;

        masterSlider = root.Q<SliderInt>("MasterSlider");
        musicSlider = root.Q<SliderInt>("MusicSlider");
        sfxSlider = root.Q<SliderInt>("SFXSlider");
        voiceSlider = root.Q<SliderInt>("VoiceSlider");

        masterAmount = root.Q<TextElement>("MasterAmountText");
        musicAmount = root.Q<TextElement>("MusicAmountText");
        sfxAmount = root.Q<TextElement>("SFXAmountText");
        voiceAmount = root.Q<TextElement>("VoiceAmountText");

        confirmButton = root.Q<Button>("Confirm");
        defaultButton = root.Q<Button>("Default");

        FocusFirstElement(masterSlider);
        ignoreInputTime = Time.time + .25f;

        InitializeAudioMenu();
    }

    public void FocusFirstElement(VisualElement firstElement)
    {
        firstElement.Focus();
    }

    private void InitializeAudioMenu()
    {
        masterValue = (int)(GameManager.instance.audioManager.MasterVolume * 100);
        musicValue = (int)(GameManager.instance.audioManager.MusicVolume * 100);
        sfxValue = (int)(GameManager.instance.audioManager.SfxVolume * 100);
        voiceValue = (int)(GameManager.instance.audioManager.VoiceVolume * 100);

        masterSlider.value = masterValue;
        musicSlider.value = musicValue;
        sfxSlider.value = sfxValue;
        voiceSlider.value = voiceValue;

        masterAmount.text = masterValue.ToString();
        musicAmount.text = musicValue.ToString();
        sfxAmount.text = sfxValue.ToString();
        voiceAmount.text = voiceValue.ToString();
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
            GameManager.instance.audioManager.MasterVolume = (float)masterValue / 100;
            GameManager.instance.audioManager.MusicVolume = (float)musicValue / 100;
            GameManager.instance.audioManager.SfxVolume = (float)sfxValue / 100;
            GameManager.instance.audioManager.VoiceVolume = (float)voiceValue / 100;

            GameManager.instance.uiManager.ToggleOptionsMenu(true);
            gameObject.SetActive(false);
        }

        if (focusedElement == defaultButton)
        {
            masterSlider.value = defaultSliderValue;
            musicSlider.value = defaultSliderValue;
            sfxSlider.value = defaultSliderValue;
            voiceSlider.value = defaultSliderValue;

            masterAmount.text = defaultSliderValue.ToString();
            musicAmount.text = defaultSliderValue.ToString();
            sfxAmount.text = defaultSliderValue.ToString();
            voiceAmount.text = defaultSliderValue.ToString();
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

            if (focusedElement == masterSlider)
            {
                confirmButton.Focus();
            }

            if (focusedElement == musicSlider)
            {
                masterSlider.Focus();
            }

            if (focusedElement == sfxSlider)
            {
                musicSlider.Focus();
            }

            if (focusedElement == voiceSlider)
            {
                sfxSlider.Focus();
            }

            if (focusedElement == confirmButton)
            {
                voiceSlider.Focus();
            }

            if (focusedElement == defaultButton)
            {
                voiceSlider.Focus();
            }
        }

        if (context.ReadValue<Vector2>() == Vector2.down)
        {
            var focusedElement = GetFocusedElement();

            if (focusedElement == masterSlider)
            {
                musicSlider.Focus();
            }

            if (focusedElement == musicSlider)
            {
                sfxSlider.Focus();
            }

            if (focusedElement == sfxSlider)
            {
                voiceSlider.Focus();
            }

            if (focusedElement == voiceSlider)
            {
                confirmButton.Focus();
            }

            if (focusedElement == confirmButton)
            {
                masterSlider.Focus();
            }

            if (focusedElement == defaultButton)
            {
                masterSlider.Focus();
            }
        }

        if (context.ReadValue<Vector2>() == Vector2.left)
        {
            var focusedElement = GetFocusedElement();

            if (focusedElement == masterSlider)
            {
                masterValue = Math.Min(100, Math.Max(0, masterValue - 10));
                masterSlider.value = masterValue;
                masterAmount.text = masterValue.ToString();
            }

            if (focusedElement == musicSlider)
            {
                musicValue = Math.Min(100, Math.Max(0, musicValue - 10));
                musicSlider.value = musicValue;
                musicAmount.text = musicValue.ToString();
            }

            if (focusedElement == sfxSlider)
            {
                sfxValue = Math.Min(100, Math.Max(0, sfxValue - 10));
                sfxSlider.value = sfxValue;
                sfxAmount.text = sfxValue.ToString();
            }

            if (focusedElement == voiceSlider)
            {
                voiceValue = Math.Min(100, Math.Max(0, voiceValue - 10));
                voiceSlider.value = voiceValue;
                voiceAmount.text = voiceValue.ToString();
            }

            if (focusedElement == defaultButton)
            {
                confirmButton.Focus();
            }
        }

        if (context.ReadValue<Vector2>() == Vector2.right)
        {
            var focusedElement = GetFocusedElement();

            if (focusedElement == masterSlider)
            {
                masterValue = Math.Min(100, Math.Max(0, masterValue + 10));
                masterSlider.value = masterValue;
                masterAmount.text = masterValue.ToString();
            }

            if (focusedElement == musicSlider)
            {
                musicValue = Math.Min(100, Math.Max(0, musicValue + 10));
                musicSlider.value = musicValue;
                musicAmount.text = musicValue.ToString();
            }

            if (focusedElement == sfxSlider)
            {
                sfxValue = Math.Min(100, Math.Max(0, sfxValue + 10));
                sfxSlider.value = sfxValue;
                sfxAmount.text = sfxValue.ToString();
            }

            if (focusedElement == voiceSlider)
            {
                voiceValue = Math.Min(100, Math.Max(0, voiceValue + 10));
                voiceSlider.value = voiceValue;
                voiceAmount.text = voiceValue.ToString();
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
