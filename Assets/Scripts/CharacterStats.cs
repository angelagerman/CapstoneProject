using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterStats", menuName = "RPG/Character Stats/Base")]
public class CharacterStats : ScriptableObject
{
    [Header("General Info")]
    public string characterName;
    //public Sprite portrait; //(tbd)

    [Header("Base Stats")]
    public int level;
    public int maxHealth;
    public int attack;
    public int magic;
    public int defense;
    public int speed;
    public int luck;
    
    [Header("Level Progress")]
    public int experience;
    public int experienceToNextLevel;
    
    [Header("Index Flavortext")]
    [TextArea]
    public string description;
}