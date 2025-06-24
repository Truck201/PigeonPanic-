using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    private const string LevelKey = "LevelCompleted"; // PlayerPrefs key

    public int SelectedLevel { get; private set; } = 1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persistir entre escenas
        }
    }

    // Retorna el último nivel completado (por defecto 0)
    public int GetLastCompletedLevel()
    {
        return PlayerPrefs.GetInt(LevelKey, 0);
    }

    // Marca que el jugador ha completado un nivel específico
    public void MarkLevelCompleted(int levelNumber)
    {
        if (levelNumber > GetLastCompletedLevel())
        {
            PlayerPrefs.SetInt(LevelKey, levelNumber);
            PlayerPrefs.Save();
        }
    }

    // Carga el siguiente nivel pendiente (LOAD LEVEL)
    public void LoadNextLevel(int level)
    {
        int nextLevel = level;
        string sceneToLoad = "Level" + nextLevel; // Escenas deben llamarse "Level1", "Level2", etc.

        Debug.Log("Cargando siguiente nivel: " + "Level" + nextLevel);
        SceneManager.LoadScene(sceneToLoad);
    }


    public void LoadPreviusLevel()
    {
        string sceneToLoad = "LevelPrevio";
        Debug.Log("Cargando Nivel previo " + sceneToLoad);

        SceneManager.LoadScene(sceneToLoad);
    }

    public int GetNextLevel()
    {
        int lastCompleted = GetLastCompletedLevel();
        int level = lastCompleted + 1;
  
        return level;
    }

    // Reinicia el progreso (para testing o reset)
    public void ResetProgress()
    {
        PlayerPrefs.DeleteKey(LevelKey);
        PlayerPrefs.Save();
    }

    public int MetaActual(int level)
    {
        switch (level)
        {
            case 1: return 800;
            case 2: return 15000;
            case 3: return 25000;
            case 4: return 40000;
            case 5: return 70000;

            // Podés seguir agregando manualmente
            default: return 4500 + ((level - 4) * 1500); // a partir del nivel 5 crece de a 1500
        }
    }

    public void SetSelectedLevel(int level)
    {
        SelectedLevel = level;
    }

}
