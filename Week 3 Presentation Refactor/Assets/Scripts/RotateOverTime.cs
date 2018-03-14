﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOverTime : MonoBehaviour {

    public float speed = 5f;

	void Start () {
		
	}
	
	void Update () {
        transform.Rotate(Vector3.forward, speed * Time.deltaTime);
	}
}
