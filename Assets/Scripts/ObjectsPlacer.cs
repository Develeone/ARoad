using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class ObjectsPlacer : MonoBehaviour
{
    public List<float[]> gasolinesPrises;

    public List<Vector2> sceneCamCoordinates        = new List<Vector2>();
    public List<Vector2> sceneGasStationCoordinates = new List<Vector2>();
    public List<Vector2> sceneParkingCoordinates    = new List<Vector2>();

    public GameObject speedCamPointer;
    public GameObject gasStationPointer;
    public GameObject parkingPointer;

    public TextAsset camsFile;
    public TextAsset gasolinesFile;
    public TextAsset parkingFile;

    public Transform camPointersParent;
    public Transform gasStationPointerParent;
    public Transform parkingPointerParent;

    int camPointersCount        = 0;
    int gasStationPointersCount = 0;
    int parkingPointersCount    = 0;


    
    IEnumerator Start()
    {
        List<float[]> gasolinesPrises           = CsvParser.ParceCsvPrises(gasolinesFile);

        while (!GpsTracking.GpsReady)
            yield return new WaitForSeconds(1);

        sceneCamCoordinates = worldToSceneCoordinates(CsvParser.ParseCsvCoordinates(camsFile));
        sceneGasStationCoordinates = worldToSceneCoordinates(CsvParser.ParseCsvCoordinates(gasolinesFile));
        sceneParkingCoordinates = worldToSceneCoordinates(CsvParser.ParseCsvCoordinates(parkingFile));
    }

    void OnGUI()
    {
        GUILayout.Label("");
        GUILayout.Label("");
        GUILayout.Label("Camers instantiated: " + camPointersCount);
        GUILayout.Label("Gas Station instantiated: " + gasStationPointersCount);
        GUILayout.Label("Parking instantiated: " + parkingPointersCount);
    }
    
    void FixedUpdate()
    {
        if (!GpsTracking.GpsReady)
            return;

        foreach (Vector2 elem in sceneCamCoordinates)
        {
            Vector3 currentCam     = new Vector3(elem.x, 5, elem.y);
            float distanceToObject = Math.Abs(Vector3.Distance(new Vector3(GpsTracking.currentCoordinate.longitude, 5, GpsTracking.currentCoordinate.latitude), currentCam));

            if (distanceToObject < 500)
            {
                GameObject newCamPointer       = (GameObject)GameObject.Instantiate(speedCamPointer, currentCam, Quaternion.identity);
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

        foreach (Vector2 elem in sceneParkingCoordinates)
        {
            Vector3 currentParking = new Vector3(elem.x, 5, elem.y);
            float distanceToObject = Math.Abs(Vector3.Distance(new Vector3(GpsTracking.currentCoordinate.longitude, 5, GpsTracking.currentCoordinate.latitude), currentParking));

            if (distanceToObject < 1000)
            {
                GameObject newParkingPointer = (GameObject)GameObject.Instantiate(parkingPointer, currentParking, Quaternion.identity);
                newParkingPointer.transform.parent = parkingPointerParent;
                parkingPointersCount++;
            }
        }
    }


    List<Vector2> worldToSceneCoordinates(List<Coordinate> worldCoordinateList)
    {
        List<Vector2> sceneCoordinateList = new List<Vector2>(worldCoordinateList.Count);

        foreach (Coordinate elem in worldCoordinateList)
            sceneCoordinateList.Add(CoordinatesConverter.ConvertCoordinate(elem));

        return sceneCoordinateList;
    }

}
