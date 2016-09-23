using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

public class CsvToArr : MonoBehaviour {

    public List<List<float>> data = new List<List<float>>();

    public CsvToArr ()
    {
        string filePath = @"Assets\Resources\cams.csv";
        StreamReader file = new StreamReader(filePath);

        while (!file.EndOfStream)
        {
            String[] line = file.ReadLine().Split(';');
            List<float> coordinate = new List<float>();

            foreach (String elem in line)
                coordinate.Add(float.Parse(elem));

            data.Add(coordinate);
        }
    }
}
