using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave {
    
    // Number to spawn
    public int TotalToSpawn { get; set; }

    // Movement range of enemy
    public float SlowestMS { get; set;  }
    public float FastestMS { get; set; }

    // Attempt to spawn with ChanceOfSpawn every SpawnFrequency seconds
    public float ChanceOfSpawn { get; set; }
    public float SpawnFrequency { get; set; }

    // How many have been spawned at this wave so far
    public int NumEnemiesSpawned { get; set; }

    public Wave(int n, float s, float fa, float c, float fr) {
        TotalToSpawn = n;
        SlowestMS = s;
        FastestMS = fa;
        ChanceOfSpawn = c;
        SpawnFrequency = fr;
    }
}
