using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements"), SerializeField]
    private GameObject mainMenu;

    public void ToggleMainMenu(bool active)
    {
        mainMenu.SetActive(active);
    }
}
