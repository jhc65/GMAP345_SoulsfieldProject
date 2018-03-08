using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Look at player
public class GameOverCam : MonoBehaviour
{
    public Vector3 target;

    void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
        transform.LookAt(target);
	}
}
