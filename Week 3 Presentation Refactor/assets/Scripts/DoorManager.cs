﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour {

    public int soulsRequired;
    public AIManager a;

    private Vector3 pivot;
    private GameObject player;
    private SoulsManager soulsManager;
    public float distance = 5f;
    public float movementAngle;

	void Start () {
        pivot = transform.GetChild(0).position;
        player = GameObject.FindGameObjectWithTag("Player");
        soulsManager = player.GetComponent<SoulsManager>();
    }
	
	void Update () {
        float dist = Vector3.Distance(player.transform.position, transform.position);
      //  Debug.Log(dist);
        if (dist < distance)
        {
            if(Input.GetKeyDown(KeyCode.E) && soulsManager.getSouls() >= soulsRequired)
            {
                transform.RotateAround(pivot, Vector3.up, movementAngle);
                soulsManager.numSouls -= soulsRequired;
                a.ActivateNewZone();
                enabled = false;
            }
        }
	}
}
