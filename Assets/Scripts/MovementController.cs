using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour {

	public Transform character;

	Vector2 characterPosition = new Vector2 (); // в метрах - на карте сцены Юнити
	public float characterRotation = 0;

	void Start () {
		// Запускаем сервис
		Input.compass.enabled = true;
		Input.gyro.enabled = true;
	}

	void Update () {
		characterRotation = Mathf.LerpAngle(characterRotation, Input.compass.trueHeading, Time.deltaTime);
		character.eulerAngles = new Vector3 (0, characterRotation, 0);

		characterPosition = CoordinatesConverter.ConvertCoordinate(GpsTracking.currentCoordinate);

		Vector3 targetPosition = new Vector3 (characterPosition.x, character.position.y, characterPosition.y);
		character.position = Vector3.Lerp(character.position, targetPosition, Time.deltaTime * 15f);
	}

	void OnGUI () {
		GUILayout.Label (CoordinatesConverter.ConvertCoordinate(GpsTracking.currentCoordinate).ToString());
	}
}