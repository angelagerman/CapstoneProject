using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueButton : MonoBehaviour
{
    public CharacterStats[] startingStats;
    public CharacterStats[] currentStats;
    public string sceneToLoad;

    public void ContinueGame()
    {
        for (int i = 0; i < currentStats.Length; i++)
        {
            SaveSystem.LoadCharacter(currentStats[i], startingStats[i]);
        }

        SceneManager.LoadScene(sceneToLoad);
    }
}
