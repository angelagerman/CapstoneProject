using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class MagicMenuUI : MonoBehaviour
{
    public GameObject magicButtonPrefab;
    public GameObject magicPanel;
    public Transform contentParent;

    private AllyBattleActions currentAlly;
    private System.Action<AllyBattleActions.MagicSpell> onSpellSelected;

    public void Open(AllyBattleActions ally, System.Action<AllyBattleActions.MagicSpell> callback)
    {
        currentAlly = ally;
        onSpellSelected = callback;

        // Clear old buttons
        foreach (Transform t in contentParent)
            Destroy(t.gameObject);

        // Create buttons
        foreach (var spell in ally.magicSpells)
        {
            if (spell.levelRequirement > ally.stats.level)
                continue;
            
            GameObject btnObj = Instantiate(magicButtonPrefab, contentParent);
            Text spellText = btnObj.GetComponentInChildren<Text>();
            Button btn = btnObj.GetComponent<Button>();
            
            spellText.text = $"{spell.spellName}  ({spell.manaCost} MP)";

            
            bool canAfford = ally.stats.currentMagic >= spell.manaCost;
            btn.interactable = canAfford;

            btnObj.GetComponent<Button>().onClick.AddListener(() =>
            {
                onSpellSelected?.Invoke(spell);
            });
        }

        magicPanel.SetActive(true);
    }

    public void Close()
    {
        magicPanel.SetActive(false);
    }
}
