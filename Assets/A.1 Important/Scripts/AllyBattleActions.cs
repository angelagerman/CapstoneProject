using UnityEngine;

public class AllyBattleActions : MonoBehaviour, ICombatant
{
    public CharacterStats stats;
    public WeaponStats weapon;
    public EquipmentStats equipment;
    
    public string DisplayName => stats != null ? stats.characterName : "Unnamed Ally";
    public bool IsAlive => stats != null && stats.isAlive;

    public int CalculateAttackSpeed()
    {
        if (weapon != null)
        {
            if (equipment != null)
            {
                return stats.speed - (weapon.weight + equipment.weight - (stats.strength / 5));
            }
            else
            {
                return stats.speed - (weapon.weight - (stats.strength / 5));
            }
        }
        else
        {
            return stats.speed + (stats.strength / 5);
        }
    }

    public void ReviveForBattle()
    {
        if (!stats.isAlive)
        {
            stats.currentHealth = Mathf.Max(1, stats.maxHealth / 5);
            stats.isAlive = true;
        }
    }
    
    public void TakeDamage(int amount)
    {
        stats.currentHealth -= amount;
        if (stats.currentHealth < 0)
        {
            stats.currentHealth = 0;
        }
    }
}
