using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEditor;
using System.Collections;
using System;

public class MainMenu : MonoBehaviour
{
    public string soloScene;
    public string multiBallScene;
    public string multiScene;

    private VisualElement root;

    private VisualElement activeIndicator;

    private Button soloButton;
    private Button versusButton;
    private Button optionsButton;
    private Button quitButton;

    private float ignoreInputTime;
    private bool inputEnabled;

    private GameManager gameManager;

    public Texture2D emptyCheckbox;
    public Texture2D filledCheckbox;

    private void OnEnable()
    {
        inputEnabled = false;

        root = GetComponent<UIDocument>().rootVisualElement;

        activeIndicator = root.Q<VisualElement>("ActiveIndicator");

        soloButton = root.Q<Button>("1PlayerPlayButton");
        versusButton = root.Q<Button>("2PlayerPlayButton");
        optionsButton = root.Q<Button>("OptionsButton");
        quitButton = root.Q<Button>("QuitButton");

        FocusFirstElement(soloButton);
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

    private void Start()
    {
        gameManager = GameManager.instance;
        gameManager.player1Left = true;
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

        if(focusedElement == soloButton)
        {
            if(gameManager.multiBallsMode)
                gameManager.InitializeScene(multiBallScene, isMultiplayer: false);
            else
                gameManager.InitializeScene(soloScene, isMultiplayer: false);
        }

        if(focusedElement == versusButton)
        {
            SceneManager.LoadScene(multiScene);
            Instantiate(GameManager.instance.uiManager.fadeToBlack, GameManager.instance.uiManager.transform);
            gameManager.StartMultiplayer();
            gameObject.SetActive(false);
        }

        if(focusedElement == optionsButton)
        {
            gameManager.uiManager.ToggleOptionsMenu(true);
            gameObject.SetActive(false);
        }

        if(focusedElement == quitButton)
        {
            //quit game, duh
            Application.Quit();
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

            if (focusedElement == soloButton)
            {
                quitButton.Focus();
            }

            if (focusedElement == versusButton)
            {
                soloButton.Focus();
            }

            if (focusedElement == optionsButton)
            {
                versusButton.Focus();
            }

            if (focusedElement == quitButton)
            {
                optionsButton.Focus();
            }

            try
            {
                // New
                gameManager.audioManager.PlaySfx("Pop Sound 1");

            }
            catch
            {
                Debug.LogWarning("AudioManager not found. Perhaps you're not using the GameManager prefab?");
            }

        }

        if (context.ReadValue<Vector2>() == Vector2.down)
        {
            var focusedElement = GetFocusedElement();

            if (focusedElement == soloButton)
            {
                versusButton.Focus();
            }

            if (focusedElement == versusButton)
            {
                optionsButton.Focus();
            }

            if (focusedElement == optionsButton)
            {
                quitButton.Focus();
            }

            if (focusedElement == quitButton)
            {
                soloButton.Focus();
            }

            try
            {
                // New
                gameManager.audioManager.PlaySfx("Pop Sound 1");

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

    public void ToggleGameMode(InputAction.CallbackContext context)
    {
        if (!gameObject.activeInHierarchy)
            return;

        var phase = context.phase;
        if (phase != InputActionPhase.Performed)
            return;

        if (GameManager.instance.CurrentState != GameState.Menu)
            return;

        if (GameManager.instance.multiBallsMode)
        {
            activeIndicator.style.backgroundImage = emptyCheckbox;
            GameManager.instance.multiBallsMode = false;
        }
        else
        {
            activeIndicator.style.backgroundImage = filledCheckbox;
            GameManager.instance.multiBallsMode = true;
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(MainMenu), true)]
public class MainMenuEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var menu = target as MainMenu;
        var oldSoloScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(menu.soloScene);
        var oldMultiBallScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(menu.multiBallScene);
        var oldMultiScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(menu.multiScene);

        serializedObject.Update();

        EditorGUI.BeginChangeCheck();
        var newSoloScene = EditorGUILayout.ObjectField("Solo Scene", oldSoloScene, typeof(SceneAsset), false) as SceneAsset;
        var newMultiBallScene = EditorGUILayout.ObjectField("Multi-Ball Scene", oldMultiBallScene, typeof(SceneAsset), false) as SceneAsset;
        var newMultiScene = EditorGUILayout.ObjectField("Multi Scene", oldMultiScene, typeof(SceneAsset), false) as SceneAsset;

        if (EditorGUI.EndChangeCheck())
        {
            var newSoloPath = AssetDatabase.GetAssetPath(newSoloScene);
            var soloScenePathProperty = serializedObject.FindProperty("soloScene");
            soloScenePathProperty.stringValue = newSoloPath;

            var newMultiBallPath = AssetDatabase.GetAssetPath(newMultiBallScene);
            var multiBallScenePathProperty = serializedObject.FindProperty("multiBallScene");
            multiBallScenePathProperty.stringValue = newMultiBallPath;

            var newMultiPath = AssetDatabase.GetAssetPath(newMultiScene);
            var multiScenePathProperty = serializedObject.FindProperty("multiScene");
            multiScenePathProperty.stringValue = newMultiPath;
        }

        var emptyCheckboxProperty = serializedObject.FindProperty("emptyCheckbox");
        EditorGUILayout.PropertyField(emptyCheckboxProperty);

        var filledCheckboxProperty = serializedObject.FindProperty("filledCheckbox");
        EditorGUILayout.PropertyField(filledCheckboxProperty);

        serializedObject.ApplyModifiedProperties();
    }
}
#endif