using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements"), SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private ScoreManager score;

    public void ToggleMainMenu(bool active)
    {
        mainMenu.SetActive(active);
    }

    public void ToggleScore(bool active)
    {
        score.player1Score = 0; 
        score.player2Score = 0;
        score.gameObject.SetActive(active);
    }
}
