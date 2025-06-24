using UnityEngine;

public class Egg : MonoBehaviour
{
    private GameManager gameManager;
    private int spawnIndex;

    public float lifetime = 3f;

    public void Initialize(GameManager manager, int index)
    {
        gameManager = manager;
        spawnIndex = index;

        Invoke(nameof(DestroySelf), lifetime);
    }

    void Update()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            if (touch.phase == TouchPhase.Began)
            {
                Vector2 worldPoint = Camera.main.ScreenToWorldPoint(touch.position);
                RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {
                    HandleHit();
                }
            }
        }
    }

    void HandleHit()
    {
        if (gameManager != null)
        {
            gameManager.RemovePointsWithStain(transform.position);
        }

        DestroySelf();
    }

    void DestroySelf()
    {
        if (gameManager != null)
            gameManager.FreeHole(spawnIndex);

        CancelInvoke(); // Cancela cualquier invoke pendiente por seguridad
        Destroy(gameObject);
    }
}
