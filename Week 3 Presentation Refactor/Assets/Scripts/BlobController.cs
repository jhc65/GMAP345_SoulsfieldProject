using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobController : MonoBehaviour {

    [HideInInspector] public Vector3 GoalPosition;
    [HideInInspector] public EnemyController OriginalEnemy;

    [SerializeField] private float movementSpeed = 20f;

    void Start () {
	}
	
    void Update () {
        if (OriginalEnemy == null)
            return;

        // Otherwise, move towards goal
        transform.position = Vector3.MoveTowards(transform.position, GoalPosition, movementSpeed * Time.deltaTime);

        // Reached goal
        if (Vector3.Distance(transform.position, GoalPosition) <= 1f) {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            OriginalEnemy.Reactiveate(transform.position);
            Destroy(this.gameObject);
        }

	}
}
