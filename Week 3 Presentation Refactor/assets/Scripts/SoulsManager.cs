using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulsManager : MonoBehaviour {

    public int numSouls;

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "soulsSphere")
        {
            numSouls += collision.gameObject.GetComponent<SoulsSphere>().getSouls();
            Destroy(collision.gameObject);
        }
    }

    public int getSouls()
    {
        return numSouls;
    }

}
