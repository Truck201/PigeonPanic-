using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeData", menuName = "Scriptable Objects/UpgradeData")]
public class UpgradeData : ScriptableObject
{
    [Header("Puntos acumulados")]
    public int playerPoints;

    [Header("Mejoras")]
    public int normalPigeonBonus;          // +puntos por paloma normal
    public int specialPigeonBonus;         // +puntos por paloma especial
    public float comboDurationBonus;       // segundos extra al combo
    public float comboEffectMultiplier;    // Cuenta de cuando se activa el combo

    public int PlayerPoints => ScoreManager.Instance != null ? ScoreManager.Instance.GetScore() : 0;

    public bool SpendPoints(int amount)
    {
        if (PlayerPoints >= amount)
        {
            ScoreManager.Instance.RemoveScore(amount);
            return true;
        }
        return false;
    }
    public void SaveUpgrades()
    {
        PlayerPrefs.SetInt("normalPigeonBonus", normalPigeonBonus);
        PlayerPrefs.SetInt("specialPigeonBonus", specialPigeonBonus);
        PlayerPrefs.SetFloat("comboDurationBonus", comboDurationBonus);
        PlayerPrefs.SetFloat("comboEffectMultiplier", comboEffectMultiplier);

        PlayerPrefs.Save();
    }
    public void LoadUpgrades()
    {
        normalPigeonBonus = PlayerPrefs.GetInt("normalPigeonBonus", 0);
        specialPigeonBonus = PlayerPrefs.GetInt("specialPigeonBonus", 0);
        comboDurationBonus = PlayerPrefs.GetFloat("comboDurationBonus", 0f);
        comboEffectMultiplier = PlayerPrefs.GetFloat("comboEffectMultiplier", 0f);

    }

    public void ResetUpgrades()
    {
        normalPigeonBonus = 0;
        specialPigeonBonus = 0;
        comboDurationBonus = 0f;
        comboEffectMultiplier = 0f;


        PlayerPrefs.DeleteKey("normalPigeonBonus");
        PlayerPrefs.DeleteKey("specialPigeonBonus");
        PlayerPrefs.DeleteKey("comboDurationBonus");
        PlayerPrefs.DeleteKey("comboEffectMultiplier");
        PlayerPrefs.DeleteKey("pigeonSpawnRangeBonus");
        PlayerPrefs.Save();
    }
}
