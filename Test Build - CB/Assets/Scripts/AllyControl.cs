// Chris Blanchard
// Tuesday 1/23/18
// Controls the movement of Allies when not controlled by the user
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEditor.AI;

public class AllyControl : MonoBehaviour
{
    public Transform target;
    public NavMeshAgent agent;
    public float followDist = -0.2f;    // Min distance for the player to follow behind
    public GameObject screenCanvas;         // Canvas Reference for healthbar instantiation

    public GameObject healthBar1;
    public GameObject healthBar2;
    private Transform mainPlayer;       // Currently Controlled Player
    private Vector3 storedPos;
    private float speed = 4;
    private float smoothTime = 0.2f;    // Time taken to reach followChar
    private bool saved = false;         // Denotes whether the ally has been found/saved yet


    private void Awake()
    {
        screenCanvas = GameObject.Find("Canvas");
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        storedPos = new Vector3();
    }

    void Update ()
    {
        Rigidbody targetRb = target.GetComponent<Rigidbody>();
        

        // If not saved, rotate to look at the controlled player
        if (!saved)
        {
            var targetRotation = Quaternion.LookRotation(target.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
        }

        // If saved, rotate and follow the player or 1st Ally
        else
        {
            var targetRotation = Quaternion.LookRotation(target.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);

            var velocity = Vector3.zero;

            //Establishes position for Ally to follow behind player
            Vector3 followPosition = target.TransformPoint(new Vector3(0, 0, followDist));
            if(target.position != storedPos)
            {
                RaycastHit hit;
                Vector3 back = target.TransformDirection(Vector3.back);

                if (!Physics.Raycast(target.position, back, out hit, 5f))
                    agent.destination = followPosition;
            }
            storedPos = target.position;
        }
	}

    private void OnCollisionEnter(Collision other)
    {
        Transform obj = other.transform;
        if(obj.tag.Equals("Player") && !saved)
        {
            saved = true;

            // If first ally rescued, load up in UI as first Ally
            if (obj.GetComponent<PlayerController>().firstAlly)
            {
                // Sets Bool in PlayerController that the first ally has been found
                obj.GetComponent<PlayerController>().firstAlly = false;

                // Sets the allies Tag in game
                gameObject.tag = "Ally1";

                // Sets up the health bar under the main players
                Instantiate(healthBar1, Vector3.zero, Quaternion.identity, screenCanvas.transform);

                // Sets this ally to follow the main character
                target = GameObject.FindGameObjectWithTag("Player").transform;
            }

            // Else Second Ally Load up for UI
            else
            {
                gameObject.tag = "Ally2";
                Instantiate(healthBar2, Vector3.zero, Quaternion.identity, screenCanvas.transform);
                target = GameObject.FindGameObjectWithTag("Ally1").transform;
            }
        }
    }
}