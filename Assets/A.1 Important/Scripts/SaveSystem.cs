using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static void SaveCharacter(CharacterStats stats)
    {
        string charID = stats.characterName;
        
        PlayerPrefs.SetInt($"{charID}_Level", stats.level);
        PlayerPrefs.SetInt($"{charID}_MaxHealth", stats.maxHealth);
        PlayerPrefs.SetInt($"{charID}_CurrentHealth", stats.currentHealth);
        PlayerPrefs.SetInt($"{charID}_CurrentMagic", stats.currentMagic);
        PlayerPrefs.SetInt($"{charID}_Strength", stats.strength);
        PlayerPrefs.SetInt($"{charID}_Magic", stats.magic);
        PlayerPrefs.SetInt($"{charID}_Defense", stats.defense);
        PlayerPrefs.SetInt($"{charID}_Speed", stats.speed);
        PlayerPrefs.SetInt($"{charID}_Luck", stats.luck);
        PlayerPrefs.SetInt($"{charID}_IsAlive", stats.isAlive ? 1 : 0);
        PlayerPrefs.SetInt($"{charID}_Experience", stats.experience);
        PlayerPrefs.SetInt($"{charID}_ExperienceToNextLevel", stats.experienceToNextLevel);
        PlayerPrefs.SetInt($"{charID}_HPGrowth", stats.hpGrowth);
        PlayerPrefs.SetInt($"{charID}_StrengthGrowth", stats.strengthGrowth);
        PlayerPrefs.SetInt($"{charID}_MagicGrowth", stats.magicGrowth);
        PlayerPrefs.SetInt($"{charID}_DefenseGrowth", stats.defenseGrowth);
        PlayerPrefs.SetInt($"{charID}_SpeedGrowth", stats.speedGrowth);
        PlayerPrefs.SetInt($"{charID}_LuckGrowth", stats.luckGrowth);

        PlayerPrefs.Save();
    }
    
    public static void LoadCharacter(CharacterStats current, CharacterStats starting)
    {
        string charID = current.characterName;
        
        current.characterName = starting.characterName;
        current.weaponProficiency = starting.weaponProficiency;
        current.description = starting.description;

        current.level = PlayerPrefs.GetInt($"{charID}_Level", starting.level);
        current.maxHealth = PlayerPrefs.GetInt($"{charID}_MaxHealth", starting.maxHealth);
        current.currentHealth = PlayerPrefs.GetInt($"{charID}_CurrentHealth", starting.maxHealth);
        current.currentMagic = PlayerPrefs.GetInt($"{charID}_CurrentMagic", starting.maxHealth);
        current.strength = PlayerPrefs.GetInt($"{charID}_Strength", starting.strength);
        current.magic = PlayerPrefs.GetInt($"{charID}_Magic", starting.magic);
        current.defense = PlayerPrefs.GetInt($"{charID}_Defense", starting.defense);
        current.speed = PlayerPrefs.GetInt($"{charID}_Speed", starting.speed);
        current.luck = PlayerPrefs.GetInt($"{charID}_Luck", starting.luck);
        current.isAlive = PlayerPrefs.GetInt($"{charID}_IsAlive", 1) == 1;
        current.experience = PlayerPrefs.GetInt($"{charID}_Experience", starting.experience);
        current.experienceToNextLevel = PlayerPrefs.GetInt($"{charID}_ExperienceToNextLevel", starting.experienceToNextLevel);
        current.hpGrowth = PlayerPrefs.GetInt($"{charID}_HPGrowth", starting.hpGrowth);
        current.strengthGrowth = PlayerPrefs.GetInt($"{charID}_StrengthGrowth", starting.strengthGrowth);
        current.magicGrowth = PlayerPrefs.GetInt($"{charID}_MagicGrowth", starting.magicGrowth);
        current.defenseGrowth = PlayerPrefs.GetInt($"{charID}_DefenseGrowth", starting.defenseGrowth);
        current.speedGrowth = PlayerPrefs.GetInt($"{charID}_SpeedGrowth", starting.speedGrowth);
        current.luckGrowth = PlayerPrefs.GetInt($"{charID}_LuckGrowth", starting.luckGrowth);
    }
    
    public static bool HasSave(string charID)
    {
        return PlayerPrefs.HasKey($"{charID}_CurrentHealth"); // any key to check
    }

    public static void ClearSave()
    {
        PlayerPrefs.DeleteAll();
    }
}
