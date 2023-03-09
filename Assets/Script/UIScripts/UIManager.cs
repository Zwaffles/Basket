using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public VisualTreeAsset myVisualTreeAsset;
    public UIDocument UIDoc;

    void Start()
    {
        VisualElement rootElement = myVisualTreeAsset.CloneTree();
        FindAndSetText(rootElement, "Number", "05:00");
        UIDoc.rootVisualElement.RemoveAt(0);
        UIDoc.rootVisualElement.Insert(0, rootElement);
    }

    void FindAndSetText(VisualElement element, string elementName, string textValue)
    {
        foreach (VisualElement childElement in element.Children())
        {
            if (childElement is TextElement textElement && textElement.name == elementName)
            {
                textElement.text = textValue;
                return;
            }
            else if (childElement.childCount > 0)
            {
                FindAndSetText(childElement, elementName, textValue);
            }
        }
    }
}
