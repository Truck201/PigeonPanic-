using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CargarNivel : MonoBehaviour
{
    public float changeTimer;

    public string SceneName;

    public bool Resultados = false;
    void Start()
    {
        if (Resultados == false)
        {
            int levelToLoad = LevelManager.Instance.SelectedLevel;
            Debug.Log("Cargar Niveles -> Cargar nivel " + levelToLoad);
            SceneName = $"Level{levelToLoad}";
        }   
    }

    private void Update()
    {
        changeTimer -= Time.deltaTime;
        if (changeTimer <= 0)
        {
            SceneManager.LoadScene(SceneName);
        } 
    }
}
