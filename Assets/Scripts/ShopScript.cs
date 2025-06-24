using UnityEngine;
using UnityEngine.UI;

public class ShopScript : MonoBehaviour
{
    public UpgradeData upgradeData;

    [Header("Botones de Mejora")]
    public Button normalPigeonButton;
    public Button specialPigeonButton;
    public Button comboDurationButton;
    public Button comboEffectMultiplierButton;

    [Header("Imágenes de Mejora")]
    public Image normalPigeonImage;
    public Image specialPigeonImage;
    public Image comboDurationImage;
    public Image comboEffectMultiplierImage;

    [Header("Modo Debug")]
    public bool debugResetUpgrades = false;

    private void Start()
    {
        // Cargar mejoras desde PlayerPrefs
        upgradeData.LoadUpgrades();

        if (debugResetUpgrades)
        {
            RefundUpgrades();
            upgradeData.ResetUpgrades();
            debugResetUpgrades = false;
        }

        CheckUpgradesAndDisableButtons();
    }

    private void CheckUpgradesAndDisableButtons()
    {
        if (upgradeData.normalPigeonBonus > 0)
            DisableButton(normalPigeonButton, normalPigeonImage);

        if (upgradeData.specialPigeonBonus > 0)
            DisableButton(specialPigeonButton, specialPigeonImage);

        if (upgradeData.comboDurationBonus > 0)
            DisableButton(comboDurationButton, comboDurationImage);

        if (upgradeData.comboEffectMultiplier > 0)
            DisableButton(comboEffectMultiplierButton, comboEffectMultiplierImage);
    }

    public void BuyNormalPigeonBonus()
    {
        int cost = 150000;
        if (upgradeData.SpendPoints(cost))
        {
            upgradeData.normalPigeonBonus += 250;
            upgradeData.SaveUpgrades();
            Debug.Log("Compraste mejora de puntos normales");
            DisableButton(normalPigeonButton, normalPigeonImage);
        }
        else
        {
            Debug.Log("No tenés puntos suficientes");
        }
    }

    public void BuySpecialPigeonBonus()
    {
        int cost = 300000;
        if (upgradeData.SpendPoints(cost))
        {
            upgradeData.specialPigeonBonus += 500;
            upgradeData.SaveUpgrades();
            Debug.Log("Compraste mejora de puntos normales");
            DisableButton(specialPigeonButton, specialPigeonImage);
        }
        else
        {
            Debug.Log("No tenés puntos suficientes");
        }
    }

    public void BuyComboDurationBonus()
    {
        int cost = 300000;
        if (upgradeData.SpendPoints(cost))
        {
            upgradeData.comboDurationBonus += 2f;
            upgradeData.SaveUpgrades();
            Debug.Log("Compraste mejora de duración de combo");
            DisableButton(comboDurationButton, comboDurationImage);
        }
        else
        {
            Debug.Log("No tenés puntos suficientes");
        }
    }

    public void BuyComboEffectMultiplier()
    {
        int cost = 300000;
        if (upgradeData.SpendPoints(cost))
        {
            upgradeData.comboEffectMultiplier += 5f;
            upgradeData.SaveUpgrades();
            Debug.Log("Compraste mejora de Cuenta Activa de Combo");
            DisableButton(comboEffectMultiplierButton, comboEffectMultiplierImage);
        }
        else
        {
            Debug.Log("No tenés puntos suficientes");
        }
    }

    private void DisableButton(Button button, Image image)
    {
        if (button != null)
            button.interactable = false;

        if (image != null)
        {
            Color color = image.color;
            color.a = 0.5f; // 50% opaco
            image.color = color;
        }
    }

    private void RefundUpgrades()
    {
        int refund = 0;
        if (upgradeData.normalPigeonBonus > 0)
            refund += 150000;

        if (upgradeData.specialPigeonBonus > 0)
            refund += 300000;

        if (upgradeData.comboDurationBonus > 0)
            refund += 300000;

        if (upgradeData.comboEffectMultiplier > 0)
            refund += 300000;

        if (refund > 0 && ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(refund);
            Debug.Log($"[DEBUG] Reembolso total: {refund}");
        }
    }
    
}
