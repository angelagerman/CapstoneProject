using UnityEngine;

[System.Serializable]
public class PartyMember
{
    public GameObject allyPrefab;
    public CharacterStats stats; //the "current stats" scriptable object corresponding to the ally
    
    public string DisplayName => stats != null ? stats.characterName : "Unnamed";
}