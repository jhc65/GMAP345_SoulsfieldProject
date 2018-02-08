using System.Collections.Generic;
using UnityEngine;

/*
 * 
 * Class that holds spawn points for each zone
 * 
 */
public class Zone {
    public List<Transform> SpawnPointPositions {get; set;}

    public Zone() {
        SpawnPointPositions = new List<Transform>();
    }

    public void AddSpawnPoint(Transform t) {
        SpawnPointPositions.Add(t);
    }

}
