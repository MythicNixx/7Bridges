// Chris Blanchard
// Version 3
// Tuesday 2/27/2018
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
    public int damage = 1;
    public float atkRange = 1.5f;       // Maximum range that the enemy will attack the player at
    public float visibilityRange = 10f; // Maximum range that the enemy can detect the player

    private bool visible = false;       // Determines if player is visible or not
    private bool triggered = false;     // Player has triggered the enemy
    private float atkTimer = 0;         // Timer for attacks
    private float atkCooldown = 3f;     // The delay time between each attack
    private float distToPlayer;         // The calculated distance from the enemy to the player

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        distToPlayer = Vector3.Distance(target.position, transform.position);   // Gets distance from this object to the player object
        atkTimer += Time.deltaTime;
        Rigidbody targetRb = target.GetComponent<Rigidbody>();                  // Used to detect player movement

        // Used to check if the player can be seen
        Collider[] viewHits = Physics.OverlapSphere(transform.position, visibilityRange);
        for(int i = 0; i < viewHits.Length; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, target.position - transform.position, out hit, visibilityRange))
            {
                if (hit.collider.tag == "Player")
                {
                    visible = true;
                }

                else
                    visible = false;
            }
        }

        // Used to check if the enemy can hit the player and attacks
        Collider[] atkSphereHits = Physics.OverlapSphere(transform.position, atkRange);
        
        for (int i = 0; i < atkSphereHits.Length; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, target.position - transform.position, out hit, atkRange))
            {
                if (hit.collider.tag == "Player" && atkTimer >= atkCooldown)
                {
                    Attack();
                    atkTimer = 0;
                }
            }
        }

        // If the player is visible, within range, and moving fast enough - trigger motion sensor
        if (visible)
        {
            if (distToPlayer < visibilityRange &&
                    ((targetRb.velocity.x > 3f || targetRb.velocity.x < -3f) || (targetRb.velocity.z > 3f || targetRb.velocity.z < -3f)))
            {
                triggered = true;
            }
        }  

        // Enemy will only stop chasing the player if they get out of range or within attack range
        if(distToPlayer >= visibilityRange || distToPlayer < atkRange)
        {
            triggered = false;
            // If the enemy is within attack range, stop movement
            if(distToPlayer < atkRange)
            {
                agent.destination = transform.position;
            }
        }

        // If the player can be seen, and is moving faster than the detection speed, chase
        if (triggered && visible)
        {
            agent.destination = target.position;
        }
    }

    // Controls Attack functions of Motion Sensor Enemy
    private void Attack()
    {
        CharacterHealth targetHealth = target.GetComponent<CharacterHealth>();
        targetHealth.healthSlider.value -= damage;
    }
}
