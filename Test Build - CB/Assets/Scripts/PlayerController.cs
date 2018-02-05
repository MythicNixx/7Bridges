/*Mitchell Hewitt & Brian A
 * CSG 114 - Final Project
 * 12/15/2017*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float speed = 4;
    private Vector3 moveInput;
    private Vector3 moveVel;

    void FixedUpdate()
    {
        Move();
    }
    void Move()
    {
        moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        moveVel = moveInput * speed;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100))
        {
            transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));
        }
        GetComponent<Rigidbody>().velocity = moveVel;
    }  
}