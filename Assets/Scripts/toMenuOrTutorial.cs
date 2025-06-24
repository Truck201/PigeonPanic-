using UnityEngine;
using UnityEngine.SceneManagement;


public class toMenuOrTutorial : MonoBehaviour
{
    [Header("Nombre Escena Menu")]
    public string menuName;

    [Header("Nombre Escena Tutorial")]
    public string tutorialName;

    [Header("Tiempo de Cambio")]
    public float changeTimer;

    private int levelActual;

    // Update is called once per frame
    void Update()
    {
        changeTimer -= Time.deltaTime;
        if (changeTimer <= 0 )
        {
            levelActual = LevelManager.Instance.GetLastCompletedLevel();
            Debug.Log(levelActual);

            if (levelActual != 0)
            {
                Debug.Log(menuName);
                SceneManager.LoadScene(menuName);

            }
            else
            {
                Debug.Log(tutorialName);
                SceneManager.LoadScene(menuName);
            }
        }
    }
}
