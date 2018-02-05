// Chris Blanchard
// Tuesday 1/23/2018
// Script for an enemy basic dash attack within a defined range
// Includes follow script as well for now
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    Transform mainPlayer;
    Slider playerHp;

    public float dashDist = 0.5f;

    private float speed = 4.1f;
    private float minDist = 3f;         // Minimum distance the ghost will follow the player to
    private float maxDist = 3f;        // Maximum distance the ghost will attack the player at

    private bool atkd = false;          // Bool for if the enemy has recently attacked
    private bool miss = true;          // Bool for if enemy has missed the player

    void Start ()
    {
        // Sets enemy to follow the player with the flashlight
        mainPlayer = GameObject.FindGameObjectWithTag("Player").transform;
        playerHp = mainPlayer.GetComponent<CharacterHealth>().healthSlider;
    }
	
	void Update ()
    {
        if(!atkd)
        {
            transform.LookAt(mainPlayer);
        }

        
        // If the enemy is outside of minDist follow the player
        if(Vector3.Distance(transform.position, mainPlayer.position) >= minDist)
        {
            if(atkd && miss)
            {
                StartCoroutine("AtkWaitTime");
            }
            transform.position += transform.forward * speed * Time.deltaTime;
            if (miss)
            {
                atkd = false;
            }
        }

        // Find if the ghost is in attack range and attack
        if (Vector3.Distance(transform.position, mainPlayer.position) <= maxDist)
        {
            DashAttack();
            atkd = true;
        }
    }

    public void DashAttack()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * dashDist); // Controls Dash movement
    }

    public void OnTriggerExit(Collider col)
    {
        if(col.tag == "Player")
        {
            miss = false;
            mainPlayer.GetComponent<CharacterHealth>().TakeDamage();
        }
    }

    IEnumerator AtkWaitTime()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        atkd = false;
    }
}
