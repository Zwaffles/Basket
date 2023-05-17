using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Fadetoblack : MonoBehaviour
{
    private VisualElement _blackscreen;

    private const string BLACKSCREEN = "BlackScreen";


    private void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        _blackscreen = root.Q<VisualElement>("BlackScreen");
        _blackscreen.RegisterCallback<TransitionEndEvent>(BlackScreen_TransitionEnd);
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);
        togglefade();
    }

    private void BlackScreen_TransitionEnd(TransitionEndEvent evt)
    {
        //Add stuff here that you want to run after the fade is done
    }

    private void togglefade()
    {
        //Requiers that _blackscreen already has the .BlackScreen from the style sheet to work
        _blackscreen.ToggleInClassList(BLACKSCREEN);
    }
}