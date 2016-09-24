using UnityEngine;
using System.Collections;

public static class ServerResponseHandler {

    public static void HandleServerResponse(string message)
    {

        string[] messageAttrs = message.Split(';');

        Coordinate inputGpsCoordinate = new Coordinate(float.Parse(messageAttrs[0]), float.Parse(messageAttrs[1]));
        Vector2 inputSceneCoordinate = CoordinatesConverter.ConvertCoordinate(inputGpsCoordinate);

        if (messageAttrs[2].Equals("trafficAccident"))
            ObjectsPlacer.sceneCrashCoordinates.Add(inputSceneCoordinate);
        if (messageAttrs[2].Equals("police"))
            ObjectsPlacer.scenePoliceCoordinates.Add(inputSceneCoordinate);
        if (messageAttrs[2].Equals("message"))
        {
            ObjectsPlacer.sceneMessageCoordinates.Add(inputSceneCoordinate);
            ObjectsPlacer.Messages.Add(messageAttrs[3]);
        }

    }
}
