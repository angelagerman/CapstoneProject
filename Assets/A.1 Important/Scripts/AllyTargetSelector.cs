using System;
using UnityEngine;
using UnityEngine.UI;

public class AllyTargetSelector : MonoBehaviour
{
    public GameObject panel;
    public GameObject allyButtonPrefab;
    public Transform buttonContainer;

    private Action<AllyBattleActions> onTargetSelected;

    public void OpenSelector(AllyBattleActions[] allies, 
        AllyBattleActions actingAlly,
        Action<AllyBattleActions> callback)
    {
        onTargetSelected = callback;

        panel.SetActive(true);

        // Clear previous buttons
        foreach (Transform child in buttonContainer)
            Destroy(child.gameObject);

        // Create ally buttons
        foreach (var ally in allies)
        {
            if (!ally.IsAlive) continue; // only allow selecting living allies
            
            if (ally == actingAlly)
                continue;

            GameObject b = Instantiate(allyButtonPrefab, buttonContainer);
            Button button = b.GetComponent<Button>();
            Text label = b.GetComponentInChildren<Text>();

            // Format Ally Info
            label.text = $"{ally.DisplayName}  HP:{ally.stats.currentHealth}/{ally.stats.maxHealth}";

            button.onClick.AddListener(() => Select(ally));
        }
    }

    private void Select(AllyBattleActions ally)
    {
        Close();
        onTargetSelected?.Invoke(ally);
        onTargetSelected = null;
    }

    public void Close()
    {
        panel.SetActive(false);
    }
}