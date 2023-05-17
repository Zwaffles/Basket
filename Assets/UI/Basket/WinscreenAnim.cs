using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class WinscreenAnim : MonoBehaviour
{
    private VisualElement _wincontainer;
    private VisualElement _wintexthidden;

    private const string POPUP_ANIMATION = "pop-animation-hide";
    
    private void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        _wincontainer = root.Q<VisualElement>("Container");
        _wintexthidden = root.Q<VisualElement>("WinImage");
        _wincontainer.RegisterCallback<TransitionEndEvent>(Container_TransitionEnd);
    }
    
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);
        _wincontainer.ToggleInClassList(POPUP_ANIMATION);
    }
    
    private void Container_TransitionEnd(TransitionEndEvent evt)
    {
        _wintexthidden.style.translate = new StyleTranslate(new Translate(0, 0, 0));
    }
}