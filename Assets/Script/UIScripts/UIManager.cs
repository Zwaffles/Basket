using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public VisualTreeAsset myVisualTreeAsset;
    public UIDocument UIDoc;
    Label labelElement;
    void Start()
    {
        VisualElement rootElement = myVisualTreeAsset.CloneTree();
        UIDoc.rootVisualElement.RemoveAt(0);
        FindAndSetText(rootElement, "Number", "05:00");
        
        
    }

    void FindAndSetText(VisualElement rootElement,string labelName, string textValue)
    {
        foreach (VisualElement childElement in rootElement.Children())
        {
            
            if (childElement.Q<Label>().name == "Number")
            {
                childElement.Q<Label>().text = textValue;
                UIDoc.rootVisualElement.Insert(0, childElement);
                return;
            }
            else if (childElement.childCount > 0)
            {
                FindAndSetText(childElement, labelName, textValue);
            }
        }
    }
}
