using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyStats", menuName = "RPG/Enemy Stats/Base")]
public class EnemyStats : ScriptableObject
{
    [Header("General Info")]
    public string enemyName;
    //public Sprite portrait; //(tbd)

    [Header("Base Stats")]
    public int level;
    public int maxHealth;
    public int strength;
    public int magic;
    public int defense;
    public int speed;
    public int luck;
    public int weight;
    public int hitRate;
    
    [Header("Defeat Rewards")]
    public int experienceReward;
    public int goldReward;

    [Header("Index Flavortext")]
    [TextArea]
    public string description;
}
