using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class CreditScroll : MonoBehaviour
{
    private VisualElement root;

    [SerializeField, Header("Scroll"), Tooltip("How fast the text should scroll."), Range(0f, 1000f)]
    private float scrollSpeed = 100f;
    [SerializeField, Tooltip("How fast the text should scroll when reversing."), Range(0f, 1000f)]
    private float reverseScrollSpeed = 50f;
    [SerializeField, Tooltip("How fast the text should scroll when fast forwarding."), Range(0f, 1000f)]
    private float fastForwardScrollSpeed = 200f;
    [SerializeField, Tooltip("How far the text should scroll."), Range(0f, 10000f)]
    private float maxScroll = 2150f;
    [SerializeField, Tooltip("How long the credits should stay at the top before scrolling"), Range(0f, 2f)]
    private float creditsHoldTimeTop = .5f;
    [SerializeField, Tooltip("How long the credits should stay at the bottom before auto-exiting"), Range(0f, 2f)]
    private float creditsHoldTimeBottom = .5f;

    private VisualElement visualElement;

    private float enabledTime;
    private bool inputEnabled;

    private float endTime;
    private bool isEnding;

    private float currentScrollSpeed;

    private void OnEnable()
    {
        inputEnabled = false;
        isEnding = false;
        currentScrollSpeed = scrollSpeed;

        root = GetComponent<UIDocument>().rootVisualElement;

        visualElement = root.Q<VisualElement>("Credits");

        enabledTime = Time.time;
    }

    private void Update()
    {

        if (!gameObject.activeInHierarchy)
            return;

        if (Time.time > enabledTime + .25f)
        {
            inputEnabled = true;
        }

        if (Time.time < enabledTime + creditsHoldTimeTop) 
            return;

        float newYPosition = visualElement.transform.position.y - currentScrollSpeed * Time.deltaTime;
        newYPosition = newYPosition > 0f ? 0f : newYPosition;

        if (Mathf.Abs(newYPosition) >= maxScroll)
        {
            newYPosition = -maxScroll;
            handleCreditEnd();
        }
        else
        {
            isEnding = false;
        }

        visualElement.transform.position = new Vector3(
            0f,
            newYPosition,
            0f
        );

    }

    private void handleCreditEnd()
    {

        if (!isEnding)
        {
            endTime = Time.time;
            isEnding = true;
            return;
        }

        if (Time.time < endTime + creditsHoldTimeBottom)
            return;

        GameManager.instance.uiManager.ToggleOptionsMenu(true);
        gameObject.SetActive(false);

    }

    // Press enter to exit
    public void Submit(InputAction.CallbackContext context)
    {
        if (!inputEnabled)
            return;

        if (!gameObject.activeInHierarchy)
            return;

        var phase = context.phase;
        if (phase != InputActionPhase.Performed)
            return;

        GameManager.instance.uiManager.ToggleOptionsMenu(true);
        gameObject.SetActive(false);
    }

    public void Navigate(InputAction.CallbackContext context)
    {
        if (!gameObject.activeInHierarchy)
            return;

        var phase = context.phase;

        if (phase == InputActionPhase.Started)
        {

            if (context.ReadValue<Vector2>() == Vector2.up)
            {
                currentScrollSpeed = -reverseScrollSpeed;
            }
            else if (context.ReadValue<Vector2>() == Vector2.down)
            {
                currentScrollSpeed = fastForwardScrollSpeed;
            }

        }

        if (context.canceled != true)
            return;

        currentScrollSpeed = scrollSpeed;

    }

    public void Cancel(InputAction.CallbackContext context)
    {

        if (!gameObject.activeInHierarchy)
            return;

        var phase = context.phase;
        if (phase != InputActionPhase.Performed)
            return;

        GameManager.instance.uiManager.ToggleOptionsMenu(true);
        gameObject.SetActive(false);

    }

}
