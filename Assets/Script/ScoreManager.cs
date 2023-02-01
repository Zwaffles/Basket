using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public int player1Score;
    public int player2Score;
    public TextMeshProUGUI player1ScoreText;
    public TextMeshProUGUI player2ScoreText;

    public void PlayerOneScore()
    {
        player1Score++;
    }
    public void PlayerTwoScore()
    {
        player2Score++;
    }

    private void Update()
    {
        player1ScoreText.text = player1Score.ToString();
        player2ScoreText.text = player2Score.ToString();

    }
}
