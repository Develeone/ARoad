using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

public static class CsvParser {
	
	public static List<Coordinate> ParseCsv ()
    {
		List<Coordinate> data = new List<Coordinate> ();	
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

		return data;
    }
}
