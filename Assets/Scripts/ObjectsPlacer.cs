using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class ObjectsPlacer : MonoBehaviour
{
    public List<float[]> gasolinesPrises;
    public List<string> Messages;

    public List<Vector2> sceneCamCoordinates        = new List<Vector2>();
    public List<Vector2> sceneGasStationCoordinates = new List<Vector2>();
    public List<Vector2> sceneParkingCoordinates    = new List<Vector2>();
    public List<Vector2> scenePitStopCoordinates    = new List<Vector2>();
    public List<Vector2> sceneTiresCoordinates      = new List<Vector2>();
    public List<Vector2> sceneCrashCoordinates      = new List<Vector2>();
    public List<Vector2> scenePoliceCoordinates     = new List<Vector2>();
    public List<Vector2> sceneMessageCoordinates    = new List<Vector2>();

    public GameObject[] instantietedCams;
    public GameObject[] instantietedGasStations;
    public GameObject[] instantietedParkings;
    public GameObject[] instantietedPitStops;
    public GameObject[] instantietedTires;
    public GameObject[] instantietedCrash;
    public GameObject[] instantietedPolice;
    public GameObject[] instantietedMessage;

    public GameObject speedCamPointer;
    public GameObject gasStationPointer;
    public GameObject parkingPointer;
    public GameObject pitStopPointer;
    public GameObject tiresPointer;
    public GameObject crashPointer;
    public GameObject policePointer;
    public GameObject messagePointer;

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
    public Transform crashPointerParent;
    public Transform policePointerParent;
    public Transform messagePointerParent;

    IEnumerator Start()
    {
        gasolinesPrises = CsvParser.ParceCsvPrises(gasolinesFile);

        while (!GpsTracking.GpsReady)
            yield return new WaitForSeconds(1);

        if (PlayerPrefs.GetInt("showCam") == 1)
            sceneCamCoordinates        = worldToSceneCoordinates(CsvParser.ParseCsvCoordinates(camsFile));

        if (PlayerPrefs.GetInt("showGasStation") == 1)
            sceneGasStationCoordinates = worldToSceneCoordinates(CsvParser.ParseCsvCoordinates(gasolinesFile));

        if (PlayerPrefs.GetInt("showParking") == 1)
            sceneParkingCoordinates    = worldToSceneCoordinates(CsvParser.ParseCsvCoordinates(parkingFile));

        if (PlayerPrefs.GetInt("showPitStop") == 1)
            scenePitStopCoordinates    = worldToSceneCoordinates(CsvParser.ParseCsvCoordinates(pitStopFile));

        if (PlayerPrefs.GetInt("showTire") == 1)
            sceneTiresCoordinates      = worldToSceneCoordinates(CsvParser.ParseCsvCoordinates(tiresFile));

        instantietedCams        = new GameObject[sceneCamCoordinates.Count];
        instantietedGasStations = new GameObject[sceneGasStationCoordinates.Count];
        instantietedParkings    = new GameObject[sceneParkingCoordinates.Count];
        instantietedPitStops    = new GameObject[scenePitStopCoordinates.Count];
        instantietedTires       = new GameObject[sceneTiresCoordinates.Count];
        instantietedCrash       = new GameObject[sceneCrashCoordinates.Count];
        instantietedPolice      = new GameObject[scenePoliceCoordinates.Count];
        instantietedMessage     = new GameObject[sceneMessageCoordinates.Count];
    }

    void OnGUI()
    {
        GUILayout.Label("");
        GUILayout.Label("");
    }
    
    void FixedUpdate()
    {
        if (!GpsTracking.GpsReady)
            return;

        InstancePointer(sceneCamCoordinates, 500, speedCamPointer, camPointersParent, false, instantietedCams);
        InstancePointer(sceneGasStationCoordinates, 1000, gasStationPointer, gasStationPointerParent, true, instantietedGasStations);
        InstancePointer(sceneParkingCoordinates, 1000, parkingPointer, parkingPointerParent, false, instantietedParkings);
        InstancePointer(scenePitStopCoordinates, 1000, pitStopPointer, pitStopPointerParent, false, instantietedPitStops);
        InstancePointer(sceneTiresCoordinates, 1000, tiresPointer, tiresPointerParent, false, instantietedTires);
        InstancePointer(sceneCrashCoordinates, 1000, crashPointer, crashPointerParent, false, instantietedCrash);
        InstancePointer(scenePoliceCoordinates, 1000, policePointer, policePointerParent, false, instantietedPolice);
        InstancePointer(sceneMessageCoordinates, 1000, messagePointer, messagePointerParent, false, instantietedMessage);

    }

    List<Vector2> worldToSceneCoordinates(List<Coordinate> worldCoordinateList)
    {
        List<Vector2> sceneCoordinateList = new List<Vector2>(worldCoordinateList.Count);

        foreach (Coordinate elem in worldCoordinateList)
            sceneCoordinateList.Add(CoordinatesConverter.ConvertCoordinate(elem));

        return sceneCoordinateList;
    }

    void InstancePointer(List<Vector2> sceneCoordinateList, int requiredDistance, GameObject currentPointer, Transform pointerParent, bool setPointerText, GameObject[] instantietedPointerList)
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
                    TextMesh priceTextMesh = newPointer.GetComponentInChildren<TextMesh>();
                    priceTextMesh.text = gasolinesPrises[i][PlayerPrefs.GetInt("gasolineType")] + "р";
                }

                newPointer.transform.parent = pointerParent;
            }
            else if (distanceToObject > requiredDistance && instantietedPointerList[i] != null)
            {
                GameObject.Destroy(instantietedPointerList[i]);
                instantietedPointerList[i] = null;
            }
                
        }
    
    }

}
