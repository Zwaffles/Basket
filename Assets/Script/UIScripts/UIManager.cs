
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public VisualTreeAsset myVisualTreeAsset;
    public UIDocument UIDoc;
    void Start()
    {
        VisualElement rootElement = myVisualTreeAsset.CloneTree();
        
    }

    public void FindAndSetText(VisualElement rootElement,string labelName, string textValue)
    {
        foreach (VisualElement childElement in rootElement.Children())
        {          
            if (childElement.Q<Label>().name == labelName)
            {
                UIDoc.rootVisualElement.Q<Label>().text = textValue;
                return;
            }
            else if (childElement.childCount > 0)
            {
                FindAndSetText(childElement, labelName, textValue);
            }
        }
    }
}
