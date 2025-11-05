using System.Collections.Generic;
using UnityEngine;

public class EnemyGroupManager : MonoBehaviour
{
    // Dictionary to hold enemies by danger zone number
    private Dictionary<int, List<OverworldEnemy>> dangerZoneEnemies = new Dictionary<int, List<OverworldEnemy>>();

    // Add enemy to the corresponding danger zone
    public void RegisterEnemyToZone(OverworldEnemy enemy)
    {
        if (!dangerZoneEnemies.ContainsKey(enemy.dangerZoneNumber))
        {
            dangerZoneEnemies[enemy.dangerZoneNumber] = new List<OverworldEnemy>();
        }
        dangerZoneEnemies[enemy.dangerZoneNumber].Add(enemy);
    }

    // Deactivate all enemies in the same danger zone
    public void DeactivateEnemiesInZone(int dangerZone)
    {
        if (dangerZoneEnemies.ContainsKey(dangerZone))
        {
            foreach (var enemy in dangerZoneEnemies[dangerZone])
            {
                enemy.gameObject.SetActive(false);
            }
        }
    }

    // reactivate enemies (just in case I end up deciding I need this later for some reason idk)
    public void ReactivateEnemiesInZone(int dangerZone)
    {
        if (dangerZoneEnemies.ContainsKey(dangerZone))
        {
            foreach (var enemy in dangerZoneEnemies[dangerZone])
            {
                enemy.gameObject.SetActive(true);
            }
        }
    }
    
    public int GetEnemyCountInZone(int dangerZone)
    {
        if (dangerZoneEnemies.ContainsKey(dangerZone))
            return dangerZoneEnemies[dangerZone].Count;

        return 0;
    }
}
