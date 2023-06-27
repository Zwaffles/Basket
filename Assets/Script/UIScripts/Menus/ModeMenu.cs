using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEditor;

public class ModeMenu : MonoBehaviour
{

    public string soloScene;
    public string multiBallScene;
    public string multiScene;

    private VisualElement root;

    private TextElement headerText;

    private Button standardButton;
    private Button mayhemButton;

    private float ignoreInputTime;
    private bool inputEnabled;

    private GameManager gameManager;

    private bool isMultiplayer = false;

    private void OnEnable()
    {
        inputEnabled = false;

        root = GetComponent<UIDocument>().rootVisualElement;

        standardButton = root.Q<Button>("StandardButton");
        mayhemButton = root.Q<Button>("MayhemButton");

        headerText = root.Q<TextElement>("HeaderText");

        if (isMultiplayer)
        {
            headerText.text = GetLocalizedVariant("Versus");
        }
        else
        {
            headerText.text = GetLocalizedVariant("Solo");
        }

        FocusFirstElement(standardButton);
        ignoreInputTime = Time.time + .25f;

    }

    private void Update()
    {
        if (Time.time > ignoreInputTime)
        {
            inputEnabled = true;
        }
    }

    private void Start()
    {
        gameManager = GameManager.instance;
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

        if (focusedElement == standardButton)
        {

            GameManager.instance.multiBallsMode = false;

            if (isMultiplayer)
            {
                SceneManager.LoadScene(multiScene);
                Instantiate(GameManager.instance.uiManager.fadeToBlack, GameManager.instance.uiManager.transform);
                gameManager.StartMultiplayer();
                gameObject.SetActive(false);
            }
            else
            {
                gameManager.InitializeScene(soloScene, isMultiplayer: false);
            }
                
        }

        if (focusedElement == mayhemButton)
        {

            GameManager.instance.multiBallsMode = true;

            if (isMultiplayer)
            {
                SceneManager.LoadScene(multiScene);
                Instantiate(GameManager.instance.uiManager.fadeToBlack, GameManager.instance.uiManager.transform);
                gameManager.StartMultiplayer();
                gameObject.SetActive(false);
            }
            else
            {
                gameManager.InitializeScene(multiBallScene, isMultiplayer: false);
            }

        }

    }

    public void Navigate(InputAction.CallbackContext context)
    {
        if (!gameObject.activeInHierarchy)
            return;

        var phase = context.phase;
        if (phase != InputActionPhase.Performed)
            return;

        if (context.ReadValue<Vector2>() == Vector2.left)
        {
            var focusedElement = GetFocusedElement();

            if (focusedElement == standardButton)
            {
                mayhemButton.Focus();
            }

            if (focusedElement == mayhemButton)
            {
                standardButton.Focus();
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

        if (context.ReadValue<Vector2>() == Vector2.right)
        {
            var focusedElement = GetFocusedElement();

            if (focusedElement == standardButton)
            {
                mayhemButton.Focus();
            }

            if (focusedElement == mayhemButton)
            {
                standardButton.Focus();
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

    public void ToggleMultiplayer(bool isMultiplayer)
    {
        
        this.isMultiplayer = isMultiplayer;

    }

    public void Cancel(InputAction.CallbackContext context)
    {

        if (!gameObject.activeInHierarchy)
            return;

        var phase = context.phase;
        if (phase != InputActionPhase.Performed)
            return;

        gameManager.uiManager.ToggleMainMenu(true);
        gameObject.SetActive(false);

    }

    #region Localization

    private string GetLocalizedVariant(string english)
    {

        string localizedString = "UNDEFINED";

        switch (GetCurrentLanguage())
        {

            case Language.English:
                if (english == "Solo") localizedString = "Solo";
                if (english == "Versus") localizedString = "Versus";
                break;
            case Language.French:
                if (english == "Solo") localizedString = "Solo";
                if (english == "Versus") localizedString = "Versus";
                break;
            case Language.German:
                if (english == "Solo") localizedString = "Allein";
                if (english == "Versus") localizedString = "Gegenüber";
                break;
            case Language.Hawaiian:
                if (english == "Solo") localizedString = "Hānau";
                if (english == "Versus") localizedString = "Kū'ikahi";
                break;
            case Language.Italian:
                if (english == "Solo") localizedString = "Singolo";
                if (english == "Versus") localizedString = "Contro";
                break;
            case Language.Polish:
                if (english == "Solo") localizedString = "Solo";
                if (english == "Versus") localizedString = "Przeciwko";
                break;
            case Language.Portuguese:
                if (english == "Solo") localizedString = "Solo";
                if (english == "Versus") localizedString = "Versus";
                break;
            case Language.Russian:
                if (english == "Solo") localizedString = "Соло";
                if (english == "Versus") localizedString = "Против";
                break;
            case Language.Spanish:
                if (english == "Solo") localizedString = "Solo";
                if (english == "Versus") localizedString = "Versus";
                break;
            case Language.Turkish:
                if (english == "Solo") localizedString = "Tekli";
                if (english == "Versus") localizedString = "Karşı";
                break;
            case Language.Ukrainian:
                if (english == "Solo") localizedString = "Соло";
                if (english == "Versus") localizedString = "Проти";
                break;
            case Language.Chinese:
                if (english == "Solo") localizedString = "单人";
                if (english == "Versus") localizedString = "对战";
                break;
            case Language.Japanese:
                if (english == "Solo") localizedString = "ソロ";
                if (english == "Versus") localizedString = "バーサス";
                break;
            case Language.Icelandic:
                if (english == "Solo") localizedString = "Einmanaleikur";
                if (english == "Versus") localizedString = "Andstæður";
                break;
            default:
                if (english == "Solo") localizedString = "Solo";
                if (english == "Versus") localizedString = "Versus";
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

#if UNITY_EDITOR
[CustomEditor(typeof(ModeMenu), true)]
public class ModeMenuEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var menu = target as ModeMenu;
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

        serializedObject.ApplyModifiedProperties();
    }
}
#endif