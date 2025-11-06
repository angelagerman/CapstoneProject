using System;
using UnityEngine;

public class EnemyBattleActions : MonoBehaviour, ICombatant
{
    public EnemyStats stats;
    public int currentHealth;
    
    public string DisplayName => stats != null ? stats.enemyName : "Unnamed Enemy";
    private bool isAlive = true;
    public bool IsAlive => isAlive;

    void Start()
    {
        isAlive = true;
        currentHealth = stats.maxHealth;
    }

    public int CalculateAttackSpeed()
    {
        return stats.speed - (stats.weight - (stats.strength / 5));
    }
}
