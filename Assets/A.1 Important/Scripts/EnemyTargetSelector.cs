using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTargetSelector : MonoBehaviour
{
    public GameObject panel;
    public GameObject enemyButtonPrefab;
    public Transform buttonContainer;

    private Action<EnemyBattleActions> onTargetSelected;

    public void OpenSelector(EnemyBattleActions[] enemies, Action<EnemyBattleActions> callback)
    {
        onTargetSelected = callback;

        panel.SetActive(true);

        // Clear old buttons
        foreach (Transform child in buttonContainer)
            Destroy(child.gameObject);

        // Populate buttons
        foreach (var enemy in enemies)
        {
            if (!enemy.IsAlive) continue;

            GameObject b = Instantiate(enemyButtonPrefab, buttonContainer);
            Button button = b.GetComponent<Button>();
            Text label = b.GetComponentInChildren<Text>();

            label.text = $"{enemy.DisplayName}  Lv:{enemy.stats.level}  HP:{enemy.currentHealth}";

            button.onClick.AddListener(() => Select(enemy));
        }
    }

    private void Select(EnemyBattleActions enemy)
    {
        panel.SetActive(false);

        onTargetSelected?.Invoke(enemy);
        onTargetSelected = null;
    }
}
