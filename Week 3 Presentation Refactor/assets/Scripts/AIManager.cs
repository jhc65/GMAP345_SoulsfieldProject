using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour {

    public List<GameObject> enemyObjPool;
    public List<EnemyController> enemyScriptPool;
    public List<GameObject> inactivePool;

    // Areas for spawning
    public List<Zone> Zones;

    // Wave data for spawning
    public List<Wave> Waves;

    //move to resources
    [SerializeField]
    private GameObject enemyPrefab;

    [Header("MUST SET IN INSPECTOR!")]
    [Tooltip("Seperate spawn points by zones. E.g. size of 3 with values of 4 will make first 4 child spawn points zone1, next 4 child spawn points zone2 ..etc")]
    public int[] ZoningSpawnPoints;

    [HideInInspector]
    public int CurrentZoneActive = 0;

    // For spawning
    private bool readyToSpawn = false;
    private int currentWave = 0;
    private float timeSinceLastSpawn = 0f;
    private int currentEnemy = 0;

    // Setup zones as dictated by the ZoningSpawnPoints public object
    void InitZones() {
        Debug.Assert(ZoningSpawnPoints.Length > 0);
        Zones = new List<Zone>();

        int endingChild = 0; // For keeping track of child spawn points 
        for (int numZone = 0; numZone < ZoningSpawnPoints.Length; numZone++) {
            Zones.Add(new Zone());
            for (int j = endingChild; j < endingChild + ZoningSpawnPoints[numZone]; j++) {
                Zones[numZone].AddSpawnPoint(transform.GetChild(j));
             
            }
            endingChild += ZoningSpawnPoints[numZone];
        }
    }

    // Read file to get wave data of parameters for each wave
    void InitWaves() {
        Waves = ReadSpawnData.GetWavesFromFile();
    }

    // Initialize spawn points and zones and enemies. 
    void Start() {
        InitZones();
        InitWaves();
        InitEnemies();
    }

    // Instantiate all enemies and set to inactive
    void InitEnemies() {
        int numWaves = Waves.Count;
       // for (int i = 0; i < numWaves; i++) {
        InsantiateWaveNum(0);
        //}

        readyToSpawn = true;
    }


    // Instantiate game object of enemies at correct positions and set them inactive
    void InsantiateWaveNum(int index) {
        Wave w = Waves[index];
        int numSpawned = 0;

        // Loop over all enemies to spawn
        while (numSpawned <= w.TotalToSpawn) {

            // ---Create enemy-----
            float ms = Random.Range(w.SlowestMS, w.FastestMS);
            int souls = w.NumberOfSouls;

            // Pick random spawn point
            int randomPoint = Random.Range(0, Zones[CurrentZoneActive].SpawnPointPositions.Count);
            Transform spawnPos = Zones[CurrentZoneActive].SpawnPointPositions[randomPoint];

            // Spawn enemy and set its ms and other data
            enemyObjPool.Add(Instantiate(enemyPrefab, spawnPos.position, Quaternion.identity));
            enemyScriptPool.Add(enemyObjPool[numSpawned].GetComponent<EnemyController>());
            enemyScriptPool[numSpawned].MovementSpeed = ms;
            enemyScriptPool[numSpawned].numSouls = souls;
            enemyScriptPool[numSpawned].spawnPos = spawnPos.position;
            enemyScriptPool[numSpawned].aiManager = GetComponent<AIManager>();
            enemyObjPool[numSpawned].SetActive(false); // set inactive
            numSpawned++;
        }
    }

    // Spawn enemies one by one
    void Update() {
        if (!readyToSpawn)
            return;
        
        Wave w = Waves[currentWave];
        if (timeSinceLastSpawn < w.SpawnFrequency && currentEnemy > 0) {
            timeSinceLastSpawn += Time.deltaTime;
            return;
        } else { // Time to spawn if chance is met
            float randomChance = Random.Range(0, 1);
            if (randomChance <= w.ChanceOfSpawn) {
                enemyObjPool[currentEnemy].SetActive(true);
                currentEnemy++;
                if (currentEnemy > w.TotalToSpawn) {
                    currentEnemy = 0;
                    readyToSpawn = false;
                }
            }

            timeSinceLastSpawn = 0f;
        }

        // TODO: Increment currentWave when all enemies have been killed
        // TODO/note: Possible logic error. EnemyObjPool currentEnemy index may not work for wave 2+. 
    }

}
