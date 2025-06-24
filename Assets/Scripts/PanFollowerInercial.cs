using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PanFollowerInercial : MonoBehaviour
{
    public float followSpeed = 10f;
    [SerializeField] private ParticleSystem particulas;

    private Camera mainCam;
    private Rigidbody2D rb;

    private Vector3 targetPosition;
    private Vector3 initialPosition;
    private bool isDragging = false;

    private Vector3 lastPosition;
    private Vector3 velocity;

    private float totalDragDistance;
    private float spriteSizeThreshold;

    public Slider cargaSlider;
    public GameObject botonIniciar;

    private float tiempoArrastre = 0f;
    private int particulasEmitidas = 0;
    private bool objetivoAlcanzado = false;

    void Start()
    {
        mainCam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;

        initialPosition = transform.position;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        spriteSizeThreshold = sr != null
            ? 0.9f * Mathf.Max(sr.bounds.size.x, sr.bounds.size.y)
            : 0.9f;

        if (particulas != null)
        {
            particulas.Stop();
            var trigger = particulas.trigger;
            trigger.enabled = false;
            particulas.gameObject.GetComponent<ParticleSystem>().GetComponent<ParticleSystem>().Stop();
            particulas.gameObject.GetComponent<ParticleSystem>().GetComponent<ParticleSystem>().Clear();
        }
    }

    void Update()
    {
        // TOQUE COMIENZA
        if ((Input.GetMouseButtonDown(0) || Input.touchCount > 0) && !isDragging)
        {
            Vector3 inputPos = Input.touchCount > 0 ? (Vector3)Input.GetTouch(0).position : Input.mousePosition;
            Vector3 worldPos = mainCam.ScreenToWorldPoint(inputPos);
            worldPos.z = 0;

            Collider2D hit = Physics2D.OverlapPoint(worldPos);
            if (hit && hit.gameObject == gameObject)
            {
                isDragging = true;
                rb.bodyType = RigidbodyType2D.Kinematic;
                rb.gravityScale = 0;
                velocity = Vector3.zero;
                totalDragDistance = 0;
                lastPosition = transform.position;
            }
        }

        // TOQUE SOSTENIDO
        if (isDragging)
        {
            Vector3 inputPos = Input.touchCount > 0 ? (Vector3)Input.GetTouch(0).position : Input.mousePosition;
            Vector3 worldPos = mainCam.ScreenToWorldPoint(inputPos);
            worldPos.z = 0;

            targetPosition = worldPos;
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

            velocity = (transform.position - lastPosition) / Time.deltaTime;
            float movement = Vector3.Distance(transform.position, lastPosition);
            totalDragDistance += movement;

            // Activar partículas solo si el movimiento supera un umbral mínimo
            if (movement > 0.01f)
            {
                if (particulas != null && !particulas.isPlaying)
                {
                    particulas.Play();
                }

                // Contabilizar progreso cada frame durante el arrastre
                tiempoArrastre += Time.deltaTime;

                // Emitir una partícula virtual cada cierto tiempo o movimiento (opcional)
                particulasEmitidas++;

                float progreso = Mathf.Min((particulasEmitidas / 1000f) + (tiempoArrastre / 5f), 1f);
                cargaSlider.value = progreso;

                if (!objetivoAlcanzado && progreso >= 1f)
                {
                    objetivoAlcanzado = true;
                    botonIniciar.SetActive(true);
                }
            }

            else
            {
                if (particulas != null && particulas.isPlaying)
                {
                    particulas.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                }
            }

            lastPosition = transform.position;
        }

        // TOQUE TERMINA
        if ((Input.GetMouseButtonUp(0) || Input.touchCount == 0) && isDragging)
        {
            isDragging = false;

            float distanceMoved = Vector3.Distance(transform.position, initialPosition);

            if (distanceMoved > spriteSizeThreshold)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.gravityScale = 1;
                rb.linearVelocity = velocity;
            }
            else
            {
                transform.position = new Vector3(initialPosition.x, initialPosition.y - 5f, initialPosition.z);
                rb.linearVelocity = Vector2.zero;
                rb.gravityScale = 0;
                rb.bodyType = RigidbodyType2D.Kinematic;
            }

            // Detener partículas
            if (particulas != null)
            {
                particulas.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
        }
    }
    public void IniciarJuego()
    {
        SceneManager.LoadScene("Game");
    }
}
