using UnityEngine;
using System.Collections;

public class RotateToCharacter : MonoBehaviour {

	Transform characterTransform;

	void Start () {
		characterTransform = Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt (characterTransform);
	}
}