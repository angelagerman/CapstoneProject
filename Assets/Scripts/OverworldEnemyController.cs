using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public float speed;
    public float idleSpeed;
    public int dangerZoneNumber;

    public PlayerController player;
    
    //bro is vibing. in his lane. unbothered king. go off evil and intimidating horse.
    private Vector3 spawnPosition;
    private Vector3 wanderTarget;
    private bool isReturningToSpawn = false;
    private bool isWandering = false;

    public float wanderRadius = 5f;
    public float idleWaitTime = 2f;
    
    private Animator animator;
    private Vector3 lastPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        spawnPosition = transform.position;
        StartCoroutine(WanderRoutine());
        
        animator = GetComponentInChildren<Animator>();
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null && player.isInZone)
        {
            if (player.DangerZone == dangerZoneNumber)
            {
                StopCoroutine(WanderRoutine()); //lock in
                isReturningToSpawn = false;
                isWandering = false;
                
                Vector3 direction = (target.position - transform.position).normalized;
                transform.position += direction * speed * Time.deltaTime;

                // Rotate toward player
                RotateTowards(direction);
            }
        }
        else if (!player.isInZone || player.DangerZone != dangerZoneNumber)
        {
            if (!isWandering)
            {
                float distanceToSpawn = Vector3.Distance(transform.position, spawnPosition);

                if (distanceToSpawn > wanderRadius + 0.5f)
                {
                    // Go back to spawn if too far from home I don't want him to get lost
                    isReturningToSpawn = true;
                    Vector3 direction = (spawnPosition - transform.position).normalized;
                    transform.position += direction * speed * Time.deltaTime;
                    RotateTowards(direction);

                    if (distanceToSpawn < 0.1f)
                    {
                        isReturningToSpawn = false;
                        StartCoroutine(WanderRoutine());
                    }
                }
            }

            if (isWandering)
            {
                Vector3 direction = (wanderTarget - transform.position).normalized;
                transform.position += direction * idleSpeed * Time.deltaTime;
                RotateTowards(direction);

                if (Vector3.Distance(transform.position, wanderTarget) < 0.2f)
                {
                    isWandering = false;
                    StartCoroutine(WanderRoutine());
                }
            }
        }
        
        float movementSpeed = (transform.position - lastPosition).magnitude / Time.deltaTime;
        lastPosition = transform.position;

        if (animator != null)
        {
            animator.SetFloat("Speed", movementSpeed);
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform == target)
        {
            Rigidbody targetRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            if (targetRigidbody != null)
            {
                print("ow");
            }
        }
    }
    
    void RotateTowards(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
        }
    }
    
    IEnumerator WanderRoutine()
    {
        yield return new WaitForSeconds(idleWaitTime);

        // Pick a random point near the spawn within the wander radius. go my horse.
        Vector2 randomPoint = Random.insideUnitCircle * wanderRadius;
        wanderTarget = spawnPosition + new Vector3(randomPoint.x, 0, randomPoint.y);
        isWandering = true;
    }

}
