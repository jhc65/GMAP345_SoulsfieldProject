using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour {

    public List<GameObject> enemyObjPool;
    public List<EnemyController> enemyScriptPool;
    public List<GameObject> inactivePool;

    // Areas for spawning
    public List<Zone> Zones;

    //move to resources
    [SerializeField]
    private GameObject enemyPrefab;

    [Tooltip("Seperate spawn points by zones. E.g. size of 3 with values of 4 will make first 4 child spawn points zone1, next 4 child spawn points zone2 ..etc")]
    public int[] ZoningSpawnPoints;

    [HideInInspector]
    public int CurrentZoneActive = 0;

    // Setup zones as dictated by the ZoningSpawnPoints public object
    void InitZones() {
        Debug.Assert(ZoningSpawnPoints.Length > 0);

        int endingChild = 0; // For keeping track of child spawn points 
        for (int numZone = 0; numZone < ZoningSpawnPoints.Length; numZone++) {
            Zone z = new Zone();
            Zones[numZone] = z;

            for (int j = endingChild; j < endingChild + ZoningSpawnPoints[numZone]; j++) {
                Zones[numZone].AddSpawnPoint(transform.GetChild(j));
                endingChild++;
            }

        }
    }

    // Initialize spawn points and zones. 
    void Start() {
        InitZones();

    }

    // Spawn an enemy in the current zone
    void SpawnInCurrentZone() {
        int numSpawnPoints = Zones[CurrentZoneActive].SpawnPointPositions.Count;

        // Spawn an enemy
        for (int i = 0; i < numSpawnPoints; i++)
        {
            enemyObjPool.Add(Instantiate(enemyPrefab, Zones[CurrentZoneActive].SpawnPointPositions[i].transform.position, Quaternion.identity));
            enemyScriptPool.Add(enemyObjPool[i].GetComponent<EnemyController>());

            // Log info on each enemy
            enemyScriptPool[i].spawnPos = Zones[CurrentZoneActive].SpawnPointPositions[i].transform.position;
            enemyScriptPool[i].aiManager = GetComponent<AIManager>();
        }
    }


    //we should change this to a custom event
    void Update()
    {
        /*
        if (inactivePool.Count == numSpawnPoints)
        {
            Debug.Log("inative pool");
            for (int i = 0; i < numSpawnPoints; i ++)
            {
                //set AI back to starting Position
                enemyScriptPool[i].transform.position = enemyScriptPool[i].spawnPos;
                //set AI back to active
                enemyObjPool[i].SetActive(true);
                // Clear Inactive pool
                
            }
            inactivePool = new List<GameObject>();


        }
        */

    }
}
