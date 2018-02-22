using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulsSphere : MonoBehaviour
{
    private GameObject player;
    private Vector3 currentTarget;
    public int numSouls;
    private float MovementSpeed;
    private float distance;

    public SoulsSphere(int souls)
    {
        numSouls = souls;
    }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        MovementSpeed = 10;
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(player.transform.position, transform.position);
        currentTarget = player.transform.position;
        if (distance <= 15f)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentTarget, MovementSpeed * Time.deltaTime);
        }
    }

    public int getSouls()
    {
        return numSouls;
    }

}
