using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OpcionesUI : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject canvasOpciones;
    public Slider sliderVolumen;
    public AudioMixer mixerMaster; // Vinculado al AudioMixer
    public Button botonSalir;
    public Button botonVolver;

    private void Start()
    {
        // Cargar el volumen guardado o poner valor por defecto
        float volumenGuardado = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
        sliderVolumen.value = volumenGuardado;
        SetVolumen(volumenGuardado);

        // Eventos
        sliderVolumen.onValueChanged.AddListener(SetVolumen);
        botonSalir.onClick.AddListener(SalirDelJuego);
        botonVolver.onClick.AddListener(() => canvasOpciones.SetActive(false));

        canvasOpciones.SetActive(false); // Por defecto oculto
    }

    public void MostrarOpciones()
    {
        canvasOpciones.SetActive(true);
    }

    public void OcultarOpciones()
    {
        canvasOpciones.SetActive(false);
    }

    private void SetVolumen(float valor)
    {
        // Convertimos a dB porque el AudioMixer trabaja en -80 a 0 dB
        float volumenDB = Mathf.Log10(Mathf.Clamp(valor, 0.001f, 1f)) * 20f;
        mixerMaster.SetFloat("MasterVolume", volumenDB);

        PlayerPrefs.SetFloat("MasterVolume", valor);
        PlayerPrefs.Save();
    }

    public void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
