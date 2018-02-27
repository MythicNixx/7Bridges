using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayerY : MonoBehaviour
{
	public Transform target;
	private float smoothing = 25f;
	Vector3 offset;

	// Use this for initialization
	void Start()
    {
        offset = transform.position - target.position;
    }
	
	// Update is called once per frame
	void LateUpdate()
    {
        Vector3 targetCamPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }
}

