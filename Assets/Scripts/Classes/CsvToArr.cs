using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

public class CsvToArr : MonoBehaviour {

    public List<Coordinate> data = new List<Coordinate>();

    public CsvToArr ()
    {
        string filePath = @"Assets\Resources\cams.csv";
        StreamReader file = new StreamReader(filePath);

        while (!file.EndOfStream)
        {
            String[] line = file.ReadLine().Split(';');
            Coordinate coordinate = new Coordinate();

            coordinate.longitude = float.Parse(line[1]);
            coordinate.latitude = float.Parse(line[0]);

            data.Add(coordinate);
        }
    }
}
