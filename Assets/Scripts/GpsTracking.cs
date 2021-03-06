﻿using UnityEngine;
using System.Collections;

public class GpsTracking : MonoBehaviour {

    // Use this for initialization
	static public Coordinate currentCoordinate = new Coordinate(); // Координаты в текущий момент
	static public Coordinate startCoordinate = new Coordinate(); // Координаты в момент запуска приложения

	static public bool GpsReady = false;

    string displayMessage = ""; // Вывод справочной инфы в OnGUI

	void Awake () {
		StartCoroutine ("StartTracking");
	}

    IEnumerator StartTracking()
    {

		#if UNITY_EDITOR
			yield return new WaitForSeconds(1);
            startCoordinate = new Coordinate(43.025191f, 131.8923505f);
			currentCoordinate = startCoordinate;

			Debug.Log ("GPS Tracker ready! Start coordinates are set to: " + startCoordinate.latitude + " " + startCoordinate.longitude);
	
			GpsReady = true;
			yield break;
        #endif

        // Если у нашего юзера отключена геолокация
        // TODO: Вывод ошибки!
        if (!Input.location.isEnabledByUser)
        {
            Debug.LogError("Location service disabled by user!");
            displayMessage = "Location service disabled by user!";

            yield break;
        }

        // Запускаем сервис
		Input.location.Start(0.001f, 0.001f);

        // Ждем инициализации
        int maxWait = 20;
		while (Input.location.status != LocationServiceStatus.Running && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
            Debug.Log("1 second left");
            displayMessage = "1 second left";
        }

        // Если не успел иницализироваться
        // TODO: Вывод ошибки!
        if (maxWait < 1)
        {
            Debug.LogError("Location service timed out!");
            displayMessage = "Location service timed out!";
            yield break;
        }

        // Если подключение закрашилось
        // TODO: Вывод ошибки!
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.LogError("Location service connection failed!");
            displayMessage = "Location service connection failed!";
            yield break;
        }
        else {
            // Если всё прошло успешно
            Debug.Log("Success!");
            startCoordinate = new Coordinate(Input.location.lastData.latitude, Input.location.lastData.longitude);
            currentCoordinate = startCoordinate;
        }

		GpsReady = true;

		Debug.Log ("GPS Tracker ready! Start coordinates are set to: " + startCoordinate.latitude + " " + startCoordinate.longitude);
    }

    void Update()
    {
		#if UNITY_EDITOR
			return;
		#endif

		Coordinate lastData = new Coordinate(Input.location.lastData.latitude, Input.location.lastData.longitude);

        if (Input.location.status == LocationServiceStatus.Running)
        {
            if (currentCoordinate.latitude != lastData.latitude || currentCoordinate.longitude != lastData.longitude)
            {
				currentCoordinate = new Coordinate(lastData.latitude, lastData.longitude);
            }
            displayMessage = currentCoordinate.latitude + " " + currentCoordinate.longitude + " " + Input.compass.trueHeading.ToString();
        }
    }

    void OnGUI()
    {
		GUILayout.Label ("");
        GUILayout.Label(displayMessage);
    }
}
