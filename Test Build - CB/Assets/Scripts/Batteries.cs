// Chris Blanchard
// Sunday 02/04/2018
// Controls battery display and function
// Placed on the Player Character

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Batteries : MonoBehaviour
{
    public int batteries = 1;
    public Text batteryText;
	
	void Update ()
    {
        batteryText.text = "Batteries: " + batteries;
	}
}
