using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    private Animator anim;
    private int health = 1;

    [HideInInspector]
    public Vector3 spawnPos;
    public AIManager aiManager;

    private GameObject player;
    private Vector3 currentTarget;

    public float MovementSpeed; // speed of enemy towards player
    public int numSouls;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {

        currentTarget = player.transform.position;
        transform.LookAt(currentTarget);
        transform.position = Vector3.MoveTowards(transform.position, currentTarget, MovementSpeed * Time.deltaTime);

    }

    private void OnTriggerEnter(Collider hit)
    {
        if (hit.gameObject.tag == "Sword")
        {
            health--;
            if (health <= 0) {
                if (numSouls > 0)
                {
                    GameObject soulsSphere = Instantiate(Resources.Load("SoulsSphere"), transform.position, transform.rotation) as GameObject;
                    soulsSphere.GetComponent<SoulsSphere>().numSouls = this.numSouls;
                }
                gameObject.SetActive(false);
            }
        }
    }


    /*
    public void DisableMush()
    {
        aiManager.inactivePool.Add(gameObject);
        gameObject.SetActive(false);
    }
    */


}
