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
    public List<Vector2> scenePitStopCoordinates    = new List<Vector2>();

    public GameObject speedCamPointer;
    public GameObject gasStationPointer;
    public GameObject parkingPointer;
    public GameObject pitStopPointer;

    public TextAsset camsFile;
    public TextAsset gasolinesFile;
    public TextAsset parkingFile;
    public TextAsset pitStopFile;

    public Transform camPointersParent;
    public Transform gasStationPointerParent;
    public Transform parkingPointerParent;
    public Transform pitStopPointerParent;

    int camPointersCount        = 0;
    int gasStationPointersCount = 0;
    int parkingPointersCount    = 0;
    int pitStopPointersCount    = 0;



    IEnumerator Start()
    {
        gasolinesPrises           = CsvParser.ParceCsvPrises(gasolinesFile);

        while (!GpsTracking.GpsReady)
            yield return new WaitForSeconds(1);

        sceneCamCoordinates        = worldToSceneCoordinates(CsvParser.ParseCsvCoordinates(camsFile));
        sceneGasStationCoordinates = worldToSceneCoordinates(CsvParser.ParseCsvCoordinates(gasolinesFile));
        sceneParkingCoordinates    = worldToSceneCoordinates(CsvParser.ParseCsvCoordinates(parkingFile));
        scenePitStopCoordinates    = worldToSceneCoordinates(CsvParser.ParseCsvCoordinates(pitStopFile));
    }

    void OnGUI()
    {
        GUILayout.Label("");
        GUILayout.Label("");
        GUILayout.Label("Camers instantiated: " + camPointersCount);
        GUILayout.Label("Gas Station instantiated: " + gasStationPointersCount);
        GUILayout.Label("Parking instantiated: " + parkingPointersCount);
        GUILayout.Label("PitStop instantiated: " + pitStopPointersCount);
    }
    
    void FixedUpdate()
    {
        if (!GpsTracking.GpsReady)
            return;

        InstancePointer(sceneCamCoordinates, 500, speedCamPointer, camPointersParent, ref camPointersCount, false);
        InstancePointer(sceneGasStationCoordinates, 2000, gasStationPointer, gasStationPointerParent, ref gasStationPointersCount, true);
        InstancePointer(sceneParkingCoordinates, 1000, parkingPointer, parkingPointerParent, ref parkingPointersCount, false);
        InstancePointer(scenePitStopCoordinates, 1000, pitStopPointer, pitStopPointerParent, ref pitStopPointersCount, false);

    }

    List<Vector2> worldToSceneCoordinates(List<Coordinate> worldCoordinateList)
    {
        List<Vector2> sceneCoordinateList = new List<Vector2>(worldCoordinateList.Count);

        foreach (Coordinate elem in worldCoordinateList)
            sceneCoordinateList.Add(CoordinatesConverter.ConvertCoordinate(elem));

        return sceneCoordinateList;
    }

    void InstancePointer(List<Vector2> sceneCoordinateList, int requiredDistance, GameObject currentPointer, Transform pointerParent, ref int counter, bool setPointerText)
    {

        for (int i = 0; i < sceneCoordinateList.Count; i++)
        {
            Vector3 currentPointerPosition = new Vector3(sceneCoordinateList[i].x, 5, sceneCoordinateList[i].y);
            float distanceToObject = Math.Abs(Vector3.Distance(new Vector3(GpsTracking.currentCoordinate.longitude, 5, GpsTracking.currentCoordinate.latitude), currentPointerPosition));

            if (distanceToObject < requiredDistance)
            {
                GameObject newPointer = (GameObject)GameObject.Instantiate(currentPointer, currentPointerPosition, Quaternion.identity);

                if (setPointerText)
                {
                    TextMesh priceTextMesh = newPointer.GetComponentInChildren<TextMesh>();
                    priceTextMesh.text = gasolinesPrises[i][0] + "р";
                }

                newPointer.transform.parent = pointerParent;
                counter++;
            }
        }
    
    }

}
