using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCControllerTest : MonoBehaviour {
    private Rigidbody pcRigidbody;
    private GameObject pcCamera;

    private GameObject currentGround;

    private Animator anim;

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

    private Vector3 currentFacing;
    // Use this for initialization
    void Start () {
		pcRigidbody = GetComponent<Rigidbody>();
        pcCamera = Camera.main.gameObject;
        anim = GetComponentInChildren<Animator>();
	}

 
	
	// Update is called once per frame
	void LateUpdate () {
        if (Input.GetMouseButton(0))
        {
            anim.SetBool("attack", true);
        }
        else
        {
            anim.SetBool("attack", false);
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
        //ground functions
        if (currentGround != null)
        {
            if ((Input.GetKey(KeyCode.W)) || (Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.S)) || (Input.GetKey(KeyCode.D)))
            {
                pcRigidbody.velocity = transform.forward * groundSpeed + new Vector3(0, pcRigidbody.velocity.y, 0);
            }

            if ((Input.GetKey(KeyCode.Space)))
            {
                pcRigidbody.velocity = new Vector3(pcRigidbody.velocity.x, jumpForce, pcRigidbody.velocity.z);
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
        if(IsGrounded() && pcRigidbody.velocity.magnitude > 0)
        {
            anim.SetBool("IsWalking", true);
        }
        else
        {
            anim.SetBool("IsWalking", false);
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
        if (contact.point.y < transform.position.y - .48f && hitAngle < walkableAngle)
        {
            //log a reference for the ground hit
            currentGround = hit.gameObject;
        }

        if( hit.gameObject.tag == "Bouncy")
        {
            pcRigidbody.velocity = new Vector3(pcRigidbody.velocity.x, jumpForce, pcRigidbody.velocity.z);
        }
    }

    private void OnCollisionExit(Collision hit)
    {
        if (hit.collider.gameObject == currentGround)
        {
            currentGround = null;
        }
    }

    //
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
