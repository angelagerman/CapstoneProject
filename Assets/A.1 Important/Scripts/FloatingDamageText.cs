using UnityEngine;
using UnityEngine.UI;

public class FloatingDamageText : MonoBehaviour
{
    public float floatSpeed = 30f; // pixels/sec
    public float duration = 1f;

    private Text tmp;
    private float timer = 0f;
    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        tmp = GetComponentInChildren<Text>();

    }

    public void SetText(string text, Color color)
    {
        if (tmp != null)
        {
            tmp.text = text;
            tmp.color = color;
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;

        // Float up
        if (rect != null)
            rect.anchoredPosition += Vector2.up * floatSpeed * Time.deltaTime;

        // Destroy after duration
        if (timer >= duration)
            Destroy(gameObject);
    }
}