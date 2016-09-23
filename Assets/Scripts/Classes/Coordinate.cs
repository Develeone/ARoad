using UnityEngine;

public class Coordinate {
	private float _latitude;
	private float _longitude;

	public Coordinate(float __latitude = 0, float __longitude = 0) {
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
}