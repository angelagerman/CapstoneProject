using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponStats", menuName = "RPG/Weapon Stats/Base")]
public class WeaponStats : ScriptableObject
{
    [Header("General Info")]
    public string weaponName;
    public string weaponType;

    [Header("Stats")] 
    public int levelRequirement;
    public int weight;
    public int hitRate;
    public int might; //strength but for weapons
    
    [Header("Index Flavortext")]
    [TextArea]
    public string description;
}
