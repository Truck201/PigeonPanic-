using UnityEngine;
using TMPro;

public class FloatingScoreText : MonoBehaviour
{
    public float floatSpeed = 1f;
    public float duration = 1f;
    public float fadeDuration = 0.5f;

    private TextMeshPro text;
    private float timer;

    void Start()
    {
        text = GetComponent<TextMeshPro>();
        Destroy(gameObject, duration);
        timer = duration;
    }

    void Update()
    {
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;
        timer -= Time.deltaTime;

        if (timer <= fadeDuration)
        {
            float alpha = Mathf.Clamp01(timer / fadeDuration);
            Color c = text.color;
            c.a = alpha;
            text.color = c;
        }

        if (timer <= 0)
            Destroy(gameObject);
    }

    public void SetColor(Color color)
    {
        if (text == null)
            text = GetComponent<TextMeshPro>();

        text.color = color;
    }

    public void SetText(string value)
    {
        if (text == null)
            text = GetComponent<TextMeshPro>();

        text.text = value;
    }
}
