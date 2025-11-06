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

    private List<ICombatant> turnOrder = new List<ICombatant>();
    private int currentTurnIndex = 0;
    private bool battleActive = false;

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
        StartRound();
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
    
    void StartRound()
    {
        Debug.Log("=== Starting New Round ===");

        turnOrder.Clear();

        // Collect all combatants in the battle
        var allCombatants = new List<ICombatant>();
        allCombatants.AddRange(battleSpawnArea.GetComponentsInChildren<ICombatant>());
        allCombatants.AddRange(allySpawnArea.GetComponentsInChildren<ICombatant>());

        foreach (var c in allCombatants)
        {
            if (c.IsAlive)
                turnOrder.Add(c);
        }

        // Sort by attack speed descending
        turnOrder.Sort((a, b) => b.CalculateAttackSpeed().CompareTo(a.CalculateAttackSpeed()));

        Debug.Log("Turn order for this round:");
        foreach (var c in turnOrder)
            Debug.Log($"{c.DisplayName} (Speed: {c.CalculateAttackSpeed()})");

        currentTurnIndex = 0;

        //if (turnOrder.Count > 0)
            //StartCoroutine(RunTurnOrder());
    }

    
    void SpawnBattleEnemies(int count)
    {
        if (battleEnemyPrefab == null || battleSpawnArea == null)
        {
            Debug.LogWarning("Missing battle enemy prefab or spawn area!");
            return;
        }

        // Clear old enemies
        foreach (Transform child in battleSpawnArea)
            Destroy(child.gameObject);

        if (count <= 0)
            return;

        // Define arc and radius
        float baseRadius = enemyRadius;
        float minArc = 60f;
        float maxArc = 140f;
        float totalArcAngle = Mathf.Lerp(minArc, maxArc, (count - 1) / 2f);
        float startAngle = -totalArcAngle / 2f;
        float angleStep = count > 1 ? totalArcAngle / (count - 1) : 0f;

        for (int i = 0; i < count; i++)
        {
            float angle = startAngle + angleStep * i;
            float rad = angle * Mathf.Deg2Rad;

            // Local semicircle offset (in front of battleSpawnArea)
            // Using negative Z here so enemies are in front of their spawn area
            Vector3 localOffset = new Vector3(Mathf.Sin(rad), 0, Mathf.Cos(rad)) * baseRadius;
            Vector3 spawnPos = battleSpawnArea.position + battleSpawnArea.TransformDirection(localOffset);

            GameObject enemy = Instantiate(battleEnemyPrefab, spawnPos, Quaternion.identity, battleSpawnArea);

            // Make enemies face toward the player
            if (playerBattleTransform != null)
            {
                Vector3 focusPoint = playerBattleTransform.position;
                Vector3 dir = (focusPoint - enemy.transform.position).normalized;
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

            // Make them look toward the enemy / battle center
            if (playerBattleTransform != null)
            {
                Vector3 focusPoint = playerBattleTransform.position + playerBattleTransform.forward * 5f; // adjust 5f to taste
                ally.transform.LookAt(new Vector3(focusPoint.x, ally.transform.position.y, focusPoint.z));
            }
        }
    }




}
