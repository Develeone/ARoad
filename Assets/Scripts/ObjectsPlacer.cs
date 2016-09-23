using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class ObjectsPlacer : MonoBehaviour {

	public List<Coordinate> camCoordinates;
    public List<Vector2> sceneCamCoordinates = new List<Vector2>();

    // Use this for initialization
    void Start () {
		camCoordinates = CsvParser.ParseCsv();

        worldToSceneCoordinates();
        Debug.Log(sceneCamCoordinates[0]);
    }
	
	// Update is called once per frame
	void Update () {
	}

    void FixedUpdate()
    {

    }


    void worldToSceneCoordinates()
    {
        foreach (Coordinate elem in camCoordinates)
            sceneCamCoordinates.Add(CoordinatesConverter.ConvertCoordinate(elem));
    }

}
