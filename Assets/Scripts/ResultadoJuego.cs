using UnityEngine;
using UnityEngine.UI;

public class ResultadoJuego : MonoBehaviour
{
    public GameObject panelVictoria;
    public GameObject panelDerrota;

    void Start()
    {
        bool gano = PlayerPrefs.GetInt("GanoNivel", 0) == 1;

        if (gano)
        {
            panelVictoria.SetActive(true);
            SoundController.Instance.PlaySFX(SoundController.Instance.sfxLevelPassed);
            panelDerrota.SetActive(false);
        }
        else
        {
            panelVictoria.SetActive(false);
            SoundController.Instance.PlaySFX(SoundController.Instance.sfxLevelMissed);
            panelDerrota.SetActive(true);
        }
    }
}
