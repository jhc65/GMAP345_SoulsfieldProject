using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour {

    [Header("MUST SET IN INSPECTOR!")]
    public int LeadsToZone;
    public int soulsRequired;
    public AIManager a;

    private GameObject player;
    //fade out per second
    private float fadePerSecond = .01f;
    private SoulsManager soulsManager;
    private Collider treeColider;
    //material of the tree
    private Material material;
    //used to indicate when the tree needs to fade out.
    private bool fadeOut;
    //used to indicate when the player is close to the tree
    private bool close;
    public float distance = 5f;

	void Start () {
        treeColider = GetComponent<Collider>();
        player = GameObject.FindGameObjectWithTag("Player");
        soulsManager = player.GetComponent<SoulsManager>();
        material = GetComponent<Renderer>().material;
        fadeOut = false;
        close = false;
    }
	
	void Update () {
        if (close && !fadeOut)
        {
            if(Input.GetKeyDown(KeyCode.E) && soulsManager.getSouls() >= soulsRequired)
            {
                soulsManager.numSouls -= soulsRequired;
                fadeOut = true;
            }
        }
        //when fading out
        else if(fadeOut)
        {
            //update the alpha of the material's color
            material.color = new Color(material.color.r, material.color.g, material.color.b, material.color.a - fadePerSecond);
            //disable the tree colider
            treeColider.enabled = false;
        }

        //once the alpha is less than zero, activate the new zone and disable the script.
        if(material.color.a <= 0)
        {
            if (LeadsToZone <= 0)
            {
                Debug.LogError("Tree manager lead to zone must be set in inspector!!!!!!!!!!!");
                UnityEditor.EditorApplication.isPlaying = false;
            }
            a.ActivateNewZone(LeadsToZone);
            
            enabled = false;
        }
	}

    //The child is a trigger zone which calls these methods
    //in order to tell when the player is close to the tree

    public void OnChildTriggerEnter()
    {
        close = true;
    }

    public void OnChildTriggerExit()
    {
        close = false;
    }

}
