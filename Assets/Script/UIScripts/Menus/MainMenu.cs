using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEditor;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public string soloScene;
    public string multiScene;

    private VisualElement root;

    private Button soloButton;
    private Button versusButton;
    private Button optionsButton;
    private Button quitButton;

    private float ignoreInputTime;
    private bool inputEnabled;

    private void OnEnable()
    {
        inputEnabled = false;

        root = GetComponent<UIDocument>().rootVisualElement;

        soloButton = root.Q<Button>("1PlayerPlayButton");
        versusButton = root.Q<Button>("2PlayerPlayButton");
        optionsButton = root.Q<Button>("OptionsButton");
        quitButton = root.Q<Button>("QuitButton");

        FocusFirstElement(soloButton);
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

        if(focusedElement == soloButton)
        {
            GameManager.instance.InitializeScene(soloScene, isMultiplayer: false);
        }

        if(focusedElement == versusButton)
        {
            SceneManager.LoadScene(multiScene);
            GameManager.instance.StartMultiplayer();
            gameObject.SetActive(false);
        }

        if(focusedElement == optionsButton)
        {
            GameManager.instance.uiManager.ToggleOptionsMenu(true);
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
        }
    }

    public Focusable GetFocusedElement()
    {
        return root.focusController.focusedElement;
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
        var oldMultiScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(menu.multiScene);

        serializedObject.Update();

        EditorGUI.BeginChangeCheck();
        var newSoloScene = EditorGUILayout.ObjectField("Solo Scene", oldSoloScene, typeof(SceneAsset), false) as SceneAsset;
        var newMultiScene = EditorGUILayout.ObjectField("Multi Scene", oldMultiScene, typeof(SceneAsset), false) as SceneAsset;

        if (EditorGUI.EndChangeCheck())
        {
            var newSoloPath = AssetDatabase.GetAssetPath(newSoloScene);
            var soloScenePathProperty = serializedObject.FindProperty("soloScene");
            soloScenePathProperty.stringValue = newSoloPath;           
            
            var newMultiPath = AssetDatabase.GetAssetPath(newMultiScene);
            var multiScenePathProperty = serializedObject.FindProperty("multiScene");
            multiScenePathProperty.stringValue = newMultiPath;
        }
        serializedObject.ApplyModifiedProperties();
    }
}
#endif