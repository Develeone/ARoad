﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class ObjectsPlacer : MonoBehaviour {

    public CsvToArr camCoordinates;
    public List<Vector2> sceneCamCoordinates = new List<Vector2>();
    public GameObject SpeedCamPointer;

    // Use this for initialization
    void Start () {
        camCoordinates = new CsvToArr();

        worldToSceneCoordinates();

        foreach (Vector2 elem in sceneCamCoordinates)
            GameObject.Instantiate(SpeedCamPointer, new Vector3(elem.x, 5, elem.y), Quaternion.identity);
    }
	
	// Update is called once per frame
	void Update () {
	}

    void FixedUpdate()
    {

    }


    void worldToSceneCoordinates()
    {
        foreach (Coordinate elem in camCoordinates.data)
            sceneCamCoordinates.Add(CoordinatesConverter.ConvertCoordinate(elem));
    }

}
