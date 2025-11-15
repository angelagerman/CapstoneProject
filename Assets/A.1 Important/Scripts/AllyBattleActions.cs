using Unity.VisualScripting;
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
    
    public void MeleeAttack(EnemyBattleActions target)
    {
        if (target == null || !target.IsAlive)
        {
            Debug.Log($"{DisplayName} tried to attack, but the target is invalid!");
            return;
        }
        
        int damage;
        if (IsAHit(target))
        {
            if (CheckForCrit())
            {
                damage = CalculateAttack(target, "melee") * 2;
            }
            else
            {
                damage = CalculateAttack(target, "melee");
            }
        }
        else
        {
            damage = 0;
            print("miss!");
        }
        
        Debug.Log($"{DisplayName} attacks {target.DisplayName} for {damage} damage!");
        target.TakeDamage(damage);

        // add animation triggers here later
    }

    public bool IsAHit(EnemyBattleActions target)
    {
        int weaponHit  = weapon?.hitRate ?? 0;
        int equipHit   = equipment?.hitRateBuff ?? 0;
        int statHit    = (stats.speed + stats.luck) / 2;

        int hit = weaponHit + equipHit + statHit;
        int avoid = target.CalculateAttackSpeed();

        int hitRate = hit - avoid;
        
        int rand1 = Random.Range(0, 100);
        int rand2 = Random.Range(0, 100);
        float hitAverage = (rand1 + rand2) / 2f;

        if (hitRate > hitAverage)
        {
            return true;
        }
        else
        {
            return false;
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
    public int CalculateAttack(EnemyBattleActions target, string attackType)
    {
        int weaponMight  = weapon?.might ?? 0;
        int equipbuff   = equipment?.strengthBuff ?? 0;

        if (attackType == "melee")
        {
            return weaponMight + equipbuff + stats.strength - target.stats.defense;
        }
        else if (attackType == "magic")
        {
            return weaponMight + equipbuff + stats.magic - target.stats.defense;
        }
        else
        {
            print("you're bad at spelling bro");
            return 0;
        }
    }
}
