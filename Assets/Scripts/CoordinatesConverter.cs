using UnityEngine;
using System.Collections;

public static class CoordinatesConverter {
    
    //Коэффициент для широты
    private const int latCoef = 111000;
    //Коэффициент для долготы
    private const int lngCoef = 81200;

    public static Vector2 ConvertCoordinate(Coordinate gpsCoordinate)
    {
        Coordinate startCoordinate = GpsTracking.startCoordinate;

        float lat_diff = startCoordinate.latitude - gpsCoordinate.latitude;
        float lon_diff = startCoordinate.longitude - gpsCoordinate.longitude;

		Debug.LogAssertion (startCoordinate.latitude + " " + startCoordinate.longitude + " " + lat_diff + " " + lon_diff);

        return new Vector2(lat_diff * latCoef, lon_diff * lngCoef);
    }
}
