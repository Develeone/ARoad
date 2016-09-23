using UnityEngine;
using System.Collections;

public class ObjectsPlacer : MonoBehaviour {

    public CsvToArr camCoordinates;

	// Use this for initialization
	void Start () {
        camCoordinates = new CsvToArr();
        Debug.Log(camCoordinates.data[0][0]);
        Debug.Log(camCoordinates.data[0][1]);
    }
	
	// Update is called once per frame
	void Update () {
	}

    void FixedUpdate()
    {

    }
}
