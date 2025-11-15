using UnityEngine;

[CreateAssetMenu(fileName = "NewEquipmentStats", menuName = "RPG/Equipment Stats/Base")]
public class EquipmentStats : ScriptableObject
{
    [Header("General Info")]
    public string EquipmentName;

    [Header("Stats")] 
    public int levelRequirement;
    public int weight;
    public int hitRateBuff;
    public int strengthBuff;
    public int magicBuff;
    public int luckBuff;
    public int defenseBuff;
    public int speedBuff;
    
    [Header("Index Flavortext")]
    [TextArea]
    public string description;
}
