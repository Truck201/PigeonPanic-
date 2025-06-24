using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class MapLevel
{
    public GameObject levelObject; // GameObject que representa el nivel
    public Button levelButton;     // Botón para entrar al nivel
    public Animator levelAnimator; // Animator del nivel
    public int requiredLevel;      // Nivel necesario para desbloquear
}

public class MapConfigs : MonoBehaviour
{
    public MapLevel[] mapLevels;
    private int lastCompleted;

    void Start()
    {
        lastCompleted = Mathf.Max(0, LevelManager.Instance.GetLastCompletedLevel());
        Debug.Log("Último nivel completado: " + lastCompleted);

        UpdateMapLevels();
    }

    void UpdateMapLevels()
    {
        foreach (var level in mapLevels)
        {
            int levelNumber = level.requiredLevel;

            bool isUnlocked = levelNumber <= lastCompleted;

            // Activar/desactivar botón
            if (level.levelButton != null)
            {
                level.levelButton.interactable = isUnlocked;

                // Limpiamos listeners viejos por si acaso
                level.levelButton.onClick.RemoveAllListeners();

                if (isUnlocked)
                {
                    level.levelButton.onClick.AddListener(() => OnLevelButtonPressed(levelNumber));
                }
            }

            // Activar o desactivar animación idle
            if (level.levelAnimator != null)
            {
                if (isUnlocked)
                    level.levelAnimator.Play("idle-punto-paloma"); // Asegurate de tener esa animación
                else
                    level.levelAnimator.Play("lock"); // Animación vacía o estado sin nada
            }

            // También podrías activar/desactivar visibilidad, efectos, etc.
            if (level.levelObject != null)
                level.levelObject.SetActive(true); // Siempre visible, o ponlo en false si querés ocultarlo
        }
    }

    void OnLevelButtonPressed(int levelNumber)
    {
        LevelManager.Instance.SetSelectedLevel(levelNumber + 1);
        LevelManager.Instance.LoadPreviusLevel();
    }
}
