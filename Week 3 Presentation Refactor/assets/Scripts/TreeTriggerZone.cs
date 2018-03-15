using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class is used to let the parent tree manager know when the player is in the trigger zone.
*/
public class TreeTriggerZone : MonoBehaviour {

    private TreeManager parent;

    void Start()
    {
        parent = GetComponentInParent<TreeManager>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            parent.OnChildTriggerEnter();
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            parent.OnChildTriggerExit();
        }
    }
}
