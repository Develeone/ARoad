using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class ObjectsPlacer : MonoBehaviour {

	public List<Coordinate> camCoordinates;
    public List<Vector2> sceneCamCoordinates = new List<Vector2>();
	public Transform camPointersParent;
    public GameObject SpeedCamPointer;

	public TextAsset camsFile;

	int camPointersCount = 0;

    // Use this for initialization
	IEnumerator Start () {
		camCoordinates = CsvParser.ParseCsv(camsFile);

		while (!GpsTracking.GpsReady) {
			Debug.Log ("Object Placer is waiting until GPS ready...");
			yield return new WaitForSeconds (1);
		}

        worldToSceneCoordinates();

		foreach (Vector2 elem in sceneCamCoordinates) {
			GameObject newCamPointer = (GameObject)GameObject.Instantiate (SpeedCamPointer, new Vector3 (elem.x, 5, elem.y), Quaternion.identity);
			newCamPointer.transform.parent = camPointersParent;
			camPointersCount++;
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
//			Debug.Log (GpsTracking.startCoordinate.latitude + " " + GpsTracking.startCoordinate.longitude + " " + elem.latitude + " " + elem.longitude + " " + CoordinatesConverter.ConvertCoordinate (elem).x + " " + CoordinatesConverter.ConvertCoordinate (elem).y);
			sceneCamCoordinates.Add (CoordinatesConverter.ConvertCoordinate (elem));
		}
    }

}
