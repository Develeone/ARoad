using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

public static class CsvParser {

	public static List<Coordinate> ParseCsv (TextAsset camsFile)
    {
		List<Coordinate> data = new List<Coordinate> ();	

		String content = camsFile.text;

		String[] contentArr = content.Split ('\n');

		for (int i = 0; i < contentArr.Length; i++)
        {
			String[] line = contentArr[i].Split(';');
            Coordinate coordinate = new Coordinate();

            coordinate.longitude = float.Parse(line[1]);
            coordinate.latitude = float.Parse(line[0]);

            data.Add(coordinate);
        }

		return data;
    }
}
