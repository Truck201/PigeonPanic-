using UnityEngine;
using UnityEngine.EventSystems; // ¡Necesario para SetSelectedGameObject!

public class InteractionMenu : MonoBehaviour
{
    [Header("Paneles de mejoras")]
    public GameObject panelLabel1;
    public GameObject panelLabel2;
    public GameObject panelLabel3;

    [Header("Botones Labels")]
    public GameObject buttonLabel1;
    public GameObject buttonLabel2;
    public GameObject buttonLabel3;

    void Start()
    {
        MostrarPanel(1);
    }

    public void MostrarPanel(int index)
    {
        panelLabel1.SetActive(index == 1);
        panelLabel2.SetActive(index == 2);
        panelLabel3.SetActive(index == 3);

        // Set selected visual state para el botón correspondiente
        switch (index)
        {
            case 1:
                EventSystem.current.SetSelectedGameObject(buttonLabel1);
                break;
            case 2:
                EventSystem.current.SetSelectedGameObject(buttonLabel2);
                break;
            case 3:
                EventSystem.current.SetSelectedGameObject(buttonLabel3);
                break;
        }
    }
}
