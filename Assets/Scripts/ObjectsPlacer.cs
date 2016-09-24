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
    public List<Vector2> sceneTiresCoordinates      = new List<Vector2>();

    public GameObject[] instantietedCams;
    public GameObject[] instantietedGasStations;
    public GameObject[] instantietedParkings;
    public GameObject[] instantietedPitStops;
    public GameObject[] instantietedTires;

    public GameObject speedCamPointer;
    public GameObject gasStationPointer;
    public GameObject parkingPointer;
    public GameObject pitStopPointer;
    public GameObject tiresPointer;

    public TextAsset camsFile;
    public TextAsset gasolinesFile;
    public TextAsset parkingFile;
    public TextAsset pitStopFile;
    public TextAsset tiresFile;

    public Transform camPointersParent;
    public Transform gasStationPointerParent;
    public Transform parkingPointerParent;
    public Transform pitStopPointerParent;
    public Transform tiresPointerParent;

    int camPointersCount        = 0;
    int gasStationPointersCount = 0;
    int parkingPointersCount    = 0;
    int pitStopPointersCount    = 0;
    int tiresPointersCount      = 0;

    IEnumerator Start()
    {
        gasolinesPrises = CsvParser.ParceCsvPrises(gasolinesFile);

        while (!GpsTracking.GpsReady)
            yield return new WaitForSeconds(1);

        PlayerPrefs.SetInt("showCam", 1);
        PlayerPrefs.SetInt("showTires", 1);

        if (PlayerPrefs.GetInt("showCam") == 1)
            sceneCamCoordinates        = worldToSceneCoordinates(CsvParser.ParseCsvCoordinates(camsFile));

        if (PlayerPrefs.GetInt("showGasStation") == 1)
            sceneGasStationCoordinates = worldToSceneCoordinates(CsvParser.ParseCsvCoordinates(gasolinesFile));

        if (PlayerPrefs.GetInt("showParking") == 1)
            sceneParkingCoordinates    = worldToSceneCoordinates(CsvParser.ParseCsvCoordinates(parkingFile));

        if (PlayerPrefs.GetInt("showPitStop") == 1)
            scenePitStopCoordinates    = worldToSceneCoordinates(CsvParser.ParseCsvCoordinates(pitStopFile));

        if (PlayerPrefs.GetInt("showTires") == 1)
            sceneTiresCoordinates      = worldToSceneCoordinates(CsvParser.ParseCsvCoordinates(tiresFile));

        instantietedCams        = new GameObject[sceneCamCoordinates.Count];
        instantietedGasStations = new GameObject[sceneGasStationCoordinates.Count];
        instantietedParkings    = new GameObject[sceneParkingCoordinates.Count];
        instantietedPitStops    = new GameObject[scenePitStopCoordinates.Count];
        instantietedTires       = new GameObject[sceneTiresCoordinates.Count];
    }

    void OnGUI()
    {
        GUILayout.Label("");
        GUILayout.Label("");
        GUILayout.Label("Camers instantiated: " + camPointersCount);
        GUILayout.Label("Gas Station instantiated: " + gasStationPointersCount);
        GUILayout.Label("Parking instantiated: " + parkingPointersCount);
        GUILayout.Label("PitStop instantiated: " + pitStopPointersCount);
        GUILayout.Label("Tires instantiated: " + tiresPointersCount);
    }
    
    void FixedUpdate()
    {
        if (!GpsTracking.GpsReady)
            return;

        InstancePointer(sceneCamCoordinates, 500, speedCamPointer, camPointersParent, ref camPointersCount, false, instantietedCams);
        InstancePointer(sceneGasStationCoordinates, 1000, gasStationPointer, gasStationPointerParent, ref gasStationPointersCount, true, instantietedGasStations);
        InstancePointer(sceneParkingCoordinates, 1000, parkingPointer, parkingPointerParent, ref parkingPointersCount, false, instantietedParkings);
        InstancePointer(scenePitStopCoordinates, 1000, pitStopPointer, pitStopPointerParent, ref pitStopPointersCount, false, instantietedPitStops);
        InstancePointer(sceneTiresCoordinates, 1000, tiresPointer, tiresPointerParent, ref tiresPointersCount, false, instantietedTires);

    }

    List<Vector2> worldToSceneCoordinates(List<Coordinate> worldCoordinateList)
    {
        List<Vector2> sceneCoordinateList = new List<Vector2>(worldCoordinateList.Count);

        foreach (Coordinate elem in worldCoordinateList)
            sceneCoordinateList.Add(CoordinatesConverter.ConvertCoordinate(elem));

        return sceneCoordinateList;
    }

    void InstancePointer(List<Vector2> sceneCoordinateList, int requiredDistance, GameObject currentPointer, Transform pointerParent, ref int counter, bool setPointerText, GameObject[] instantietedPointerList)
    {

        for (int i = 0; i < sceneCoordinateList.Count; i++)
        {
            Vector3 currentPointerPosition = new Vector3(sceneCoordinateList[i].x, 5, sceneCoordinateList[i].y);



            float distanceToObject = Math.Abs(Vector3.Distance(new Vector3(MovementController.characterPosition.x, 5, MovementController.characterPosition.y), currentPointerPosition));


            if (distanceToObject < requiredDistance && instantietedPointerList[i] == null)
            {
                GameObject newPointer = (GameObject)GameObject.Instantiate(currentPointer, currentPointerPosition, Quaternion.identity);

                instantietedPointerList[i] = newPointer;

                TextMesh[] textMeshes = newPointer.GetComponentsInChildren<TextMesh>();

                int distanceIndex = textMeshes[0].text.Equals("Price") ? 0 : 1;

                if (setPointerText)
                {
                    TextMesh priceTextMesh = textMeshes[distanceIndex];
                    priceTextMesh.text = gasolinesPrises[i][PlayerPrefs.GetInt("gasolineType")] + "р";
                }

                newPointer.transform.parent = pointerParent;
                counter++;
            }
            else if (distanceToObject > requiredDistance && instantietedPointerList[i] != null)
            {
                GameObject.Destroy(instantietedPointerList[i]);
                instantietedPointerList[i] = null;
                counter--;
            }
                
        }
    
    }

}
