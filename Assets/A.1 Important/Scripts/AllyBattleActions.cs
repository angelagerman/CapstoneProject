using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AllyBattleActions : MonoBehaviour, ICombatant
{
    public CharacterStats stats;
    public WeaponStats weapon;
    public EquipmentStats equipment;
    
    [System.Serializable]
    public class MagicSpell
    {
        public string spellName;
        public string spellElement;
        public string supportStat;
        public int might;
        public int hitRate;
        public int manaCost;
        public int levelRequirement;
        public bool isAOE;
        public bool isSupport;
    }
    
    public List<MagicSpell> magicSpells = new List<MagicSpell>();

    
    public string DisplayName => stats != null ? stats.characterName : "Unnamed Ally";
    public bool IsAlive => stats != null && stats.isAlive;

    public int CalculateAttackSpeed()
    {
        int weaponWeight  = weapon?.weight ?? 0;
        int equipWeight   = equipment?.weight ?? 0;
        int equipSpeedBuff   = equipment?.speedBuff ?? 0;
        int equipStrengthBuff   = equipment?.strengthBuff ?? 0;

        int speed = stats.speed + equipSpeedBuff;
        int strength = stats.strength + equipStrengthBuff;

        return speed - (weaponWeight + equipWeight - (strength / 5));
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
        if (MeleeHitCheck(target))
        {
            if (CheckForCrit())
            {
                damage = CalculateMeleeAttackDamage(target) * 2;
            }
            else
            {
                damage = CalculateMeleeAttackDamage(target);
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
    public void MagicAttack(EnemyBattleActions target, MagicSpell spell)
    {
        if (target == null || !target.IsAlive)
        {
            Debug.Log($"{DisplayName} tried to attack, but the target is invalid!");
            return;
        }
        
        stats.currentMagic -= spell.manaCost;
        
        int damage;
        if (MagicHitCheck(target, spell))
        {
            if (CheckForCrit())
            {
                damage = CalculateMagicAttackDamage(target, spell) * 2;
            }
            else
            {
                damage = CalculateMagicAttackDamage(target, spell);
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
    
    
    public bool MeleeHitCheck(EnemyBattleActions target)
    {
        int weaponHit  = weapon?.hitRate ?? 0;
        int equipHit   = equipment?.hitRateBuff ?? 0;
        int statHit    = (stats.speed + stats.luck) / 2;

        int hit = weaponHit + equipHit + statHit;
        int avoid = target.CalculateAttackSpeed();

        int hitRate = hit - avoid;
        
        float hitAverage = (Random.Range(0, 100) + Random.Range(0, 100)) * 0.5f;
        
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
    public bool MagicHitCheck(EnemyBattleActions target, MagicSpell spell)
    {
        int spellHit  = spell?.hitRate ?? 0;
        int equipHit   = equipment?.hitRateBuff ?? 0;
        int statHit    = (stats.speed + stats.luck) / 2;

        int hit = spellHit + equipHit + statHit;
        int avoid = target.CalculateAttackSpeed();

        int hitRate = hit - avoid;
        
        float hitAverage = (Random.Range(0, 100) + Random.Range(0, 100)) * 0.5f;
        
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
    public bool CheckForCrit()
    {
        int equipLuckBuff   = equipment?.luckBuff ?? 0;
        int luck = stats.luck + equipLuckBuff;
        
        int critRate = (CalculateAttackSpeed() + luck) / 2;
        
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
    
    
    public int CalculateMeleeAttackDamage(EnemyBattleActions target)
    {
        int weaponMight  = weapon?.might ?? 0;
        int equipStrengthBuff   = equipment?.strengthBuff ?? 0;

        return weaponMight + equipStrengthBuff + stats.strength - target.stats.defense;
    }
    public int CalculateMagicAttackDamage(EnemyBattleActions target, MagicSpell spell)
    {
        int spellMight  = spell?.might ?? 0;
        int equipMagicBuff   = equipment?.magicBuff ?? 0;

        return spellMight + equipMagicBuff + stats.magic - target.stats.defense;
    }
}
