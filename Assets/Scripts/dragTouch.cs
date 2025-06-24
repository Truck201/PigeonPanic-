using UnityEngine;
using UnityEngine.SceneManagement;

public class dragTouch : MonoBehaviour
{
    [Header("Escenas")]
    public string leftSceneName;
    public string rightSceneName;

    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private bool isSwiping = false;

    [Header("Configuración Swipe")]
    public float minSwipeDistance = 50f; // Distancia mínima en píxeles para considerar swipe

    [Header("Canvas de Opciones")]
    public GameObject opcionesCanvas;

    void Update()
    {
        if (opcionesCanvas != null && opcionesCanvas.activeSelf)
            return;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startTouchPosition = touch.position;
                    isSwiping = true;
                    break;

                case TouchPhase.Moved:
                    endTouchPosition = touch.position;
                    break;

                case TouchPhase.Ended:
                    if (!isSwiping) return;

                    endTouchPosition = touch.position;
                    DetectSwipeDirection();
                    isSwiping = false;
                    break;
            }
        }
    }

    private void DetectSwipeDirection()
    {
        float swipeDistanceX = endTouchPosition.x - startTouchPosition.x;
        float swipeDistanceY = Mathf.Abs(endTouchPosition.y - startTouchPosition.y);

        if (Mathf.Abs(swipeDistanceX) > minSwipeDistance && swipeDistanceX != 0 && swipeDistanceY < minSwipeDistance)
        {
            if (swipeDistanceX > 0)
            {
                Debug.Log("Swipe Right");
                if (!string.IsNullOrEmpty(rightSceneName))
                    SceneManager.LoadScene(rightSceneName);
            }
            else
            {
                Debug.Log("Swipe Left");
                if (!string.IsNullOrEmpty(leftSceneName))
                    SceneManager.LoadScene(leftSceneName);
            }
        }
    }
}
