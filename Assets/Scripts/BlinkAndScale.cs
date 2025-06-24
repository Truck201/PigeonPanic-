using UnityEngine;

public class BlinkAndScale : MonoBehaviour
{
    public float blinkInterval = 0.3f;
    public float scaleFactor = 1.3f;
    public float scaleSpeed = 2f;

    private SpriteRenderer spriteRenderer;
    private Vector3 originalScale;
    private bool isBlinking = false;
    private float blinkTimer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;
        blinkTimer = blinkInterval;
    }

    void Update()
    {
        AnimateScale();
        // AnimateBlink();
    }

    void AnimateScale()
    {
        float scale = 1 + Mathf.Sin(Time.time * scaleSpeed) * (scaleFactor - 1);
        transform.localScale = originalScale * scale;
    }

    void AnimateBlink()
    {
        if (!isBlinking) return;

        blinkTimer -= Time.deltaTime;
        if (blinkTimer <= 0f)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            blinkTimer = blinkInterval;
        }
    }

    public void StartBlinking()
    {
        isBlinking = true;
    }

    public void StopBlinking()
    {
        isBlinking = false;
        spriteRenderer.enabled = true;
    }
}
