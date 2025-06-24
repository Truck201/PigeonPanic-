using UnityEngine;
using UnityEngine.Audio;

public class SoundController : MonoBehaviour
{
    public static SoundController Instance;

    [Header("Canales de Audio")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Mixer")]
    public AudioMixer mixerMaster;

    [Header("Música")]
    public AudioClip musicaMenu;
    public AudioClip musicaJuego;
    public AudioClip musicaMapa;
    public AudioClip musicaTienda;

    [Header("SFX - Menú")]
    public AudioClip sfxBoton;
    public AudioClip sfxCambioEscena;

    [Header("SFX - Juego")]
    public AudioClip sfxGolpearPaloma;
    public AudioClip sfxEliminarPaloma;
    public AudioClip sfxAparecerPaloma;
    public AudioClip sfxLimpiarMancha;
    public AudioClip sfxErrarGolpe;
    public AudioClip sfxEventoBonus;

    public AudioClip sfxLevelPassed;
    public AudioClip sfxLevelMissed;

    private float masterVolume = 1f;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
            return;
        }

        LoadVolume();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            Debug.Log($"[SoundController] Reproduciendo SFX: {clip.name}");
            sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("[SoundController] No se pudo reproducir SFX: fuente o clip nulo");
        }
    }

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (musicSource != null && clip != null)
        {
            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.Play();
        }
    }

    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
        float volumeDb = Mathf.Log10(Mathf.Clamp(volume, 0.001f, 1f)) * 20f;

        Debug.Log($"[SoundController] SetMasterVolume: {volume} → {volumeDb} dB");

        if (mixerMaster != null)
            mixerMaster.SetFloat("MasterVolume", volumeDb);

        float currentDb;
        if (mixerMaster.GetFloat("MasterVolume", out currentDb))
        {
            Debug.Log("Volume actual (dB): " + currentDb);
        }

        PlayerPrefs.SetFloat("MasterVolume", volume);
        PlayerPrefs.Save();
    }

    public float GetMasterVolume()
    {
        return masterVolume;
    }

    private void LoadVolume()
    {
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        SetMasterVolume(masterVolume); // Aplica también a los canales
    }
}
