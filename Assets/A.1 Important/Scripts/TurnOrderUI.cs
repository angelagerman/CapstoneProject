using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnOrderUI : MonoBehaviour
{
    public Transform turnOrderContainer;  // parent object that holds all entries
    public GameObject turnOrderEntryPrefab; // your text prefab

    private readonly List<GameObject> activeEntries = new List<GameObject>();

    public void UpdateTurnOrderDisplay(List<ICombatant> turnOrder)
    {
        // Clear old entries
        foreach (var entry in activeEntries)
            Destroy(entry);
        activeEntries.Clear();

        // Create new entries
        foreach (var combatant in turnOrder)
        {
            GameObject entry = Instantiate(turnOrderEntryPrefab, turnOrderContainer);
            var text = entry.GetComponentInChildren<Text>();
            if (text != null)
                text.text = $"{combatant.DisplayName}";
            activeEntries.Add(entry);
        }
    }
    
    public void RemoveCombatantFromOrder(ICombatant combatant)
    {
        // Find the UI entry matching the combatant’s name
        GameObject entryToRemove = null;

        foreach (var entry in activeEntries)
        {
            var text = entry.GetComponentInChildren<Text>();
            if (text != null && text.text == combatant.DisplayName)
            {
                entryToRemove = entry;
                break;
            }
        }

        if (entryToRemove != null)
        {
            activeEntries.Remove(entryToRemove);
            Destroy(entryToRemove);
        }
    }
}
