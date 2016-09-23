using UnityEngine;
using System.Collections;

public class CharacterPositionController : MonoBehaviour {

	public Transform character;

	Vector2 characterPosition = new Vector2 (); // в метрах - на карте сцены Юнити
	public float characterRotation = 0;

	public Coordinates currentCoordinates = new Coordinates(); // Координаты в текущий момент
	public Coordinates startCoordinates = new Coordinates(); // Координаты в момент запуска приложения

	string displayMessage = ""; // Вывод справочной инфы в OnGUI

	IEnumerator Start () {
		// Если у нашего юзера отключена геолокация
		// TODO: Вывод ошибки!
		if (!Input.location.isEnabledByUser) {
			Debug.LogError ("Location service disabled by user!");
			displayMessage = "Location service disabled by user!";
			yield break;
		}

		// Запускаем сервис
		Input.compass.enabled = true;
		Input.gyro.enabled = true;
		Input.location.Start(1f, 1f);

		// Ждем инициализации
		int maxWait = 20;
		while (Input.location.status != LocationServiceStatus.Initializing && maxWait > 0) {
			yield return new WaitForSeconds (1);
			maxWait--;
			Debug.Log ("1 second left");
			displayMessage = "1 second left";
		}

		// Если не успел иницализироваться
		// TODO: Вывод ошибки!
		if (maxWait < 1) {
			Debug.LogError ("Location service timed out!");
			displayMessage = "Location service timed out!";
			yield break;
		}

		// Если подключение закрашилось
		// TODO: Вывод ошибки!
		if (Input.location.status == LocationServiceStatus.Failed) {
			Debug.LogError ("Location service connection failed!");
			displayMessage = "Location service connection failed!";
			yield break;
		} else {
			// Если всё прошло успешно
			Debug.Log ("Success!");
			startCoordinates = new Coordinates (Input.location.lastData.latitude, Input.location.lastData.longitude);
			currentCoordinates = startCoordinates;

			// Тут мы типа отправляем на сервак наши стартовые GPS-координаты,
			// а игровые координаты будут 0,0,0.
		}

	}


	
	private double _lastCompassUpdateTime = 0;
    private Quaternion _correction = Quaternion.identity;
    private Quaternion _targetCorrection = Quaternion.identity;
    private Quaternion _compassOrientation = Quaternion.identity;
	
	void Update () {
		// Данные GPS-ника пишем в переменную
		Coordinates lastData = new Coordinates (Input.location.lastData.latitude, Input.location.lastData.longitude);

		if (Input.location.status == LocationServiceStatus.Running) {
			if (currentCoordinates.latitude != lastData.latitude || currentCoordinates.longitude != lastData.longitude) {
				// Позиционирование
				currentCoordinates = new Coordinates (lastData.latitude, lastData.longitude);
			}

			displayMessage = currentCoordinates.latitude + " " + currentCoordinates.longitude + " " + Input.compass.trueHeading.ToString();
		}



		// Ротация с помощью компаса (должна была бы работать)
		float xrot = Mathf.Atan2(Input.acceleration.z, Input.acceleration.y);
		float yzmag = Mathf.Sqrt(Mathf.Pow(Input.acceleration.y, 2) + Mathf.Pow(Input.acceleration.z, 2));
		float zrot = Mathf.Atan2 (Input.acceleration.x, yzmag);

		float xangle = xrot * (180 / Mathf.PI) + 90;
		float zangle = -zrot * (180 / Mathf.PI);

		//character.eulerAngles = new Vector3 (xangle, 0, zangle - Input.compass.trueHeading);
		characterRotation = Mathf.LerpAngle(characterRotation, Input.compass.trueHeading, Time.deltaTime);
		character.eulerAngles = new Vector3 (0, characterRotation, 0);




        // The gyro is very effective for high frequency movements, but drifts its
        // orientation over longer periods, so we want to use the compass to correct it.
        // The iPad's compass has low time resolution, however, so we let the gyro be
        // mostly in charge here.
       
        // First we take the gyro's orientation and make a change of basis so it better
        // represents the orientation we'd like it to have
/*        Quaternion gyroOrientation = Quaternion.Euler (90, 0, 0) * Input.gyro.attitude * Quaternion.Euler(0, 0, 90);
   
        // See if the compass has new data
        if (Input.compass.timestamp > _lastCompassUpdateTime)
        {
            _lastCompassUpdateTime = Input.compass.timestamp;
       
            // Work out an orientation based primarily on the compass
            Vector3 gravity = Input.gyro.gravity.normalized;
            Vector3 flatNorth = Input.compass.rawVector - Vector3.Dot(gravity, Input.compass.rawVector) * gravity;
            _compassOrientation = Quaternion.Euler (180, 0, 0) * Quaternion.Inverse(Quaternion.LookRotation(flatNorth, -gravity)) * Quaternion.Euler (0, 0, 90);
            // Calculate the target correction factor
            _targetCorrection = _compassOrientation * Quaternion.Inverse(gyroOrientation);
        }
       
        // Jump straight to the target correction if it's a long way; otherwise, slerp towards it very slowly
        if (Quaternion.Angle(_correction, _targetCorrection) > 45)
            _correction = _targetCorrection;
        else
            _correction = Quaternion.Slerp(_correction, _targetCorrection, 0.02f);
       
        // Easy bit :)
        character.rotation = _correction * gyroOrientation;
		//character.eulerAngles = new Vector3(character.eulerAngles.x, character.eulerAngles.y, character.eulerAngles.z+90);

*/

		//Debug.Log(distanceBetweenCoordinates(float currentLatitude, float longitude1, float latitude2, float longitude2));

		if (Input.GetKey (KeyCode.LeftArrow))
			currentCoordinates.latitude -= 0.00001f;
		if (Input.GetKey (KeyCode.RightArrow))
			currentCoordinates.latitude += 0.00001f;
		if (Input.GetKey (KeyCode.UpArrow))
			currentCoordinates.longitude += 0.00001f;
		if (Input.GetKey (KeyCode.DownArrow))
			currentCoordinates.longitude -= 0.00001f;

		characterPosition.x = (currentCoordinates.latitude - startCoordinates.latitude) * 10000f;
		characterPosition.y = (currentCoordinates.longitude - startCoordinates.longitude) * 10000f;

		Vector3 targetPosition = new Vector3 (characterPosition.x, character.position.y, characterPosition.y);
		character.position = Vector3.Lerp(character.position, targetPosition, Time.deltaTime * 15f);
	}

	void OnGUI () {
		//GUILayout.Label (displayMessage);
	}
}