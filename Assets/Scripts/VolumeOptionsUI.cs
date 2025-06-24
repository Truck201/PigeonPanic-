using UnityEngine;
using UnityEngine.UI;

public class VolumeOptionsUI : MonoBehaviour
{
    public Slider volumeSlider;
    public Text percentageText;

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 0.75f);

        // Evita que el volumen quede en 0
        savedVolume = Mathf.Clamp(savedVolume, 0.001f, 1f);

        volumeSlider.minValue = 0.001f; // Previene que el slider llegue a 0
        volumeSlider.maxValue = 1f;

        volumeSlider.value = savedVolume;
        UpdateText(savedVolume);

        volumeSlider.onValueChanged.AddListener((v) =>
        {
            SoundController.Instance.SetMasterVolume(v);
            UpdateText(v);
        });
    }

    void UpdateText(float value)
    {
        if (percentageText != null)
        {
            percentageText.text = Mathf.RoundToInt(value * 100f) + "%";
        }
    }
}
