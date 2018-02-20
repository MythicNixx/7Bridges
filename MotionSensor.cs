// Chris Blanchard
// Version 2
// Tuesday 2/19/2018
// Motion Sensor Enemy Script with Nav Mesh

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor.AI;

public class MotionSensor : MonoBehaviour
{
    public Transform target;
    public NavMeshAgent agent;

    public bool visible = false;   // Determines if player is visible or not
    public bool triggered = false; // Player has triggred the enemy
    public bool stopped;

    void Awake()
    {
        stopped = false;
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    
    void Update()
    {
        Search();
        if (stopped == true)
        {
            StartCoroutine(StopEnemy());
        }
    }

    void Search()
    {
        Rigidbody targetRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        Collider[] hits = Physics.OverlapSphere(transform.position, 10f);
        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, target.position - transform.position, out hit, 10f))
            {
                if (hit.collider.tag == "Player")
                {
                    visible = true;
                }

                else
                    visible = false;
            }
        }
        if (visible)
        {
            // If the player is within range, visible, and moving fast enough - chase
            if (Vector3.Distance(target.position, transform.position) < 7f &&
                    ((targetRb.velocity.x > 3f || targetRb.velocity.x < -3f) || (targetRb.velocity.z > 3f || targetRb.velocity.z < -3f)))
            {
                triggered = true;
            }
        }
        // Enemy will only stop chasing the player if they get out of range
        if (Vector3.Distance(target.position, transform.position) >= 10f)
        {
            triggered = false;
        }

        if (triggered && visible)
        {
            agent.destination = target.position;
        }
    }

    //Damages player on touch
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player"))
        {
            Attack();
        }
    }

    IEnumerator StopEnemy()
    {
        stopped = true;
        target = null;
        yield return new WaitForSeconds(3);
        target = GameObject.FindGameObjectWithTag("Player").transform;
        stopped = false;
        triggered = false;
    }

    void Stop()
    {
        stopped = true;
    }

    // Controls Attack functions of Motion Sensor Enemy
    private void Attack()
    {
        CharacterHealth targetHealth = target.GetComponent<CharacterHealth>();
        targetHealth.healthSlider.value--;
    }
}
