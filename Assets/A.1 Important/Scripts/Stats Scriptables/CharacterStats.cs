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
    public int strength;
    public int magic;
    public int defense;
    public int speed;
    public int luck;
    public bool isAlive;
    
    [Header("Level Progress")]
    public int experience;
    public int experienceToNextLevel;
    
    [Header("Index Flavortext")]
    [TextArea]
    public string description;
}