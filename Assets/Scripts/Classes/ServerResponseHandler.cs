using UnityEngine;
using System.Collections;

public static class ServerResponseHandler {

    public static void HandleServerResponse(string message)
    {

        string[] messageAttrs = message.Split(';');
        Debug.Log("attrs arr lenth: " + messageAttrs.Length);

        Coordinate inputGpsCoordinate = new Coordinate(float.Parse(messageAttrs[0]), float.Parse(messageAttrs[1]));
        Vector2 inputSceneCoordinate = CoordinatesConverter.ConvertCoordinate(inputGpsCoordinate);

        if (messageAttrs[2].Equals("trafficAccident"))
        {
            ResizeArray(ref ObjectsPlacer.instantietedCrash, ObjectsPlacer.instantietedCrash.Length + 1);
            ObjectsPlacer.sceneCrashCoordinates.Add(inputSceneCoordinate);
        }
        if (messageAttrs[2].Equals("police"))
        {
            ResizeArray(ref ObjectsPlacer.instantietedPolice, ObjectsPlacer.instantietedPolice.Length + 1);
            ObjectsPlacer.scenePoliceCoordinates.Add(inputSceneCoordinate);
        }
        if (messageAttrs[2].Equals("message"))
        {
            ResizeArray(ref ObjectsPlacer.instantietedMessage, ObjectsPlacer.instantietedMessage.Length + 1);
            ObjectsPlacer.sceneMessageCoordinates.Add(inputSceneCoordinate);
            Debug.Log(messageAttrs[3]);
            ObjectsPlacer.Messages.Add(messageAttrs[3]);
        }

    }

    public static void ResizeArray(ref GameObject[] array, int size)
    {
        GameObject[] temp = new GameObject[size];
        for (int i = 0; i < array.Length; i++)
            temp[i] = array[i];
        array = temp;
    }
}
