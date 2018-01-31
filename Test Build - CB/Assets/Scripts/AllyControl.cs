// Chris Blanchard
// Tuesday 1/23/18
// Controls the movement of Allies when not controlled by the user
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyControl : MonoBehaviour
{
    public Transform followChar; // the character that the ally will follow
    public float followDist = -2.5f; // Min distance for the player to follow behind

    private float smoothTime = 0.2f; // Time taken to reach followChar
	
	void Update ()
    {
        transform.LookAt(followChar);

        var velocity = Vector3.zero;

        //Establishes position for Ally to follow behind player
        Vector3 targetPosition = followChar.TransformPoint(new Vector3(0, 0, followDist));

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
	}
}