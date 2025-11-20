using UnityEngine;

public class DamageTextManager : MonoBehaviour
{
    public static DamageTextManager Instance;
    public Canvas damageCanvas;  // Screen-space canvas
    public GameObject damageTextPrefab; // text prefab
    public Camera battleCamera;   // used to convert world → screen

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnDamageText(Transform target, int amount, bool isCrit)
    {
        if (damageCanvas == null || damageTextPrefab == null || battleCamera == null)
            return;

        // 1. Get world position above the target
        Vector3 worldPos = target.position + Vector3.up * 1.5f;

        // 2. Convert world position → screen point
        Vector3 screenPos = battleCamera.WorldToScreenPoint(worldPos);

        // 3. Convert screen point → canvas local position
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            damageCanvas.transform as RectTransform,
            screenPos,
            damageCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : battleCamera,
            out Vector2 localPos
        );

        // 4. Instantiate damage text prefab
        GameObject obj = Instantiate(damageTextPrefab, damageCanvas.transform);
        RectTransform rt = obj.GetComponent<RectTransform>();
        rt.anchoredPosition = localPos;

        // 5. Set the text
        var txt = obj.GetComponent<FloatingDamageText>();
        if (txt != null)
        {
            if (isCrit)
                txt.SetText($"CRIT!\n-{amount}", Color.yellow);
            else
                txt.SetText($"-{amount}", Color.red);
        }
    }

}