﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*This is my script for the flashlight. It handles the toggle on/off of light. 
 * Stops the enemy's movement. Also pick up/drop objects. 
 * And will display to a canvas once one is complete. */

public class Flashlight : MonoBehaviour {
    //Raycast that handles the enemy.
    RaycastHit lightRay;
    public float lightRayLen = 8f;
    //Raycast that handles the object pick up.
    RaycastHit pickUpRay;
    public float pickUpRayLen = 1f;
    //Handles the battery pick up distance.
    public float batteryRayLen = 0.6f;

    GameObject pickedUp;
    public GameObject pickUpSpot;

    //public Text batteries;
    //public Text batteryLife;

    //Flashlight Object
    public Light flashlight;
    //Flashlight power variables
    public float power = 100.0f;
    private float maxPower = 100.0f;
    private float minPower = 0.0f;
    //Battery charge variable
    private float batteryCharge = 100.0f;

    //Number of batteries currently possesed by the player
    public int batteryCount = 3;
    //Power Drain controls how fast the battery life decreases
    float powerDrain = 1.0f;
    //Boolean that tells whether or not the flashlight is able to be used based off of current power
    private bool usable = true;
    //Battery game object
    public GameObject battery;

    private void Awake()
    {
        //Declaring what the 'Hand' obj is.
        GameObject pickUpSpot = GameObject.FindGameObjectWithTag("Hand").gameObject;
    }

    void Update()
    {
        //Once HUD is set up, uncomment and use these strings as the displays.
        /*
        batteryLife.text = batteryCharge.ToString();
        batteries.text = batteryCount.ToString();
        */
        //If the F key is pressed and the power is greater than zero, then the flashlight will toggle between on and off
        if (Input.GetKeyDown(KeyCode.F) && usable)
        {
            flashlight.enabled = !flashlight.enabled;
        }
        //While the flashlight is off, the power will drain
        if (flashlight.enabled)
        {
            power -= Time.deltaTime * powerDrain;
        }
        //This is to ensure that the power will never go over 100
        if (power > maxPower)
        {
            power = maxPower;
        }
        //This is to disable to flashlight and make sure it can't be used until the player uses a battery to recharge the flashlight
        if (power < minPower)
        {
            power = minPower;
            flashlight.enabled = false;
            usable = false;
        }
        //After you replace the batteries, it allows you to use the flashlight again
        if (power > minPower)
        {
            usable = true;
        }
        //This says that if the player has at least one battery, and if they press R, then the flashlight will be fully charged
        if (Input.GetKeyDown(KeyCode.B) && batteryCount > 0)
        {
            power += batteryCharge;
            batteryCount -= 1;
        }
        /**************************************************************************************************************************************************/
        //This handles all the flashlight functions.
        if (Physics.Raycast(flashlight.gameObject.transform.position, flashlight.gameObject.transform.forward, out lightRay, lightRayLen) && flashlight.enabled)
        {
            //Displays the ray while in editor mode.
            Debug.DrawRay(flashlight.gameObject.transform.position, flashlight.gameObject.transform.forward * lightRayLen, Color.red, 0.5f);
            if (lightRay.collider.tag == "Enemy")
            {
                //Disables the enemy for testing.
                lightRay.collider.gameObject.SetActive(false);
                /*
                lightRay.collider.gameObject.GetComponent<NavMeshAgent>().speed = 0;
                lightRay.collider.gameObject.GetComponent<NavMeshAgent>().SetDestination(null);
                */
            }
        }
        //This ray handle the object and battery pick up.
        if (Physics.Raycast(flashlight.gameObject.transform.position, flashlight.gameObject.transform.forward, out pickUpRay, pickUpRayLen))
        {
            //Displays the ray while in editor mode.
            Debug.DrawRay(flashlight.gameObject.transform.position, flashlight.gameObject.transform.forward * pickUpRayLen, Color.blue, 0.5f);
            if (pickUpRay.collider.tag == "PickUp" && Input.GetKey(KeyCode.E))
            {
                //Sets the empty 'hand' object and the collider object.
                pickedUp = pickUpRay.collider.gameObject;
                //Sets the position, rotation, and parent of the picked up object.
                pickUpRay.collider.gameObject.transform.parent = pickUpSpot.gameObject.transform;
                pickUpRay.collider.gameObject.transform.position = pickUpSpot.gameObject.transform.position;
                pickUpRay.collider.gameObject.transform.rotation = pickUpSpot.gameObject.transform.rotation;
            }
            //Differentiates between battery and other objects 
            if (pickUpRay.collider.tag == "Battery" && Input.GetKey(KeyCode.E))
            {
                //Increments batteryCount and disables the battery object.
                Debug.DrawRay(flashlight.gameObject.transform.position, flashlight.gameObject.transform.forward * pickUpRayLen, Color.green, 0.5f);
                batteryCount++;
                pickUpRay.collider.gameObject.SetActive(false);
            }
        }
        //Releases object when 'E' is released.
        else if (Input.GetKeyUp(KeyCode.E))
        {
            //Orphans the object.
            pickUpSpot.transform.DetachChildren();
        }
    }
}
