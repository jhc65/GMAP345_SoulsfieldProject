using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/*
 * 
 * 
 * Static class for reading from a file
 * 
 */

public static class ReadSpawnData {

    // Read file line by line and return list of waves
    public static List<Wave> GetWavesFromFile() {
        List<Wave> waves = new List<Wave>();

        string path = "SpawnData";

        //Read the text from directly from the test.txt file
        TextAsset spawnData = Resources.Load(path) as TextAsset;

        string[] linesFromfile = spawnData.text.Split('\n');

        for (int i = 0; i < linesFromfile.Length; i+=7) {
            string line = linesFromfile[i];
            int num = int.Parse(line);

            line = linesFromfile[i+1];
            float min = float.Parse(line);

            line = linesFromfile[i + 2];
            float max = float.Parse(line);

            line = linesFromfile[i + 3];
            float chance = float.Parse(line);

            line = linesFromfile[i + 4];
            float freq = float.Parse(line);

            line = linesFromfile[i + 5];
            int souls = int.Parse(line);

            Wave w = new Wave(num, min, max, chance, freq, souls);
            waves.Add(w);
            line = linesFromfile[i + 6];
           // Debug.Assert(line.Equals(";"), "Error reading wave data. Semicolon delimiter missing");
        }

        return waves;
    }

}
