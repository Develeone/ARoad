using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour {

	public Transform character;

	public Vector2 characterPosition = new Vector2 (); // в метрах - на карте сцены Юнити
	public Vector2 characterPositione = new Vector2 (); // в метрах - на карте сцены Юнити
	public float characterRotation = 0;

	void Start () {
		// Запускаем сервис
		Input.compass.enabled = true;
		Input.gyro.enabled = true;
	}

	public Vector3 velocity = Vector3.zero;

	void Update () {

		float rotation = Input.compass.trueHeading + 75;
		rotation = rotation > 359 ? rotation - 359 : rotation;
		characterRotation = Mathf.LerpAngle(characterRotation, rotation, Time.deltaTime);
		character.eulerAngles = new Vector3 (Input.gyro.gravity.x, characterRotation, Input.gyro.gravity.z);

		characterPosition = CoordinatesConverter.ConvertCoordinate(GpsTracking.currentCoordinate);

		#if UNITY_EDITOR
			characterPosition = characterPositione;
		#endif

		Vector3 targetPosition = new Vector3 (characterPosition.x, character.position.y, characterPosition.y);
		character.position = Vector3.SmoothDamp(character.position, targetPosition, ref velocity, Time.deltaTime * 10f);
	}

	void OnGUI () {
		GUILayout.Label (CoordinatesConverter.ConvertCoordinate(GpsTracking.currentCoordinate).ToString() + ", rot = " + characterRotation);
	}
}