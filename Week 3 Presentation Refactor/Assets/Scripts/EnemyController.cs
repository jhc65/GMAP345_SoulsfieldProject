using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    
    public GameObject deathSmoke;
    public GameObject ghostBlob;

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
    private bool isMovingTowardsPlayer = true;

    [SerializeField] private int maxGroupSize = 4; // if there are this many ghosts, move away
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
        
        if (isMovingTowardsPlayer) {
            currentTarget = player.transform.position;
            transform.LookAt(currentTarget);
            transform.position = Vector3.MoveTowards(transform.position, currentTarget, MovementSpeed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
            CheckAndMoveAway();
            OssilateGlow();
        }

    }

    public void Die(bool wasKilled = true) {
        if (deathSmoke)
            Instantiate(deathSmoke, transform.position, Quaternion.identity);

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
            rend.material.SetFloat("_MainTex", rp);

        }
    }

    // Probably the worst performant way to do this? 
    private int CountEnemiesNearby(float radius) {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        int i = 0;
        int enemies = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].gameObject.CompareTag("Enemy"))
                enemies++;
            i++;
        }

        return enemies;
    }

    // Too many enemies nearby. Move away to a random position
    private void CheckAndMoveAway() {
        if (CountEnemiesNearby(10) >= maxGroupSize) {
            MoveAway();
        }
    }

    // Pick a random point behind ghost and turn into a blob
    public void MoveAway(float min = 5f, float max = 15f) {
            Vector3 randomPos = new Vector3(transform.position.x + Random.Range(min, max), 2, transform.position.z + Random.Range(min, max));
            GameObject blob = Instantiate(ghostBlob, transform.position, Quaternion.identity);
            blob.GetComponent<BlobController>().GoalPosition = randomPos;
            blob.GetComponent<BlobController>().OriginalEnemy = GetComponent<EnemyController>();
            Deactivate();
    }

    // Move off screen and turn invisible and stop moving towards player
    private void Deactivate() {
        transform.position = new Vector3(Random.Range(0, 100), Random.Range(-100, -200), Random.Range(0, 100));
        foreach(MeshRenderer r in GetComponentsInChildren<MeshRenderer>()) {
            r.enabled = false;
        }
        isMovingTowardsPlayer = false;
    }

    // Move back to correct position and turn rendrer back on
    public void Reactiveate(Vector3 position) {
        transform.position = position;
        foreach (MeshRenderer r in GetComponentsInChildren<MeshRenderer>())
        {
            r.enabled = true;
        }
        isMovingTowardsPlayer = true;
    }


}
