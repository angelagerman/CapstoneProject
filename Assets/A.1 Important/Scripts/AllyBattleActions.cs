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
            stats.isAlive = false;
        }
    }
    
    public void MeleeAttack(EnemyBattleActions target)
    {
        if (target == null || !target.IsAlive)
        {
            Debug.Log($"{DisplayName} tried to attack, but the target is invalid!");
            return;
        }
        
        bool crit = false;
        bool miss = false;
        int damage;
        if (MeleeHitCheck(target))
        {
            if (CheckForCrit())
            {
                damage = CalculateMeleeAttackDamage(target) * 2;
                crit = true;
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
            miss = true;
        }
        
        Debug.Log($"{DisplayName} attacks {target.DisplayName} for {damage} damage!");
        target.TakeDamage(damage);
        
        DamageTextManager.Instance.SpawnDamageText(
            target.transform,
            damage,
            crit
        );

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
        
        bool crit = false;
        int damage;
        if (MagicHitCheck(target, spell))
        {
            if (CheckForCrit())
            {
                damage = CalculateMagicAttackDamage(target, spell) * 2;
                crit = true;
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
        
        DamageTextManager.Instance.SpawnDamageText(
            target.transform,
            damage,
            crit
        );

        // add animation triggers here later
    }

    public void Heal(AllyBattleActions target, MagicSpell spell)
    {
        if (target == null || !target.IsAlive)
        {
            Debug.Log($"{DisplayName} tried to attack, but the target is invalid!");
            return;
        }
        
        int healStrength = spell.might + ( stats.magic /3 );
        int newHealth = target.stats.currentHealth + healStrength;

        if (newHealth < target.stats.maxHealth)
        {
            target.stats.currentHealth = newHealth;
        }
        else
        {
            target.stats.currentHealth = target.stats.maxHealth;
        }
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

    public void CheckLevelUpWithUI(LevelUpManager levelUpManager, System.Action onComplete)
    {
        // Only process ONE level-up per call
        if (stats.experience >= stats.experienceToNextLevel)
        {
            LevelUpWithUI(levelUpManager, onComplete);
        }
        else
        {
            onComplete?.Invoke(); // No level-up, continue
        }
    }

    private void LevelUpWithUI(LevelUpManager levelUpManager, System.Action onComplete)
    {
        // Track old stats
        Dictionary<string, int> statIncreases = new();
        int oldMaxHealth = stats.maxHealth;
        int oldStrength = stats.strength;
        int oldMagic = stats.magic;
        int oldDefense = stats.defense;
        int oldSpeed = stats.speed;
        int oldLuck = stats.luck;

        // Subtract ONLY the XP required for this level
        stats.experience -= stats.experienceToNextLevel;
        stats.level++;

        TryIncreaseStat(ref stats.maxHealth, stats.hpGrowth);
        TryIncreaseStat(ref stats.strength, stats.strengthGrowth);
        TryIncreaseStat(ref stats.magic, stats.magicGrowth);
        TryIncreaseStat(ref stats.defense, stats.defenseGrowth);
        TryIncreaseStat(ref stats.speed, stats.speedGrowth);
        TryIncreaseStat(ref stats.luck, stats.luckGrowth);

        // Record increases
        statIncreases["HP"] = stats.maxHealth - oldMaxHealth;
        statIncreases["Strength"] = stats.strength - oldStrength;
        statIncreases["Magic"] = stats.magic - oldMagic;
        statIncreases["Defense"] = stats.defense - oldDefense;
        statIncreases["Speed"] = stats.speed - oldSpeed;
        statIncreases["Luck"] = stats.luck - oldLuck;

        // Check for newly unlocked spells at this level
        List<MagicSpell> newSpells = new();
        foreach (var spell in magicSpells)
        {
            if (spell.levelRequirement <= stats.level && !stats.knownSpells.Contains(spell.spellName))
            {
                newSpells.Add(spell);
                stats.knownSpells.Add(spell.spellName); // mark as learned
            }
        }

        // Show UI and wait for player input before doing any more
        levelUpManager.ShowLevelUp(this, statIncreases, newSpells, onComplete);

        Debug.Log($"{stats.characterName} leveled up to {stats.level}!");
    }

    private void TryIncreaseStat(ref int stat, int growthRate)
    {
        int roll = Random.Range(0, 100); // 0-99
        if (roll < growthRate)
        {
            stat += 1;
            Debug.Log($"Stat increased! New value: {stat}");
        }
    }
}
