using UnityEngine;
using System.Collections;

public static class ServerResponseHandler {

    public static void HandleServerResponse(string message)
    {
        string[] messageParts = message.Split(';');

        Coordinate inputGpsCoordinate = new Coordinate(float.Parse(messageParts[0]), float.Parse(messageParts[1]));
        Vector2 inputSceneCoordinate = CoordinatesConverter.ConvertCoordinate(inputGpsCoordinate);

        if (messageParts[2].Equals("trafficAccident"))
        {

        }
        if (messageParts[2].Equals("police"))
        {

        }
        if (messageParts[2].Equals("message"))
        {

        }
        //TODO запихивание в массив из ObjectPlacer
    }
}
