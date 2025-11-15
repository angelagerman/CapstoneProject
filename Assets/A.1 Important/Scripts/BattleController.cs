using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

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
    public EnemyTargetSelector enemySelector;
    
    public GameObject allyActionMenu;
    public TurnOrderUI turnOrderUI;
    public AllyStatusUI allyStatusUI;
    public GameObject BattleUI;
    private List<GameObject> spawnedAllies = new();

    private bool playerHasFinishedInput = false;
    private AllyBattleActions currentActingAlly;


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
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        BattleUI.SetActive(true);
        
        Debug.Log($"Starting battle for zone {zoneNumber} with {enemyCount} enemies");
        
        CameraSwitch.SwapActiveCamera(overworldCamera, battleCamera);
        
        SpawnBattleEnemies(enemyCount);
        SpawnAllies();
        battleActive = true;
        StartRound();
    }
    
    void EndBattle()
    {
        battleActive = false;
        PlayerController.isInCombat = false;
        CameraSwitch.SwapActiveCamera(battleCamera, overworldCamera);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        BattleUI.SetActive(false);
        
        foreach (Transform child in battleSpawnArea)
        {
            Destroy(child.gameObject);
        }
        
        foreach (Transform child in allySpawnArea)
        {
            Destroy(child.gameObject);
        }

        spawnedAllies.Clear();
    }
    
    void StartRound()
    {
        Debug.Log("=== Starting New Round ===");

        turnOrder.Clear();

        var allCombatants = new List<ICombatant>();
        allCombatants.AddRange(battleSpawnArea.GetComponentsInChildren<ICombatant>());
        allCombatants.AddRange(allySpawnArea.GetComponentsInChildren<ICombatant>());

        foreach (var c in allCombatants)
        {
            if (c.IsAlive)
                turnOrder.Add(c);
        }

        // Shuffle first to randomize tie-breaking
        for (int i = turnOrder.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (turnOrder[i], turnOrder[j]) = (turnOrder[j], turnOrder[i]);
        }

        // Sort by attack speed descending
        turnOrder.Sort((a, b) => b.CalculateAttackSpeed().CompareTo(a.CalculateAttackSpeed()));

        Debug.Log("Turn order for this round:");
        foreach (var c in turnOrder)
            Debug.Log($"{c.DisplayName} (Attack Speed: {c.CalculateAttackSpeed()})");
        
        if (turnOrderUI != null)
            turnOrderUI.UpdateTurnOrderDisplay(turnOrder);

        currentTurnIndex = 0;

        if (turnOrder.Count > 0)
            StartCoroutine(RunTurnOrder());
    }
    
    IEnumerator RunTurnOrder()
    {
        Debug.Log("=== Running Turns ===");

        while (battleActive)
        {
            if (currentTurnIndex >= turnOrder.Count)
            {
                Debug.Log("Round complete. Starting next round...");
                StartRound();
                yield break;
            }

            ICombatant current = turnOrder[currentTurnIndex];
            if (current == null || !current.IsAlive)
            {
                currentTurnIndex++;
                continue;
            }

            Debug.Log($"It's {current.DisplayName}'s turn!");

            // Ally turn
            if (current is AllyBattleActions ally)
            {
                currentActingAlly = ally;
                allyStatusUI?.HighlightAlly(ally); // enlarge the active ally’s bar
                allyActionMenu?.SetActive(true);

                // Wait for player to confirm or simulate delay for now
                yield return StartCoroutine(WaitForPlayerInput());

                allyActionMenu?.SetActive(false);
            }
            // Enemy turn
            else if (current is EnemyBattleActions enemy)
            {
                yield return StartCoroutine(EnemyTakeTurn(enemy));
            }

            turnOrderUI?.RemoveCombatantFromOrder(current);
            allyStatusUI?.UpdateStatusBars();

            currentTurnIndex++;
            yield return new WaitForSeconds(0.5f);
        }
    }
    
    IEnumerator WaitForPlayerInput()
    {
        playerHasFinishedInput = false;
        yield return new WaitUntil(() => playerHasFinishedInput);
    }
    
    IEnumerator EnemyTakeTurn(EnemyBattleActions enemy)
    {
        Debug.Log($"{enemy.DisplayName} is attacking!");

        // Collect all living allies
        List<AllyBattleActions> livingAllies = new();
        foreach (var a in allySpawnArea.GetComponentsInChildren<AllyBattleActions>())
        {
            if (a.IsAlive)
                livingAllies.Add(a);
        }

        if (livingAllies.Count == 0)
        {
            Debug.Log("All allies are defeated!");
            EndBattle();
            yield break;
        }

        // Pick a random target
        var target = livingAllies[Random.Range(0, livingAllies.Count)];

        int damage;
        if (enemy.IsAHit(target))
        {
            if (enemy.CheckForCrit())
            {
                damage = enemy.CalculateAttackDamage(target) * 2;
            }
            else
            {
                damage = enemy.CalculateAttackDamage(target);
            }
        }
        else
        {
            damage = 0;
            print("miss!");
        }
        Debug.Log($"{enemy.DisplayName} attacks {target.DisplayName} for {damage} damage!");

        target.TakeDamage(damage);
        CheckBattleEnd();
        yield return new WaitForSeconds(1f);
    }
    
    public void OnAttackPressed()
    {
        var enemies = battleSpawnArea.GetComponentsInChildren<EnemyBattleActions>();
        allyActionMenu?.SetActive(false);

        enemySelector.OpenSelector(enemies, enemy =>
        {
            Debug.Log("Player selected: " + enemy.DisplayName);

            currentActingAlly.MeleeAttack(enemy);
            
            CheckBattleEnd();
            playerHasFinishedInput = true;
        });
    }
    
    private void CheckBattleEnd()
    {
        // Check if any living allies
        bool anyAlliesAlive = false;
        foreach (var ally in allySpawnArea.GetComponentsInChildren<AllyBattleActions>())
        {
            if (ally.IsAlive)
            {
                anyAlliesAlive = true;
                break;
            }
        }

        // Check if any living enemies
        bool anyEnemiesAlive = false;
        foreach (var enemy in battleSpawnArea.GetComponentsInChildren<EnemyBattleActions>())
        {
            if (enemy.IsAlive)
            {
                anyEnemiesAlive = true;
                break;
            }
        }

        if (!anyAlliesAlive)
        {
            Debug.Log("All allies are defeated!");
            EndBattle();
        }
        else if (!anyEnemiesAlive)
        {
            Debug.Log("All enemies are defeated!");
            EndBattle();
        }
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
        spawnedAllies.Clear();

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
            
            spawnedAllies.Add(ally);

            // Make them look toward the enemy / battle center
            if (playerBattleTransform != null)
            {
                Vector3 focusPoint = playerBattleTransform.position + playerBattleTransform.forward * 5f; // adjust 5f to taste
                ally.transform.LookAt(new Vector3(focusPoint.x, ally.transform.position.y, focusPoint.z));
            }
        }
        if (allyStatusUI != null)
            allyStatusUI.BuildStatusPanel(spawnedAllies);
    }
}
