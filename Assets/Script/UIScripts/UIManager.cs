using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public VisualTreeAsset myVisualTreeAsset;
    public UIDocument UIDoc;
    void Start()
    {
        VisualElement rootElement = myVisualTreeAsset.CloneTree();

        // Testing purposes
        FindAndSetText(rootElement, "UI-ScoreLeft-Text", "02");
        FindAndSetText(rootElement, "UI-ScoreRight-Text", "04");
    }

    public void FindAndSetText(VisualElement rootElement, string labelName, string textValue)
    {
        if (UIDoc.rootVisualElement.Q<Label>().name == labelName)
        {
            UIDoc.rootVisualElement.Q<Label>().text = textValue;
            return;
        }
    }
}
