using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerZone : MonoBehaviour
{
    public int zoneNumber;
    public int chaseTime;

    public PlayerController playerValues;

    public GameObject player;


    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            playerValues.isInZone = true;
            playerValues.DangerZone = zoneNumber;
            print("hi welcome to zone " + playerValues.DangerZone);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            StartCoroutine(WaitToStopChase());  
        }
    }
    
    private IEnumerator WaitToStopChase()
    {
        yield return new WaitForSeconds(chaseTime);
        playerValues.isInZone = false;
        playerValues.DangerZone = 0;
        print("oh okay bye :(");
    }
}
