using UnityEngine;

public class StainController : MonoBehaviour
{
    public int cleanLevel = 0; // 0 = opaco, 1 = semi, 2 = muy traslúcido

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void IncreaseCleanLevel()
    {
        cleanLevel++;

        Color c = spriteRenderer.color;

        if (cleanLevel == 1)
            c.a = 0.66f;
        else if (cleanLevel == 2)
            c.a = 0.33f;
        else if (cleanLevel >= 3)
        {
            Debug.Log("Mancha eliminada");
            Destroy(gameObject);
            return;
        }

        spriteRenderer.color = c;
    }
}
