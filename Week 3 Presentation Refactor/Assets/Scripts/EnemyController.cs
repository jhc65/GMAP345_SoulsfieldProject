﻿using System.Collections;
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
    public bool isLast = false;

    // For glowing effect
    // Gets all child renderers of the object (body parts)
    private Renderer[] rends;
    void Awake()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        rends = GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in rends) {
            if (!rend.gameObject.CompareTag("IgnoreGlow"))
                rend.material.shader = Shader.Find("Custom/GhostShader");
        }
    }

    void Update()
    {
        OssilateGlow();
        currentTarget = player.transform.position;
        transform.LookAt(currentTarget);
        transform.position = Vector3.MoveTowards(transform.position, currentTarget, MovementSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
    }

    public void Die() {
        anim.SetInteger("DeathValue", Random.Range(0,2)); // Play a random animation
        if (numSouls > 0) {
            GameObject soulsSphere = Instantiate(Resources.Load("SoulsSphere"), transform.position, transform.rotation) as GameObject;
            soulsSphere.GetComponent<SoulsSphere>().numSouls = this.numSouls;
        }

        if (isLast) {
            aiManager.OnLastEnemyKilled();
        }

        this.enabled = false;
    }

    private void OnTriggerEnter(Collider hit)
    {
        if (hit.gameObject.tag == "Sword")
        {
            health--;
            if (health <= 0) {
                Die();
            }
        }
    }

    private void OssilateGlow() {

        // With a speed, osssilate from 0.5 to 8 rim power
        float speed = 20f;
        float rp = Mathf.PingPong(Time.time * speed, 8.0f) + 0.5f;
        foreach (Renderer rend in rends) {
            rend.material.SetFloat("_RimPower", rp);
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
