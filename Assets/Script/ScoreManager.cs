using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public int player1Score;
    public int player2Score;
    public TextMeshProUGUI player1ScoreText;
    public TextMeshProUGUI player2ScoreText;
    private UIManager uiManager;

    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
    }
    public void PlayerOneScore(int value)
    {
        GameManager.instance.audioManager.PlaySfx("airhorn", Random.Range(0.62f, 1.18f));
        GameManager.instance.audioManager.PlaySfx("Goal_ClapClapClap", Random.Range(0.62f, 1.18f));
        GameManager.instance.audioManager.PlayMusic("Victory");

        player1Score += value;

        // Suppose to be updating UI Builder Score aka Halldor's UI
        if (player1Score < 10)
            uiManager.FindAndSetText(uiManager.myVisualTreeAsset.CloneTree(), "UI-ScoreLeft-Text", "0" + (player1Score).ToString());
    }
    public void PlayerTwoScore(int value)
    {
        GameManager.instance.audioManager.PlaySfx("airhorn", Random.Range(0.62f, 1.18f));
        GameManager.instance.audioManager.PlaySfx("Goal_ClapClapClap", Random.Range(0.62f, 1.18f));
        GameManager.instance.audioManager.PlayMusic("Match Time Running out");

        player2Score += value;
        // Suppose to be updating UI Builder Score aka Halldor's UI
        if (player2Score < 10)
            uiManager.FindAndSetText(uiManager.myVisualTreeAsset.CloneTree(), "UI-ScoreRight-Text", "0" + (player2Score).ToString());
    }

    private void Update()
    {
        // updating the basic score we currently have
        player1ScoreText.text = player1Score.ToString();
        player2ScoreText.text = player2Score.ToString();
    }
}
