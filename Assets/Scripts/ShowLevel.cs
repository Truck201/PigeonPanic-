using TMPro;
using UnityEngine;

public class ShowLevel : MonoBehaviour
{
    [Header("Level Actual")]
    public TextMeshProUGUI textActualLevel;
    private int lastCompleted;

    [Header("Metas Requeridas")]
    public TextMeshProUGUI requiredScoreText;

    [Header("Puntos de Meta")]
    public int scoreMetas;

    [Header("Está en Cartél?")]
    public bool isInCartel = false;

    [Header("Está en Desafios?")]
    public bool isInDesafios = false;

    void Start()
    {

        if (isInCartel == true)
        {
            lastCompleted = LevelManager.Instance.SelectedLevel;
            textActualLevel.text = $"{lastCompleted}";
        } else if (isInDesafios == true)
        {
            lastCompleted = 6;
            textActualLevel.text = "DESAFIO";
        }
        else {
            if (textActualLevel != null)
            {
                lastCompleted = Mathf.Max(0, LevelManager.Instance.GetLastCompletedLevel());
                textActualLevel.text = $"Level {lastCompleted + 1}";
            }

        }

        if (requiredScoreText != null)
        {
            lastCompleted = LevelManager.Instance.SelectedLevel;
            scoreMetas = LevelManager.Instance.MetaActual(lastCompleted);
            requiredScoreText.text = $"{scoreMetas.ToString("D6")}";
        }
    }

    private void Update()
    {

        if (isInCartel == true)
        {
            lastCompleted = LevelManager.Instance.SelectedLevel;
            textActualLevel.text = $"{lastCompleted}";
        } else if (isInDesafios == true) { 
            lastCompleted = 6;
            textActualLevel.text = "DESAFIO";
        } else {
            if (textActualLevel != null)
            {
                lastCompleted = Mathf.Max(0, LevelManager.Instance.GetLastCompletedLevel());
                textActualLevel.text = $"Level {lastCompleted + 1}";
            }
        }
    }
}
