using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class ScoreManager : MonoBehaviour
{
    private VisualElement root;
    private TextElement player1ScoreText;
    private TextElement player2ScoreText;

    public int player1Score;
    public int player2Score;

    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        player1ScoreText = root.Q<TextElement>("UI-ScoreLeft-Text");
        player2ScoreText = root.Q<TextElement>("UI-ScoreRight-Text");
    }

    public void PlayerOneScore(int value)
    {
        try
        {
            GameManager.instance.audioManager.PlaySfx("Goal_ClapClapClap", Random.Range(0.94f, 1.24f));

            float randValue = Random.value;
            if(randValue < .54f)
                GameManager.instance.audioManager.PlaySfx("airhorn", Random.Range(0.92f, 1.18f));
        }
        catch
        {
            Debug.LogWarning("AudioManager not found. Perhaps you're not using the GameManager prefab?");
        }

        player1Score += value;

        player1ScoreText.text = player1Score.ToString();

        //add condition for player win/loss ?
    }
    public void PlayerTwoScore(int value)
    {
        try
        {
            GameManager.instance.audioManager.PlaySfx("Goal_ClapClapClap", Random.Range(0.94f, 1.24f));

            float randValue = Random.value;
            if (randValue < .54f)
                GameManager.instance.audioManager.PlaySfx("airhorn", Random.Range(0.62f, 0.92f));       
        }
        catch
        {
            Debug.LogWarning("AudioManager not found. Perhaps you're not using the GameManager prefab?");
        }

        player2Score += value;

        player2ScoreText.text = player2Score.ToString();

        //add condition for player win/loss ?
    }
}
