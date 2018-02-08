using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    private Animator anim;
    public int health = 3;

    [HideInInspector]
    public Vector3 spawnPos;

    public AIManager aiManager;

    private Vector3 currentTarget;


    void Awake()
    {
        currentTarget = spawnPos;
    }

    void Update()
    {
        if (health > 0)
        {

            //nothing special here just moving between
            //our pahting nodes
            if (Vector3.Distance(transform.position, currentTarget) <= 2)
            {
                //select new target
                currentTarget = aiManager.aiNodePool[Random.Range(0, aiManager.aiNodePool.Count)].transform.position;
                Debug.Log(name + " has changed target to " + currentTarget);
            }
            else
            {
                //path to current target
                transform.LookAt(currentTarget);//(new Vector3 (currentTarget.x, transform.position.y, currentTarget.z));
                transform.position = Vector3.Lerp(transform.position, currentTarget, .5f * Time.deltaTime);
                //Debug.Log("pathing");
            }
        }
    }


    public void DisableMush()
    {
        aiManager.inactivePool.Add(gameObject);

        gameObject.SetActive(false);
    }



}
