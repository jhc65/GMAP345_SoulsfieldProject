using System.Collections.Generic;
using UnityEngine;

/*
 * 
 * Class that holds spawn points for each zone
 * 
 */
public class Zone {
    public List<Transform> SpawnPointPositions {get; set;}
    public int ID { get; set; }

    public Zone() {
        SpawnPointPositions = new List<Transform>();
    }
    public Zone(int i) {
        SpawnPointPositions = new List<Transform>();
        ID = i;
    }

    public void AddSpawnPoint(Transform t) {
        SpawnPointPositions.Add(t);
    }

}
