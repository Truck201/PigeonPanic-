using UnityEngine;
using TMPro;

public class ShowScore : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    void Start()
    {
        int score = ScoreManager.Instance.GetScore();

        if (scoreText != null)
        {
            scoreText.text = score.ToString("D6");
            //textoPuntaje.text = GameManager.finalScore.ToString("D6");
        }
    }

    private void Update()
    {
        if (scoreText != null)
        {
            int score = ScoreManager.Instance.GetScore();
            scoreText.text = score.ToString("D6");
        }
    }
}