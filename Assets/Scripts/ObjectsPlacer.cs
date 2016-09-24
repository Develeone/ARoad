using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class ObjectsPlacer : MonoBehaviour
{

    public List<Coordinate> camCoordinates;
    public List<Coordinate> gasStationCoordinates;
    public List<float[]> gasolinesPrises;
    public List<Coordinate> parkingCoordinates;

    public List<Vector2> sceneCamCoordinates = new List<Vector2>();
    public List<Vector2> sceneGasStationCoordinates = new List<Vector2>();
    public List<Vector2> sceneParkingCoordinates = new List<Vector2>();

    public GameObject speedCamPointer;
    public GameObject gasStationPointer;

    public TextAsset camsFile;
    public TextAsset gasolinesFile;
    public TextAsset parkingFile;

    public Transform camPointersParent;
    public Transform gasStationPointerParent;

    int camPointersCount        = 0;
    int gasStationPointersCount = 0;

    IEnumerator Start()
    {
        camCoordinates = CsvParser.ParseCsvCoordinates(camsFile);

        gasStationCoordinates = CsvParser.ParseCsvCoordinates(gasolinesFile);
        gasolinesPrises = CsvParser.ParceCsvPrises(gasolinesFile);

        parkingCoordinates = CsvParser.ParseCsvCoordinates(parkingFile);

        while (!GpsTracking.GpsReady)
        {
            Debug.Log("Object Placer is waiting until GPS ready...");
            yield return new WaitForSeconds(1);
        }

        worldToSceneCoordinates();

        foreach (Vector2 elem in sceneCamCoordinates)
        {
            Vector3 currentCam     = new Vector3(elem.x, 5, elem.y);
            float distanceToObject = Math.Abs(Vector3.Distance(new Vector3(GpsTracking.currentCoordinate.longitude, 5, GpsTracking.currentCoordinate.latitude), currentCam));

            if (distanceToObject < 500)
            {
                GameObject newCamPointer = (GameObject)GameObject.Instantiate(speedCamPointer, currentCam, Quaternion.identity);
                newCamPointer.transform.parent = camPointersParent;
                camPointersCount++;
            }
        }

        for (int i = 0; i < sceneGasStationCoordinates.Count; i++)
        {
            Vector3 currentGasStation = new Vector3(sceneGasStationCoordinates[i].x, 5, sceneGasStationCoordinates[i].y);
            float distanceToObject    = Math.Abs(Vector3.Distance(new Vector3(GpsTracking.currentCoordinate.longitude, 5, GpsTracking.currentCoordinate.latitude), currentGasStation));

            if (distanceToObject < 3000)
            {
                GameObject newGasStationPointer = (GameObject)GameObject.Instantiate(gasStationPointer, currentGasStation, Quaternion.identity);
                TextMesh priceTextMesh = newGasStationPointer.GetComponentInChildren<TextMesh>();
                priceTextMesh.text = gasolinesPrises[i][0] + "р";
                newGasStationPointer.transform.parent = gasStationPointerParent;
                gasStationPointersCount++;
            }
        }

    }

    void OnGUI()
    {
        GUILayout.Label("");
        GUILayout.Label("");
        GUILayout.Label("Camers instantiated: " + camPointersCount);
        GUILayout.Label("Gas Station instantiated: " + gasStationPointersCount);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {

    }


    void worldToSceneCoordinates()
    {
        foreach (Coordinate elem in camCoordinates)
            sceneCamCoordinates.Add(CoordinatesConverter.ConvertCoordinate(elem));

        foreach (Coordinate elem in gasStationCoordinates)
            sceneGasStationCoordinates.Add(CoordinatesConverter.ConvertCoordinate(elem));

        foreach (Coordinate elem in parkingCoordinates)
            sceneParkingCoordinates.Add(CoordinatesConverter.ConvertCoordinate(elem));
    }

}
