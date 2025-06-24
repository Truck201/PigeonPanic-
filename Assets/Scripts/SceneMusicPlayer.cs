using UnityEngine;

public class SceneMusicPlayer : MonoBehaviour
{
    public enum TipoMusica
    {
        Ninguna,
        Menu,
        Juego,
        Mapa,
        Tienda
    }

    [Header("Seleccionar música para esta escena")]
    public TipoMusica musicaDeEscena = TipoMusica.Menu;

    void Start()
    {
        AudioClip clipDeseado = ObtenerClipDeMusica(musicaDeEscena);

        if (clipDeseado == null)
        {
            // Si se eligió 'Ninguna', cortamos la música actual
            SoundController.Instance.StopMusic();
            return;
        }

        AudioSource currentMusicSource = SoundController.Instance.musicSource;

        if (currentMusicSource.clip == clipDeseado && currentMusicSource.isPlaying)
        {
            // Ya está sonando la música correcta → no hacemos nada
            return;
        }

        // Cambiar a la nueva música
        SoundController.Instance.PlayMusic(clipDeseado);
    }

    AudioClip ObtenerClipDeMusica(TipoMusica tipo)
    {
        var sound = SoundController.Instance;

        switch (tipo)
        {
            case TipoMusica.Menu: return sound.musicaMenu;
            case TipoMusica.Juego: return sound.musicaJuego;
            case TipoMusica.Mapa: return sound.musicaMapa;
            case TipoMusica.Tienda: return sound.musicaTienda;
            default: return null;
        }
    }
}
