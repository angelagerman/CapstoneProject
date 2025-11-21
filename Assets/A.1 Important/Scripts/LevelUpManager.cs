using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpManager : MonoBehaviour
{
    public GameObject levelUpPanel;
    public Text titleText;
    public Text characterNameText;
    public Transform statsContainer;
    public Transform newSpellsContainer;
    public GameObject statTextPrefab;
    public GameObject spellTextPrefab;
    public Button continueButton;

    private System.Action onLevelUpComplete;

    void Awake()
    {
        levelUpPanel.SetActive(false);
        continueButton.onClick.AddListener(OnContinue);
    }

    public void ShowLevelUp(AllyBattleActions ally, Dictionary<string, int> statIncreases, List<AllyBattleActions.MagicSpell> newSpells, System.Action onComplete)
    {
        onLevelUpComplete = onComplete;
        levelUpPanel.SetActive(true);
        characterNameText.text = ally.DisplayName;

        // Clear old UI
        foreach (Transform child in statsContainer) Destroy(child.gameObject);
        foreach (Transform child in newSpellsContainer) Destroy(child.gameObject);

        // Display full stats with increase
        // Display full stats with increase
        foreach (var kvp in statIncreases)
        {
            GameObject statText = Instantiate(statTextPrefab, statsContainer);

            int oldValue;
            int newValue;

            switch (kvp.Key)
            {
                case "HP":
                    oldValue = ally.stats.maxHealth - kvp.Value;
                    newValue = ally.stats.maxHealth;
                    break;
                case "Strength":
                    oldValue = ally.stats.strength - kvp.Value;
                    newValue = ally.stats.strength;
                    break;
                case "Magic":
                    oldValue = ally.stats.magic - kvp.Value;
                    newValue = ally.stats.magic;
                    break;
                case "Defense":
                    oldValue = ally.stats.defense - kvp.Value;
                    newValue = ally.stats.defense;
                    break;
                case "Speed":
                    oldValue = ally.stats.speed - kvp.Value;
                    newValue = ally.stats.speed;
                    break;
                case "Luck":
                    oldValue = ally.stats.luck - kvp.Value;
                    newValue = ally.stats.luck;
                    break;
                default:
                    oldValue = 0;
                    newValue = 0;
                    break;
            }

            // Only show old → new if stat increased, else just show current value
            if (kvp.Value > 0)
                statText.GetComponent<Text>().text = $"{kvp.Key}: {oldValue} → {newValue}";
            else
                statText.GetComponent<Text>().text = $"{kvp.Key}: {newValue}";
        }


        // Display new spells
        foreach (var spell in newSpells)
        {
            GameObject spellText = Instantiate(spellTextPrefab, newSpellsContainer);
            spellText.GetComponent<Text>().text = $"New Spell: {spell.spellName}";
        }
    }


    private void OnContinue()
    {
        levelUpPanel.SetActive(false);
        onLevelUpComplete?.Invoke();
    }
}