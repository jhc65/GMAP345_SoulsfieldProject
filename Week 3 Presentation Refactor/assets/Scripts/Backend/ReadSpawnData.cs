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

        string path = "Assets/Resources/SpawnData.txt";

        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);

        while(!reader.EndOfStream) {
            string line = reader.ReadLine();
            int num = int.Parse(line);

            line = reader.ReadLine();
            float min = float.Parse(line);

            line = reader.ReadLine();
            float max = float.Parse(line);

            line = reader.ReadLine();
            float chance = float.Parse(line);

            line = reader.ReadLine();
            float freq = float.Parse(line);

            Wave w = new Wave(num, min, max, chance, freq);
            waves.Add(w);
            line = reader.ReadLine();
            Debug.Assert(line.Equals(";"), "Error reading wave data. Semicolon delimiter missing");
        }

        reader.Close();
        return waves;
    }

}
