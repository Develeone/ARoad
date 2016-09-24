using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

public static class CsvParser {

    public static List<Coordinate> ParseCsvCoordinates(TextAsset file)
    {
        List<Coordinate> data = new List<Coordinate>();
       
        String content = file.text;

        String[] contentArr = content.Split('\n');

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

    public static List<float[]> ParceCsvPrises (TextAsset file)
    {
        List<float[]> gasolinePrises = new List<float[]>();
        float[] currentPrices = new float[3];

        String content = file.text;

        String[] contentArr = content.Split('\n');

        for (int i = 0; i < contentArr.Length; i++)
        {
            String[] line = contentArr[i].Split(';');

            currentPrices = new float[] { float.Parse(line[2]), float.Parse(line[3]), float.Parse(line[4]) };

            gasolinePrises.Add(currentPrices);
        }

        return gasolinePrises;
    }
}
