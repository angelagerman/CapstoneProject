using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public CameraSwitch CameraSwitch;

    public Camera overworldCamera;
    public Camera battleCamera;
    
    public List<PartyMember> partyMembers = new List<PartyMember>();
    public Transform allySpawnArea;
    public Transform playerBattleTransform;
    
    public GameObject battleEnemyPrefab;
    public Transform battleSpawnArea;
    
    public float enemyRadius = 4f;
    public float allyRadius = 3f;
    // radiuses to spread The Gang out

    private int currentZone;
    private int currentEnemyCount;

    public PlayerController PlayerController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            EndBattle();
        }
    }

    public void StartBattle(int zoneNumber, int enemyCount)
    {
        currentZone = zoneNumber;
        currentEnemyCount = enemyCount;
        
        Debug.Log($"Starting battle for zone {zoneNumber} with {enemyCount} enemies");
        
        CameraSwitch.SwapActiveCamera(overworldCamera, battleCamera);
        
        SpawnBattleEnemies(enemyCount);
        SpawnAllies();
    }
    
    void EndBattle()
    {
        PlayerController.isInCombat = false;
        CameraSwitch.SwapActiveCamera(battleCamera, overworldCamera);
        
        foreach (Transform child in battleSpawnArea)
        {
            Destroy(child.gameObject);
        }
    }
    
    void SpawnBattleEnemies(int count)
    {
        if (battleEnemyPrefab == null || battleSpawnArea == null)
        {
            Debug.LogWarning("Missing battle enemy prefab or spawn area!");
            return;
        }

        // Clear previous battle enemies
        foreach (Transform child in battleSpawnArea)
            Destroy(child.gameObject);

        float radius = 4f; // radius of semicircle
        float angleStep = 180f / (count + 1);

        for (int i = 0; i < count; i++)
        {
            float angle = -90f + angleStep * (i + 1);
            float rad = angle * Mathf.Deg2Rad;

            Vector3 offset = new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad)) * radius;
            Vector3 spawnPos = battleSpawnArea.position + offset;

            GameObject enemy = Instantiate(battleEnemyPrefab, spawnPos, Quaternion.identity, battleSpawnArea);

            // Face the player
            if (playerBattleTransform != null)
            {
                Vector3 dir = (playerBattleTransform.position - spawnPos).normalized;
                dir.y = 0;
                enemy.transform.rotation = Quaternion.LookRotation(dir);
            }
        }
    }
    
    void SpawnAllies()
    {
        foreach (Transform child in allySpawnArea)
            Destroy(child.gameObject);

        int count = Mathf.Min(partyMembers.Count, 4);
        if (count == 0)
        {
            Debug.Log("No allies to spawn.");
            return;
        }

        float baseRadius = 3.5f;     
        float minArc = 60f;          
        float maxArc = 140f;         

        float totalArcAngle = Mathf.Lerp(minArc, maxArc, (count - 1) / 2f);
        float startAngle = -totalArcAngle / 2f;
        float angleStep = count > 1 ? totalArcAngle / (count - 1) : 0f;

        for (int i = 0; i < count; i++)
        {
            var member = partyMembers[i];
            if (member == null || member.allyPrefab == null)
                continue;

            float angle = startAngle + angleStep * i;
            float rad = angle * Mathf.Deg2Rad;

            // Original local offset in X/Z plane
            Vector3 localOffset = new Vector3(Mathf.Sin(rad), 0, -Mathf.Cos(rad)) * baseRadius;

            // Rotate offset relative to player's facing
            Vector3 spawnPos = allySpawnArea.position + playerBattleTransform.TransformDirection(localOffset);

            GameObject ally = Instantiate(member.allyPrefab, spawnPos, Quaternion.identity, allySpawnArea);
            ally.name = member.DisplayName;

            // Make them look toward the enemy / battle center
            if (playerBattleTransform != null)
            {
                Vector3 focusPoint = playerBattleTransform.position + playerBattleTransform.forward * 5f; // adjust 5f to taste
                ally.transform.LookAt(new Vector3(focusPoint.x, ally.transform.position.y, focusPoint.z));
            }

            Debug.Log($"Spawned ally: {member.DisplayName} at {angle:F1}°");
        }
    }




}
