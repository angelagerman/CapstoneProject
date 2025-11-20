using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameButton : MonoBehaviour
{
    public CharacterStats[] startingStats;
    public CharacterStats[] currentStats;

    public void StartNewGame()
    {
        SaveSystem.ClearSave();

        Debug.Log("=== NEW GAME STARTED ===");

        if (startingStats.Length != currentStats.Length)
        {
            Debug.LogError("ERROR: startingStats and currentStats arrays have different lengths!");
            return;
        }

        for (int i = 0; i < currentStats.Length; i++)
        {
            if (startingStats[i] == null)
            {
                Debug.LogError($"StartingStats[{i}] is NULL!");
                continue;
            }
            if (currentStats[i] == null)
            {
                Debug.LogError($"CurrentStats[{i}] is NULL!");
                continue;
            }

            Debug.Log($"Copying {startingStats[i].characterName} → Current");

            currentStats[i].CopyFrom(startingStats[i]);

            Debug.Log($"Resulting health: {currentStats[i].currentHealth}");
        }

        SceneManager.LoadScene("OverworldScene");
    }
}
