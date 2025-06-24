using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ContadorJuego : MonoBehaviour
{
    [SerializeField] private float tiempoMaximo;
    [SerializeField] private Slider slider;

    [SerializeField] private GameManager gameManager;

    private float tiempoActual;
    private bool tiempoActivado = false;

    private bool pausado = false;

    private void Start()
    {
        ActivarTemporizador();
    }

    private void Update()
    {
        if (tiempoActivado)
        {
            CambiarContador();
        }
    }

    private void CambiarContador()
    {
        tiempoActual -= Time.deltaTime;

        if (tiempoActual >= 0)
        {
            slider.value = tiempoActual;
        }

        if (tiempoActual <= 0)
        {
            Debug.Log("Finalizado");
            CambiarTemporizador(false);

            if (gameManager != null)
            {
                int finalScore = gameManager.score;
                int requiredScore = gameManager.currentLevelData.requiredScore;

                // Verificar si pasó el nivel
                if (finalScore >= requiredScore)
                {

                    Debug.Log("¡Nivel aprobado!");

                    int nivelActual = int.Parse(SceneManager.GetActiveScene().name.Replace("Level", ""));
                    int ultimoCompletado = LevelManager.Instance.GetLastCompletedLevel();

                    Debug.Log("Nivel Actual -> " + nivelActual + " Ultimo Nivel Completado -> " + ultimoCompletado);

                    // Solo avanzamos si el jugador superó el nivel más avanzado desbloqueado
                    if (nivelActual == ultimoCompletado + 1)
                    {
                        LevelManager.Instance.MarkLevelCompleted(nivelActual);
                        Debug.Log("Nivel nuevo desbloqueado: Level" + (nivelActual + 1));
                    }
                    else
                    {
                        Debug.Log("Nivel ya jugado anteriormente, no se avanza.");
                    }

                    ScoreManager.Instance.AddScore(finalScore);
                }
                else
                {
                    Debug.Log("No alcanzó los puntos requeridos.");
                    // LevelManager.Instance.MarkLevelFailed(actualLevel);
                }

                // Guardar si ganó o no
                bool gano = finalScore >= requiredScore;
                PlayerPrefs.SetInt("GanoNivel", gano ? 1 : 0);
                PlayerPrefs.Save();

                // Ir a escena de resultado
                SceneManager.LoadScene("Resultado");
            }

        }
    }

    public void AñadirTiempo(float segundos)
    {
        tiempoActual += segundos;
        if (tiempoActual > tiempoMaximo)
            tiempoActual = tiempoMaximo;

        slider.value = tiempoActual;
    }

    public void RestarTiempo(float segundos)
    {
        tiempoActual -= segundos;
        if (tiempoActual < 0)
            tiempoActual = 0;

        slider.value = tiempoActual;
    }


    private void CambiarTemporizador(bool estado)
    {
            tiempoActivado = estado;
    }

    private void ActivarTemporizador()
    {
        tiempoActual = tiempoMaximo;
        slider.value = tiempoMaximo;
        CambiarTemporizador(true);
    }

    private void DesactivarTemporizador()
    {
        CambiarTemporizador(false);
    }

    public void Pausar()
    {
        pausado = true;
    }

    public void Reanudar()
    {
        pausado = false;
    }

    public float TiempoRestante()
    {
        return tiempoActual;
    }
}
