using System.Collections.Generic;
using UnityEngine;

public class AllyStatusUI : MonoBehaviour
{
    public Transform statusContainer;
    public GameObject statusEntryPrefab;

    private readonly List<AllyStatusEntry> activeEntries = new();

    public void BuildStatusPanel(List<GameObject> allyObjects)
    {
        // Clear old entries
        foreach (var entry in activeEntries)
            Destroy(entry.gameObject);
        activeEntries.Clear();

        foreach (var allyObj in allyObjects)
        {
            if (allyObj == null)
                continue;

            var allyStats = allyObj.GetComponent<AllyBattleActions>();
            if (allyStats == null)
                continue;

            var entryObj = Instantiate(statusEntryPrefab, statusContainer);
            var entry = entryObj.GetComponent<AllyStatusEntry>();
            if (entry != null)
            {
                entry.Initialize(allyStats); // pass the AllyBattleActions now
                activeEntries.Add(entry);
            }
        }
    }

    public void UpdateStatusBars()
    {
        foreach (var entry in activeEntries)
        {
            if (entry != null)
                entry.Refresh();
        }
    }
    
    public void HighlightAlly(AllyBattleActions activeAlly)
    {
        foreach (var entry in activeEntries)
        {
            if (entry == null) continue;

            bool isThisOne = (entry.GetAlly() == activeAlly);
            entry.SetHighlighted(isThisOne);
        }
    }

}
