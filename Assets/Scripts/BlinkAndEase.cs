using UnityEngine;
public class BlinkAndEase : MonoBehaviour
{
    public bool useScaleAnimation = true;
    public float scaleAmplitude = 0.1f; // Cuánto se agranda
    public float scaleSpeed = 2f;

    private float timer;
    private Vector3 initialScale;
    private Renderer objRenderer;

    void Start()
    {
        initialScale = transform.localScale;

        // Verifica si hay un renderer para ocultar visualmente
        objRenderer = GetComponent<Renderer>();
        if (objRenderer == null)
        {
            objRenderer = GetComponentInChildren<Renderer>();
        }
    }

    void Update()
    {
        timer -= Time.deltaTime;

        // Escalado animado
        if (useScaleAnimation)
        {
            float scaleFactor = 1 + Mathf.Sin(Time.time * scaleSpeed) * scaleAmplitude;
            transform.localScale = initialScale * scaleFactor;
        }
    }

    void SetVisible(bool visible)
    {
        if (objRenderer != null)
        {
            objRenderer.enabled = visible;
        }
        else
        {
            // Si no tiene Renderer, alterna el GameObject completo
            gameObject.SetActive(visible);
        }
    }
}
