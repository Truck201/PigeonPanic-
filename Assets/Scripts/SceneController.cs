using UnityEngine;
using UnityEngine.UI; // Para usar Text o TMP_Text

public class SceneController : MonoBehaviour
{
    private Text scoreText; // Asigná esto desde el Inspector (o usar TMP_Text)

    private void Start()
    {
        if (scoreText != null && ScoreManager.Instance != null)
        {
            int score = ScoreManager.Instance.GetScore();
            scoreText.text = "Puntaje: " + score.ToString();
        }
    }

    public void PlayGame()
    {
        LevelManager.Instance.LoadPreviusLevel();
        Debug.Log("Chance");
    }

    public void PlayLastGame()
    {
        int levelNumber =  LevelManager.Instance.GetLastCompletedLevel();
        LevelManager.Instance.SetSelectedLevel(levelNumber + 1);
        LevelManager.Instance.LoadPreviusLevel();
    }

    public void ResetLevels()
    {
        LevelManager.Instance.ResetProgress();
        Debug.Log("Progreso reiniciado");
    }
    public void GoToMejoras()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Mejoras");
    }

    public void GoToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void GoToDesafios()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Desafios");
    }

    public void GoToLastScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level6");
    }

    public void GoToGameMap()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameMap");
    }
}
