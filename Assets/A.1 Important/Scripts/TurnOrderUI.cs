using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnOrderUI : MonoBehaviour
{
    public Transform turnOrderContainer;
    public GameObject turnOrderEntryPrefab;

    private readonly Dictionary<ICombatant, GameObject> activeEntries = new Dictionary<ICombatant, GameObject>();

    public void UpdateTurnOrderDisplay(List<ICombatant> turnOrder)
    {
        // Remove entries that are no longer in turnOrder
        foreach (var kvp in new Dictionary<ICombatant, GameObject>(activeEntries))
        {
            if (!turnOrder.Contains(kvp.Key))
            {
                Destroy(kvp.Value);
                activeEntries.Remove(kvp.Key);
            }
        }

        // Add new entries
        foreach (var combatant in turnOrder)
        {
            if (!activeEntries.ContainsKey(combatant))
            {
                GameObject entry = Instantiate(turnOrderEntryPrefab, turnOrderContainer);
                var text = entry.GetComponentInChildren<Text>();
                if (text != null)
                    text.text = combatant.DisplayName;
                activeEntries.Add(combatant, entry);
            }
        }
    }

    public void RemoveCombatantFromOrder(ICombatant combatant)
    {
        if (activeEntries.TryGetValue(combatant, out GameObject entry))
        {
            activeEntries.Remove(combatant);
            Destroy(entry);
        }
    }
}
