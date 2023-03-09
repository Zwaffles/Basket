using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    // Reference to the Visual Tree asset in the Unity Editor
    public VisualTreeAsset myVisualTreeAsset;
    public UIDocument UIDoc;

    void Start()
    {
        // Load the Visual Tree asset from the Unity Editor
        VisualElement rootElement = myVisualTreeAsset.CloneTree();

        // Access the first child element in the VisualElement hierarchy
        var firstChildElement = rootElement.ElementAt(0) as VisualElement;
        if (firstChildElement != null)
        {
            // Find the Label element in the fourth child element
            var fourthChildElement = firstChildElement.ElementAt(0) as VisualElement;
            if (fourthChildElement != null)
            {
                var myLabelElement = fourthChildElement.Q<TextElement>("Text");
                if (myLabelElement != null)
                {
                    // Set a new text value for the Label element
                    myLabelElement.text = "Time \n 05:00";
                }
            }
        }

        // Replace the existing VisualElement hierarchy with the updated one
        UIDoc.rootVisualElement.RemoveAt(0);
        UIDoc.rootVisualElement.Insert(0, rootElement);
    }
}
