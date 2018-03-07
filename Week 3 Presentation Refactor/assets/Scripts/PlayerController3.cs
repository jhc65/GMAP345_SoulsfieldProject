﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using System;
public class PlayerController3 : MonoBehaviour {
    private Rigidbody pcRigidbody;
    private GameObject pcCamera;

    private GameObject currentGround;
    private int health = 6;
    private Animator anim;
    private Collider sword;
    

    [SerializeField]
    private float walkableAngle = 60f;

    [SerializeField]
    private float groundSpeed = 7f; //7f is a default but overwriteable value
    [SerializeField]
    private float jumpForce = 7f;
    [SerializeField]
    private float airMaxSpeed = 6f;
    [SerializeField]
    private float airMaxAccel = .1f;
    [SerializeField]
    private float sprintSpeed = 1.5f; //multipilier of original character speed
    [SerializeField]
    private int maxSprintTime = 3; //max amount of time in seconds the player can sprint
    private float timer = 0.0f;     // value used in timer count do not modify
    private int sprintTimer = 0;   // value used in timer count do not modify

    // MaxTimer value and slowdown timer used to slow the player
    private float maxSlowdownTimer = 5f;
    private float slowdownTimer = 0.0f;

    private float slowMultiplier = .75f; // multiplier to slow down speed
    private float baseGroundSpeed; // base player speed set at start in order to revert ground speed after slowed
 

    private Vector3 currentFacing;
    private bool isAttacking = false;
    // Use this for initialization
    void Start () {
        baseGroundSpeed = groundSpeed;
		pcRigidbody = GetComponent<Rigidbody>();
        pcCamera = Camera.main.gameObject;
        anim = GetComponent<Animator>();
        sword = GameObject.FindGameObjectWithTag("Sword").GetComponent<Collider>();
    }

	// Update is called once per frame
	void LateUpdate () {
        if (Input.GetMouseButton(0))
        {
            anim.SetBool("attack", true);
            sword.enabled = true;
        }
        else
        {
            anim.SetBool("attack", false);
            sword.enabled = false;
        }

        Vector3 facing = pcCamera.transform.forward;
        Vector3 rightfacing = pcCamera.transform.right;

        facing.y = 0;
        rightfacing.y = 0;

        if (Input.GetKey(KeyCode.W))
        {
            currentFacing += facing;
        }

        if (Input.GetKey(KeyCode.S))
        {
            currentFacing -= facing;
        }

        if (Input.GetKey(KeyCode.D))
        {
            currentFacing += rightfacing;
        }

        if (Input.GetKey(KeyCode.A))
        {
            currentFacing -= rightfacing;
        }

        //if the player is slowed, decrease the timer
        if(slowdownTimer > 0)
        {
            slowdownTimer -= Time.deltaTime;
        }
        //once timer goes off, revert the groundSpeed and anim speed
        else
        {
            groundSpeed = baseGroundSpeed;
            anim.speed = 1;
        }

        //ground functions
        if (currentGround != null && !anim.GetBool("attack"))
        {

            if ((Input.GetKey(KeyCode.W)) || (Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.S)) || (Input.GetKey(KeyCode.D)))
            {
                pcRigidbody.velocity = transform.forward * groundSpeed + new Vector3(0, pcRigidbody.velocity.y, 0);
            }
            if(Input.GetKey(KeyCode.LeftShift) && sprintTimer < maxSprintTime) 
            {   
                pcRigidbody.velocity = transform.forward * sprintSpeed * groundSpeed + new Vector3(0, pcRigidbody.velocity.y, 0); 
                timer += Time.deltaTime; 
                sprintTimer = Convert.ToInt32( timer % 60);   
            } 
            if(!Input.GetKey(KeyCode.LeftShift) && sprintTimer > 0) 
            {   
                timer -= Time.deltaTime; 
                sprintTimer = Convert.ToInt32( timer % 60);   
            }
            RotateDo();
        }

        else //if not grounded
        {

            if ((Input.GetKey(KeyCode.W)) || (Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.S)) || (Input.GetKey(KeyCode.D)))
            {
                pcRigidbody.velocity = AirVelocityAccelerate(pcRigidbody, airMaxAccel, airMaxSpeed);
            }

            RotateDo();
        }

        if (IsGrounded())
        {
            anim.SetBool("IsJumping", false);

            if (pcRigidbody.velocity.magnitude > .1f) {
                anim.SetBool("IsWalking", true);
            } else {
                anim.SetBool("IsWalking", false);

            }
        }
        else
        {
            
            anim.SetBool("IsWalking", false);
            anim.SetBool("IsJumping", true);

        }

    }

    private bool IsGrounded()
    {                                                       //our raycast will touch ground regardless of scale
        return Physics.Raycast(transform.position, Vector3.down, transform.localScale.y+.1f);
    }


    //Collision Detection

    private void OnCollisionStay(Collision hit)
    {
        ContactPoint contact = hit.contacts[0];
        //log difference betwen up and ground nourmal
        float hitAngle = Vector3.Angle(Vector3.up, contact.normal);
        //if contact point is at the bottom fo our collider and the angle is walkable
        if (contact.point.y < transform.position.y + .5f && hitAngle < walkableAngle)
        {
 
            //log a reference for the ground hit
            currentGround = hit.gameObject;
        }

        if( hit.gameObject.tag == "Bouncy")
        {
            pcRigidbody.velocity = new Vector3(pcRigidbody.velocity.x, jumpForce, pcRigidbody.velocity.z);
        }
    }

    private void OnCollisionEnter(Collision hit)
    {
        if (hit.gameObject.tag == "Enemy")
        {
            health--;
            hit.gameObject.GetComponent<EnemyController>().Die(false); // Remove ghost, but player didn't kill it

            // Play ghost hit effect on desired body parts wtih this script
            foreach (SwapMaterialEffect effect in GetComponentsInChildren<SwapMaterialEffect>())
                effect.enabled = true;

            //if the player is not slowed, set the slowdown timer and change ground speed.
            if (slowdownTimer <= 0)
            {
                slowdownTimer = maxSlowdownTimer;
                groundSpeed = groundSpeed * slowMultiplier;
            }
            
            //if the animation speed is normal, slow it down.
            if(anim.speed == 1)
                anim.speed = anim.speed * slowMultiplier;
        }

        if(health <= 0)
        {
            SceneManager.LoadScene("Scenes/GameOver");

            gameObject.SetActive(false);

        }
    }

    private void OnCollisionExit(Collision hit)
    {
        if (hit.collider.gameObject == currentGround)
        {
            currentGround = null;
        }
    }


    private Vector3 AirVelocityAccelerate(Rigidbody acceleratingBody, float maxAccel, float maxSpeed)
    {

        Vector3 calcVelocity;
        Vector3 dir = acceleratingBody.velocity;
        //cancle out Y velocity
        dir.y = 0;
        float Mag = dir.magnitude;
        dir = Vector3.Normalize(dir);

        //base next tick of acceleration on current momentum
        float currentAccel = maxAccel * (Mag / (maxSpeed + .11f));
        Mag += currentAccel;
        calcVelocity = transform.forward * Mathf.Min(Mag, maxSpeed) + new Vector3(0, acceleratingBody.velocity.y, 0);

        return calcVelocity;
    }

    //Rotate PC
    private void RotateDo ()
    {
        if (currentFacing != Vector3.zero)
        {
            pcRigidbody.MoveRotation(Quaternion.RotateTowards
                (transform.rotation, Quaternion.LookRotation(currentFacing.normalized), 1000f * Time.deltaTime));
            currentFacing = Vector3.zero;
        }
    }


}