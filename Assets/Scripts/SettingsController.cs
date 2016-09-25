using UnityEngine;
using System.Collections;

public static class SettingsController {

    public static bool AddTraficAccident()
    {
        NetworkManager.writeSocket(GpsTracking.currentCoordinate.ToString() + ";trafficAccident");
        
        if (!NetworkManager.socketReady)
            ServerResponseHandler.HandleServerResponse(GpsTracking.currentCoordinate.ToString() + ";trafficAccident");
        return true;
    }

    public static bool AddPolice()
    {
        NetworkManager.writeSocket(GpsTracking.currentCoordinate.ToString() + ";police");

        if (!NetworkManager.socketReady)
            ServerResponseHandler.HandleServerResponse(GpsTracking.currentCoordinate.ToString() + ";police");
        return true;
    }

    public static bool AddMessage(string messageText)
    {
        NetworkManager.writeSocket(GpsTracking.currentCoordinate.ToString() + ";message;" + messageText);
        if (!NetworkManager.socketReady)
            ServerResponseHandler.HandleServerResponse(GpsTracking.currentCoordinate.ToString() + ";message;" + messageText);
        return true;
    }
}
