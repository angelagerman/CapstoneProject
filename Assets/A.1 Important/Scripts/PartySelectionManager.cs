using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PartySelectionManager : MonoBehaviour
{
    public Dropdown[] partyDropdowns;
    public AllCharacters allCharacters;
    public GameObject playerPrefab;
    private GameObject[] currentSelection;
    private List<GameObject>[] dropdownData;
    public BattleController battleController;

    private void Start()
    {
        int slotCount = partyDropdowns.Length;
        currentSelection = new GameObject[slotCount];
        dropdownData = new List<GameObject>[slotCount];

        for (int i = 0; i < slotCount; i++)
        {
            int index = i;

            if (i == 2) // Player slot
            {
                currentSelection[i] = playerPrefab;
                partyDropdowns[i].interactable = false;
                dropdownData[i] = new List<GameObject> { playerPrefab };
                partyDropdowns[i].options.Clear();
                partyDropdowns[i].options.Add(new Dropdown.OptionData(GetCharacterName(playerPrefab)));
                partyDropdowns[i].value = 0;
                continue;
            }

            PopulateDropdown(index);
            partyDropdowns[i].onValueChanged.AddListener(value => UpdateSelection(index, value));
        }
    }

    private void PopulateDropdown(int slotIndex)
    {
        Dropdown dropdown = partyDropdowns[slotIndex];
        GameObject previousSelection = currentSelection[slotIndex];
        dropdown.onValueChanged.RemoveAllListeners();

        dropdown.ClearOptions();
        List<string> options = new List<string>();
        List<GameObject> correspondingPrefabs = new List<GameObject>();

        // Add "None" option at index 0
        options.Add("None");
        correspondingPrefabs.Add(null);

        foreach (var prefab in allCharacters.characterPrefabs)
        {
            if (prefab == playerPrefab) continue;

            // Skip characters already selected in other slots
            bool alreadySelected = false;
            for (int i = 0; i < currentSelection.Length; i++)
            {
                if (i != slotIndex && currentSelection[i] == prefab)
                {
                    alreadySelected = true;
                    break;
                }
            }
            if (alreadySelected) continue;

            correspondingPrefabs.Add(prefab);
            options.Add(GetCharacterName(prefab));
        }

        dropdown.AddOptions(options);
        dropdownData[slotIndex] = correspondingPrefabs;

        // Restore previous selection if still valid
        int prevIndex = previousSelection != null ? correspondingPrefabs.IndexOf(previousSelection) : 0; // default to "None"
        dropdown.value = prevIndex >= 0 ? prevIndex : 0;
        currentSelection[slotIndex] = correspondingPrefabs[dropdown.value];
        dropdown.onValueChanged.AddListener(value => UpdateSelection(slotIndex, value));
    }


    private void UpdateSelection(int slotIndex, int optionIndex)
    {
        if (optionIndex < 0 || optionIndex >= dropdownData[slotIndex].Count) return;

        // Allow null selection for "None"
        currentSelection[slotIndex] = dropdownData[slotIndex][optionIndex];

        // Refresh other dropdowns to remove duplicates (skip player slot)
        for (int i = 0; i < partyDropdowns.Length; i++)
        {
            if (i != slotIndex && i != 2) // skip player slot
            {
                PopulateDropdown(i);
            }
        }
    }


    private string GetCharacterName(GameObject prefab)
    {
        AllyBattleActions ally = prefab.GetComponent<AllyBattleActions>();
        return ally != null && ally.stats != null ? ally.stats.characterName : "Unnamed";
    }

    public void OnConfirmParty()
    {
        GameObject[] selectedParty = GetSelectedParty();
        battleController.SetParty(selectedParty);

        Debug.Log("Party confirmed!");
    }


    public GameObject[] GetSelectedParty()
    {
        return currentSelection;
    }

}
