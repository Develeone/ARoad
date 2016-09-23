using UnityEngine;

public class Coordinates {
	private float _latitude;
	private float _longitude;

	public Coordinates(float __latitude = 0, float __longitude = 0) {
		_latitude = __latitude;
		_longitude = __longitude;
	}

	public float latitude {
		get { return _latitude; }
		set { _latitude = value; }
	}

	public float longitude {
		get { return _longitude; }
		set { _longitude = value; }
	}

	// Расстояние меж текущими и новыми координатами
	public float distanceToNewCoordinates(Coordinates newCoordinates)
	{
		float theta = _longitude - newCoordinates.longitude; // Сколько мы прошли по долготе в ебучих морских милях
		float dist = 	Mathf.Sin(Mathf.Deg2Rad * _latitude) * // Получаем синус широты в градусах
						Mathf.Sin(Mathf.Deg2Rad * newCoordinates.latitude) + // И умножаем на синус второй широты в градусах
						Mathf.Cos(Mathf.Deg2Rad * _latitude) * 
						Mathf.Cos(Mathf.Deg2Rad * newCoordinates.latitude) * 
						Mathf.Cos(Mathf.Deg2Rad * theta);

		dist = Mathf.Acos(dist);
		dist = Mathf.Rad2Deg * dist;
		float miles = dist * 60f * 1.1515f;
		return (miles * 1.609344f);
	}

}