using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This script handles the function of the compass and aligns the needs in update.

public class CompassScript : MonoBehaviour {

    public Vector3 northDir;
    public Transform player;
    public Quaternion MissionDir;

    public RectTransform NorthNeedle;
    public RectTransform MissionNeedle;

    public Transform missionLoc;
	
	// Updates both of the needles each frame.
	void Update ()
    {
        ChangeMissionDir();
        ChangeNorthDir();
	}

    //This updates the 'North' direction.
    public void ChangeNorthDir()
    {
        northDir.z = player.eulerAngles.y;
        NorthNeedle.localEulerAngles = northDir;
    }

    //This directs the mission location on the compass.
    public void ChangeMissionDir()
    {
        Vector3 dir = transform.position - missionLoc.position;

        MissionDir = Quaternion.LookRotation(dir);

        MissionDir.z = -MissionDir.y;
        MissionDir.x = 0;
        MissionDir.y = 0;

        MissionNeedle.localRotation = MissionDir * Quaternion.Euler(northDir);
    }
}
