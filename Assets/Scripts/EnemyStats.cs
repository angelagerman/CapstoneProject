using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyStats", menuName = "RPG/Enemy Stats/Base")]
public class EnemyStats : ScriptableObject
{
    [Header("General Info")]
    public string enemyType;
    //public Sprite portrait; //(tbd)

    [Header("Base Stats")]
    public int level;
    public int maxHealth;
    public int attack;
    public int magic;
    public int defense;
    public int speed;
    public int luck;
    
    [Header("Defeat Rewards")]
    public int experienceReward;
    public int goldReward;

    [Header("Index Flavortext")]
    [TextArea]
    public string description;
}
