using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public int player1Score;
    public int player2Score;
    public TextMeshProUGUI player1ScoreText;
    public TextMeshProUGUI player2ScoreText;
    [SerializeField] Animator doublePointsAnim;
    UIManager uiManager;
    
    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
    }
    public void PlayerOneScore(int value)
    {
        GameManager.instance.audioManager.PlaySfx("airhorn", Random.Range(0.62f, 1.18f));

        player1Score += value;
        if (player1Score < 10)
        uiManager.FindAndSetText(uiManager.myVisualTreeAsset.CloneTree(), "UI-ScoreRight-Text", "0" + (player1Score + 1).ToString());
        else if(player1Score > 9)
        {
            uiManager.FindAndSetText(uiManager.myVisualTreeAsset.CloneTree(), "UI-ScoreRight-Text", "0" + (player1Score + 1).ToString());
        }
                 
    }
    public void PlayerTwoScore(int value)
    {
        GameManager.instance.audioManager.PlaySfx("airhorn", Random.Range(0.62f, 1.18f));

        player2Score += value;
        if (player2Score < 10)
            uiManager.FindAndSetText(uiManager.myVisualTreeAsset.CloneTree(), "UI-ScoreLeft-Text", "0" + (player2Score + 1).ToString());
        else if(player2Score > 9)
        {
            uiManager.FindAndSetText(uiManager.myVisualTreeAsset.CloneTree(), "UI-ScoreLeft-Text", "0" + (player2Score + 1).ToString());
        }
            
    }

    private void Update()
    {
        player1ScoreText.text = player1Score.ToString();
        player2ScoreText.text = player2Score.ToString();
    }
    public void ShowDoublePointsText()
    {
        doublePointsAnim.SetTrigger("DoublePointsTrigger");
    }
}
