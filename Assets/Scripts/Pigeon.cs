using UnityEngine;

public class Pigeon : MonoBehaviour
{
    [SerializeField]
    private float lifetimeNormals = 1.7f;  // Ahora editable en Inspector

    [SerializeField]
    private float lifetimeSpecials = 1.7f;
    private int spawnIndex = -1;
    private GameManager gameManager;
    public Vector3 spawnOffset = new Vector3(0, 0.5f, 0);

    private bool wasTouched = false;

    private int normalPigeonPoints;
    private int specialPigeonPoints;

    // Especial
    public bool isSpecial = false;
    private int hitsNeeded = 1;
    private int currentHits = 0;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private bool isExploding = false;

    public void Initialize(Transform[] spawnLocations, int index, GameManager manager, bool special = false, float normalLifetime = 1.7f, float specialLifetime = 1.7f)
    {
        spawnIndex = index;
        gameManager = manager;
        isSpecial = special;

        lifetimeNormals = normalLifetime;
        lifetimeSpecials = specialLifetime;

        transform.position = spawnLocations[spawnIndex].position + spawnOffset;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if (isSpecial)
        {
            hitsNeeded = 2;
            // Base para especiales
        }


        Invoke(nameof(DestroySelf), isSpecial ? lifetimeSpecials : lifetimeNormals);
        isExploding = false;
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
        wasTouched = true;
        if (isExploding) return;
        if (isSpecial)
        {
            int damage = 1;

            // Verifica si el evento especial activo es DoubleTap
            if (gameManager.IsDoubleTapActive())
            {
                damage = 2;
            }

            currentHits += damage;

            CancelInvoke(nameof(DestroySelf));
            lifetimeSpecials += 0.2f;
            Invoke(nameof(DestroySelf), lifetimeSpecials);

            if (currentHits >= hitsNeeded)
            {
                isExploding = true;
                specialPigeonPoints = gameManager.pigeonSpecialPoints;
                gameManager.AddPoint(transform.position, specialPigeonPoints, "+" + specialPigeonPoints, Color.yellow);
                animator.SetTrigger("explode");
            }
            else if (currentHits == 1 && hitsNeeded == 2)
            {
                animator.SetTrigger("preexplode");
            }
        }
        else
        {
            isExploding = true;
            normalPigeonPoints = gameManager.pigeonNormalPoints;
            gameManager.AddPoint(transform.position, normalPigeonPoints, "+" + normalPigeonPoints);
            animator.SetTrigger("explode");
        }
    }

    void DestroySelf()
    {
        if (gameManager != null)
        {
            gameManager.FreeHole(spawnIndex);

            // Solo contar si NO fue tocada por el jugador
            if (!wasTouched)
            {
                gameManager.RegisterMissedPigeon(); // Este método lo crearemos
            }
        }

        CancelInvoke(); // Cancela cualquier invoke pendiente por seguridad
        Destroy(gameObject);
    }
}
