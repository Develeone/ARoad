using UnityEngine;
using System.Collections;

public class GpsTracking : MonoBehaviour {

    // Use this for initialization
    static public Coordinate currentCoordinate = new Coordinate(); // Координаты в текущий момент
    static public Coordinate startCoordinate = new Coordinate(); // Координаты в момент запуска приложения

    string displayMessage = ""; // Вывод справочной инфы в OnGUI

    // Update is called once per frame
    IEnumerator Start()
    {
        #if UNITY_EDITOR
            startCoordinate = new Coordinate(43.02749f, 131.8884f);
            currentCoordinate = startCoordinate;
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
        Input.location.Start(1f, 1f);

        // Ждем инициализации
        int maxWait = 20;
        while (Input.location.status != LocationServiceStatus.Initializing && maxWait > 0)
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

    }

    void Update()
    {
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
