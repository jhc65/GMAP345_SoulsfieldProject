using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Look at player
public class GameOverCam : MonoBehaviour
{
    [SerializeField]
    private GameObject pcObj;

    void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
        transform.LookAt(pcObj.transform.position);
	}
}
