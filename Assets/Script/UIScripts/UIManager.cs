using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements"), SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private ScoreManager score;

    private PlayerInput playerInput;

    private void OnEnable()
    {

    }

    public void ToggleMainMenu(bool active)
    {
        mainMenu.SetActive(active);
    }

    public void ToggleScore(bool active)
    {
        score.player1Score = 0; 
        score.player2Score = 0;
        score.ResetTimer();
        score.StartTimer();
        score.gameObject.SetActive(active);
    }
}
