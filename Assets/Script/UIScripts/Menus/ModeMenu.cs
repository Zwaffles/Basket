using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ModeMenu : MonoBehaviour
{

    public string soloScene;
    public string multiBallScene;
    public string multiScene;

    private VisualElement root;

    private Button standardButton;
    private Button mayhemButton;

    private float ignoreInputTime;
    private bool inputEnabled;

    private GameManager gameManager;

    private void OnEnable()
    {
        inputEnabled = false;

        root = GetComponent<UIDocument>().rootVisualElement;

        activeIndicator = root.Q<VisualElement>("ActiveIndicator");

        standardButton = root.Q<Button>("1PlayerPlayButton");
        mayhemButton = root.Q<Button>("2PlayerPlayButton");

        FocusFirstElement(standardButton);
        ignoreInputTime = Time.time + .25f;

        try
        {
            activeIndicator.style.backgroundImage = GameManager.instance.multiBallsMode ? filledCheckbox : emptyCheckbox;
        }
        catch
        {
            Debug.LogWarning("Noel");
            Debug.LogError("Actually GameManager hasn't loaded in yet ;)");
        }
    }

}
