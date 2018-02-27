// Chris Blanchard
// Thursday 02/22/2018
// Spawn Manager for enemy spawn points that controls how often and where enemies spawn
// Can also be used to spawn game objects like batteries

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawnManager : MonoBehaviour
{
    public float spawnDelay = 1f;               // Time delay between each spawn
    public float minSpawnDist = 10f;            // Minimum distance that the enemy can spawn to the player
    public string objectSpawnTag = "";          // Tag of the spawn locations of the object
    public string lockedSpawnPointTag = "";     // Tag of the object's locked spawnlocation
    public GameObject objectPrefab;

    private string itemTag = "";                 // Tag of the object being spawned
    private int itemDetected = 0;
    private GameObject[] spawnPoints;       // Array of all Spawn Point objects
    private GameObject[] lockedSpawnPoints;
    private GameObject prevSpawnPoint;
    private float elapsedTime = 0;

    public void Awake()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag(objectSpawnTag);    // Grabs Every Spawn Point tagged object and puts it in the array
        lockedSpawnPoints = GameObject.FindGameObjectsWithTag(lockedSpawnPointTag);
        itemTag = objectPrefab.tag;
    }

    // Update is called once per frame
    void Update ()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag(objectSpawnTag);

        elapsedTime += Time.deltaTime;

		if(elapsedTime >= spawnDelay)
        {
            if(spawnPoints.Length != 0)
            {
                Spawn();            // Spawns enemy within a range
            }
            
            elapsedTime = 0;
        }

        lockedSpawnPoints = GameObject.FindGameObjectsWithTag(lockedSpawnPointTag);

        // If there are locked spawn points
        if (lockedSpawnPoints.Length != 0)
        {
            //Run for every locked Spawn Point
            for (int i = 0; i < lockedSpawnPoints.Length; i++)
            {
                itemDetected = 0;

                // Check for an object at the spawn location
                Collider[] hits = Physics.OverlapSphere(lockedSpawnPoints[i].transform.position, 1f);

                for (int hitNum = 0; hitNum < hits.Length; hitNum++)
                {
                    // If an object is inside the spawn location with the proper tag, increment items detected
                    if (hits[hitNum].gameObject.tag == itemTag)
                    {
                        itemDetected++;
                    }
                }

                if (itemDetected == 0)
                {
                    lockedSpawnPoints[i].gameObject.tag = objectSpawnTag;
                }
            }
        }
    }
    // Spawns Objects at the closest location to the player outside of a minimum range
    private void Spawn()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        int index = 0;


        for(int i = 1; i < spawnPoints.Length; i++)
        {

            if ((Vector3.Distance(player.position, spawnPoints[i].transform.position) <
                    Vector3.Distance(player.position, spawnPoints[index].transform.position)) 
                    &&
                    Vector3.Distance(player.position, spawnPoints[i].transform.position) > minSpawnDist)
            {
                index = i;
            }
        }

        if (Vector3.Distance(player.position, spawnPoints[index].transform.position) > minSpawnDist)
        {
            Instantiate(objectPrefab, spawnPoints[index].transform.position, Quaternion.identity);
            spawnPoints[index].tag = lockedSpawnPointTag;
        }
    }
}
