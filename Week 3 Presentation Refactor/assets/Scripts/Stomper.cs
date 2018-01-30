using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stomper : MonoBehaviour {
    public MushController mush;
    public Animator anim;
    public GameObject topPoint;

    void Awake ()
    {
        mush = transform.parent.GetComponent<MushController>();
        anim = mush.GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision hit)
    {
        if (hit.transform.position.y > topPoint.transform.position.y - .2f) {
            mush.health--;
            if (mush.health > 0)
            {
                anim.SetTrigger("Squish");
            }
            else
            {
                anim.SetTrigger("Death");
                mush.health = 1;
            }
        }
        
    }
       

  


  


   
}
