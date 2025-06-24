using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    private const string ScoreKey = "PlayerScore";
    private int currentScore;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persistir entre escenas
            currentScore = PlayerPrefs.GetInt(ScoreKey, 0);
        }
    }

    public int GetScore()
    {
        return currentScore;
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        PlayerPrefs.SetInt(ScoreKey, currentScore);
        PlayerPrefs.Save();
    }

    public void RemoveScore(int amount) 
    {
        currentScore -= amount;
        PlayerPrefs.SetInt(ScoreKey, currentScore);
        PlayerPrefs.Save();
    }

    public void ResetScore()
    {
        currentScore = 0;
        PlayerPrefs.SetInt(ScoreKey, currentScore);
        PlayerPrefs.Save();
    }
}
