using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterStats", menuName = "RPG/Character Stats/Base")]
public class CharacterStats : ScriptableObject
{
    [Header("General Info")]
    public string characterName;
    public string weaponProficiency;
    //public Sprite portrait; //(tbd)

    [Header("Base Stats")]
    public int level;
    public int maxHealth;
    public int currentHealth;
    public int currentMagic;
    public int strength;
    public int magic;
    public int defense;
    public int speed;
    public int luck;
    public bool isAlive;

    [Header("Stat Growths")] 
    public int hpGrowth;
    public int strengthGrowth;
    public int magicGrowth;
    public int defenseGrowth;
    public int speedGrowth;
    public int luckGrowth;
    
    [Header("Level Progress")]
    public int experience;
    public int experienceToNextLevel;
    
    [Header("Index Flavortext")]
    [TextArea]
    public string description;
    
    public void CopyFrom(CharacterStats starting)
    {
        characterName = starting.characterName;
        weaponProficiency = starting.weaponProficiency;
        level = starting.level;
        maxHealth = starting.maxHealth;
        currentHealth = maxHealth;
        currentMagic = maxHealth;
        strength = starting.strength;
        magic = starting.magic;
        defense = starting.defense;
        speed = starting.speed;
        luck = starting.luck;
        isAlive = starting.isAlive;
        experience = starting.experience;
        experienceToNextLevel = starting.experienceToNextLevel;
        description = starting.description;
        hpGrowth = starting.hpGrowth;
        strengthGrowth = starting.strengthGrowth;
        magicGrowth = starting.magicGrowth;
        defenseGrowth = starting.defenseGrowth;
        speedGrowth = starting.speedGrowth;
        luckGrowth = starting.luckGrowth;
    }
}