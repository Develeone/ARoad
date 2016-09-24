using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class ObjectsPlacer : MonoBehaviour {

	public List<Coordinate> camCoordinates;
	public Transform camPointersParent;
    public List<Coordinate> gasolinesCoordinates;
    public List<float[]> gasolinesPrises;

    public List<Vector2> sceneCamCoordinates       = new List<Vector2>();
    public List<Vector2> sceneGasolinesCoordinates = new List<Vector2>();

    public GameObject SpeedCamPointer;

	public TextAsset camsFile;
    public TextAsset gasolinesFile;

	int camPointersCount = 0;

	IEnumerator Start () {
		camCoordinates       = CsvParser.ParseCsvCoordinates(camsFile);
        gasolinesCoordinates = CsvParser.ParseCsvCoordinates(gasolinesFile);
        gasolinesPrises      = CsvParser.ParceCsvPrises(gasolinesFile);

        while (!GpsTracking.GpsReady) {
			Debug.Log ("Object Placer is waiting until GPS ready...");
			yield return new WaitForSeconds (1);
		}

        worldToSceneCoordinates();

		foreach (Vector2 elem in sceneCamCoordinates) {
            Vector3 currentCam = new Vector3(elem.x, 5, elem.y);
            float distanceToObject = Math.Abs(Vector3.Distance(new Vector3(GpsTracking.currentCoordinate.longitude, 5, GpsTracking.currentCoordinate.latitude), currentCam));
            if (distanceToObject < 500)
            {
				GameObject newCamPointer = (GameObject)GameObject.Instantiate (SpeedCamPointer, new Vector3 (elem.x, 5, elem.y), Quaternion.identity);
				newCamPointer.transform.parent = camPointersParent;
				camPointersCount++;
            }
		}
    }

	void OnGUI () {
		GUILayout.Label ("");
		GUILayout.Label ("");
		GUILayout.Label ("Cubes instantiated: " + camPointersCount);
	}

	// Update is called once per frame
	void Update () {
	}

    void FixedUpdate()
    {

    }


    void worldToSceneCoordinates()
    {
		foreach (Coordinate elem in camCoordinates) {
			sceneCamCoordinates.Add (CoordinatesConverter.ConvertCoordinate (elem));
		}

        foreach (Coordinate elem in gasolinesCoordinates)
            sceneGasolinesCoordinates.Add(CoordinatesConverter.ConvertCoordinate(elem));
    }

}
