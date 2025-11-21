using UnityEngine;

[CreateAssetMenu(fileName = "AllCharacters", menuName = "Game/AllCharacters")]
public class AllCharacters : ScriptableObject
{
    public GameObject[] characterPrefabs;
}