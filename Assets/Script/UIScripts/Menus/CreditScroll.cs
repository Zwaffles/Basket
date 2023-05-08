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
    [SerializeField, Tooltip("How far the text should scroll."), Range(0f, 10000f)]
    private float maxScroll = 2150f;

    private VisualElement visualElement;

    private float ignoreInputTime;
    private bool inputEnabled;

    private void OnEnable()
    {
        inputEnabled = false;

        root = GetComponent<UIDocument>().rootVisualElement;

        visualElement = root.Q<VisualElement>("Credits");

        ignoreInputTime = Time.time + .25f;
    }

    private void Update()
    {

        if (!gameObject.activeInHierarchy)
            return;

        if (Mathf.Abs(visualElement.transform.position.y) >= maxScroll) return;

        visualElement.transform.position = new Vector3(
            0f,
            visualElement.transform.position.y - scrollSpeed * Time.deltaTime,
            0f
        );

        if (Time.time > ignoreInputTime)
        {
            inputEnabled = true;
        }

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

}
