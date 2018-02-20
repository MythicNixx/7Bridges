//Mitchell Hewitt 
//2/20/2018

/*This is my script for the flashlight. It handles the toggle on/off of light. 
 * Stops the enemy's movement. Also pick up/drop objects. 
 * And will display to a canvas once one is complete. */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Flashlight : MonoBehaviour {
    //Raycast that handles the enemy
    RaycastHit lightRay;
    public float lightRayLen = 8f;
    //Raycast that handles the object pick up
    RaycastHit pickUpRay;
    public float pickUpRayLen = 1f;
    //Handles the battery pick up distance
    public float batteryRayLen = 0.6f;

    public GameObject motionEnemy;

    //These GameObjects represent the object in hand
    GameObject pickedUp;
    public GameObject pickUpSpot;
    public bool isHolding;
    public bool inLight;
    //These are the Texts that are displayed on the canvas
    //public Text batteries;
    //public Text batteryLife;
    public Text pickUpDis;

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
        //Declaring what the 'Hand' obj is
        GameObject pickUpSpot = GameObject.FindGameObjectWithTag("Hand").gameObject;
    }

    void Update()
    {
        //Once HUD is set up, uncomment and use these strings as the displays
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
        //This handles all the flashlight functions
        //This ray handle the object and battery pick up
        if (Physics.Raycast(flashlight.gameObject.transform.position, flashlight.gameObject.transform.forward, out pickUpRay, pickUpRayLen))
        {
            if (pickUpRay.collider.tag == "PickUp" && isHolding == false)
            {
                pickUpDis.gameObject.SetActive(true);
                pickUpDis.text = "Hold 'E' to pick up ".ToString();
            }
            else
            {
                pickUpDis.gameObject.SetActive(false);
                pickUpDis.text = " ".ToString();
            }
            if (pickUpRay.collider.tag == "PickUp" && Input.GetKey(KeyCode.E))
            {
                PickUpObj();
            }
            else
            {
                isHolding = false;
            }
            //Differentiates between battery and other objects 
            if (pickUpRay.collider.tag == "Battery" && Input.GetKey(KeyCode.E))
            {
                BatteryPickUp();
            }
            else
            {
                pickUpDis.gameObject.SetActive(false);
            }
            //Releases object when 'E' is released
            if (Input.GetKeyUp(KeyCode.E))
            {
                //Orphans the object
                pickUpSpot.transform.DetachChildren();
            }
            else if (isHolding == true)
            {
                pickUpDis.gameObject.SetActive(false);
            }
            else
            {
                pickUpDis.gameObject.SetActive(true);
            }
        }
        if (Physics.Raycast(flashlight.gameObject.transform.position, flashlight.gameObject.transform.forward, out lightRay, lightRayLen))
        {
            if(lightRay.collider.tag == "Mo-Sensor")
            {
                lightRay.collider.gameObject.SendMessage("Stop");
            }
        }
    }
    void BatteryPickUp()
    {
        //Increments batteryCount and disables the battery object
        Debug.DrawRay(flashlight.gameObject.transform.position, flashlight.gameObject.transform.forward * pickUpRayLen, Color.green, 0.5f);
        pickUpDis.gameObject.SetActive(true);
        pickUpDis.text = "Hold 'E' to pick up ".ToString();
        batteryCount++;
        pickUpRay.collider.gameObject.SetActive(false);
    }
    void PickUpObj()
    {
        //Sets the empty 'hand' object and the collider object
        pickedUp = pickUpRay.collider.gameObject;
        Debug.DrawRay(flashlight.gameObject.transform.position, flashlight.gameObject.transform.forward * pickUpRayLen, Color.blue, 0.5f);
        //Sets the position, rotation, and parent of the picked up object
        pickUpRay.collider.gameObject.transform.parent = pickUpSpot.gameObject.transform;
        pickUpRay.collider.gameObject.transform.position = pickUpSpot.gameObject.transform.position;
        pickUpRay.collider.gameObject.transform.rotation = pickUpSpot.gameObject.transform.rotation;
        isHolding = true;
    }
}
