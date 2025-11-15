using System;
using UnityEngine;
using Random = UnityEngine.Random;

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
    
    public int CalculateAttackDamage(AllyBattleActions target)
    {
        int equipDefenseBuff  = target.equipment?.defenseBuff ?? 0;
        int targetDef = equipDefenseBuff + target.stats.defense;

        return stats.strength - targetDef;
    }
    
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isAlive = false;
        }
    }
    public bool CheckForCrit()
    {
        int critRate = (CalculateAttackSpeed() + stats.luck) / 2;
        
        int rand1 = Random.Range(0, 100);
        int rand2 = Random.Range(0, 100);
        float rollAverage = (rand1 + rand2) / 2f;
        
        if (critRate > rollAverage)
        {
            print("critical hit!");
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool IsAHit(AllyBattleActions target)
    {
        int hit = (stats.speed + stats.luck) / 2;
        int avoid = target.CalculateAttackSpeed();

        int hitRate = stats.hitRate + hit - avoid;

        int rand1 = Random.Range(0, 100);
        int rand2 = Random.Range(0, 100);
        float hitAverage = (rand1 + rand2) / 2f;
        
        print("Hit rate: " + hitRate + ", Hit average: " + hitAverage);

        if (hitRate > hitAverage)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
