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

    private int HighestZone = 1;

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

    // Increase highest zone 
    public void ActivateNewZone() {
        HighestZone++;
    }

    // Callback for when the last enemy is killed.
    // Start spawning next wave
    public void OnLastEnemyKilled() {
        readyToSpawn = false;
        currentWave++;
        InitEnemies();
        StartCoroutine(TimeBetweenWaves(5));
    }

    // After x seconds, start spawning again
    IEnumerator TimeBetweenWaves(float time) {
        yield return new WaitForSeconds(time);
        readyToSpawn = true;
    }

    // Read file to get wave data of parameters for each wave
    void InitWaves() {
        Waves = ReadSpawnData.GetWavesFromFile();
    }

    // Initialize spawn points and zones and enemies. 
    void Start() {
        
        // Set up zones
        InitZones();
        // Read from file data about the waves to spawn
        InitWaves();

        // Create enemies with currentWave = 0
        InitEnemies();

        // Start spawning
        StartCoroutine(TimeBetweenWaves(0));
    }

    // Instantiate all enemies and set to inactive
    void InitEnemies() {
        if (currentWave >= Waves.Count) {
            currentWave = 0;
        }
        InsantiateWaveNum(currentWave);
    }


    // Instantiate game object of enemies at correct positions and set them inactive
    void InsantiateWaveNum(int index) {
        Wave w = Waves[index];
        int numSpawned = 0;
        int numWithSouls = 0;
        enemyObjPool.Clear();
        enemyScriptPool.Clear();
        currentEnemy = 0;

        // Loop over all enemies to spawn
        while (numSpawned < w.TotalToSpawn) {

            // ---Create enemy-----
            float ms = Random.Range(w.SlowestMS, w.FastestMS);

            // Pick random spawn point from a random zone (that have been unlocked)
            int randomZone = Random.Range(0, HighestZone);
            int randomPoint = Random.Range(0, Zones[randomZone].SpawnPointPositions.Count);

            Transform spawnPos = Zones[randomZone].SpawnPointPositions[randomPoint];

            // Instantiate enemy obj and set its ms and other data
            enemyObjPool.Add(Instantiate(enemyPrefab, spawnPos.position, Quaternion.identity));
            enemyScriptPool.Add(enemyObjPool[numSpawned].GetComponent<EnemyController>());
            enemyScriptPool[numSpawned].MovementSpeed = ms;
            enemyScriptPool[numSpawned].numSouls = 0; //initiate all numSouls to zero
            enemyScriptPool[numSpawned].spawnPos = spawnPos.position;
            enemyScriptPool[numSpawned].aiManager = GetComponent<AIManager>();
            enemyObjPool[numSpawned].SetActive(false); // set inactive
            numSpawned++;
        }

        // Set callback for last enemy
        enemyScriptPool[numSpawned - 1].isLast = true; 

        // Set soul
        Dictionary<int, int> enemiesWithSouls = new Dictionary<int, int>();
        while(numWithSouls < w.NumEnemiesHaveSouls)
        {
            // pick a random enemy to assign a soul
            int loc = Random.Range(0, enemyScriptPool.Count);

            // keep getting a valid location that hasn't been picked yet
            if (!enemiesWithSouls.ContainsKey(loc))
            {
                enemyScriptPool[loc].numSouls = w.NumberOfSouls;
                enemiesWithSouls.Add(loc, 1);
                numWithSouls++;
            }
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

                // Last enemy was created
                if (currentEnemy >= w.TotalToSpawn) {

                    // Reset
                    readyToSpawn = false;
                    return;
                }
            }
            timeSinceLastSpawn = 0f;
        }
    }

}
