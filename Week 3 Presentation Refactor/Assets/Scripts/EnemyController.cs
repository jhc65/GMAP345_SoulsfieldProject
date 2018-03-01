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
    public bool isLast = false;
    private bool isDead = false;

    // For glowing effect
    // Gets all child renderers of the object (body parts)
    private Renderer[] rends;
    void Awake()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        rends = GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in rends) {
            if (!rend.gameObject.CompareTag("IgnoreGlow")) // Ignore glow on particle effects
                rend.material.shader = Shader.Find("Custom/GhostShader");
        }
    }

    void Update()
    {
        if (isDead)
            return;
        currentTarget = player.transform.position;
        transform.LookAt(currentTarget);
        transform.position = Vector3.MoveTowards(transform.position, currentTarget, MovementSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        OssilateGlow();
    }

    public void Die(bool wasKilled = true) {
        isDead = true; // stop moving towards player and glowing

        // Remove capsule collider while death animation plays
        GetComponent<CapsuleCollider>().enabled = false;
        anim.SetInteger("DeathValue", Random.Range(0,2)); // Play a random animation
        if (numSouls > 0 && wasKilled) {
            GameObject soulsSphere = Instantiate(Resources.Load("SoulsSphere"), transform.position, transform.rotation) as GameObject;
            soulsSphere.GetComponent<SoulsSphere>().numSouls = this.numSouls;
        }

        if (isLast) {
            aiManager.OnLastEnemyKilled();
        }
    }

    // When enemy is hit by sword
    private void OnTriggerEnter(Collider hit)
    {
        if (hit.gameObject.tag == "Sword")
        {
            health--;
            if (health <= 0) {
                GetComponent<PlayRandomSoundEffect>().PlayDeathSound();
                Die(true);
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
}
