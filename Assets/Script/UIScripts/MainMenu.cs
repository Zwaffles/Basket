using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEditor;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    private VisualElement root;

    private Button soloButton;
    private Button versusButton;
    private Button optionsButton;
    private Button quitButton;

    public string soloScene;

    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        soloButton = root.Q<Button>("1PlayerPlayButton");
        versusButton = root.Q<Button>("2PlayerPlayButton");
        optionsButton = root.Q<Button>("OptionsButton");
        quitButton = root.Q<Button>("QuitButton");
    }

    private void Start()
    {
        FocusFirstElement(soloButton);
    }

    public void FocusFirstElement(VisualElement firstElement)
    {
        firstElement.Focus();
    }

    public void Submit(InputAction.CallbackContext context)
    {
        var phase = context.phase;
        if (phase != InputActionPhase.Performed)
            return;

        var focusedElement = GetFocusedElement();

        if(focusedElement == soloButton)
        {
            GameManager.instance.InitializeScene(soloScene);
        }

        if(focusedElement == versusButton)
        {
            //do some other stuff
            Debug.LogWarning("No multiplayer yet :(");
        }

        if(focusedElement == optionsButton)
        {
            //do some OPTIONS stuff
            Debug.LogWarning("No options menu yet :(");
        }

        if(focusedElement == quitButton)
        {
            //quit game, duh
            Application.Quit();
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
        var oldScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(menu.soloScene);

        serializedObject.Update();

        EditorGUI.BeginChangeCheck();
        var newScene = EditorGUILayout.ObjectField("Solo Scene", oldScene, typeof(SceneAsset), false) as SceneAsset;

        if (EditorGUI.EndChangeCheck())
        {
            var newPath = AssetDatabase.GetAssetPath(newScene);
            var scenePathProperty = serializedObject.FindProperty("soloScene");
            scenePathProperty.stringValue = newPath;
        }
        serializedObject.ApplyModifiedProperties();
    }
}
#endif