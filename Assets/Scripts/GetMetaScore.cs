using UnityEngine;
using TMPro;

public class GetMetaScore : MonoBehaviour
{
    public TextMeshPro requiredScoreText;
    private LevelData currentLevelData;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (requiredScoreText != null && currentLevelData != null)
        {
            requiredScoreText.text = $"{currentLevelData.requiredScore.ToString("D6")}";
        }
    }
}
