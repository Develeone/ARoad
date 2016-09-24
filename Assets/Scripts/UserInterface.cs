﻿using UnityEngine;
using System.Collections;

public class UserInterface : MonoBehaviour {

	public GUISkin FlatGUISkin;

	public Texture splashScreen;
	public Texture settingsButton;

	float splashScreenVerticalPosition = 0;
	bool showSettings = false;
	Rect settingsRect = new Rect(50, 50, Screen.width-100, Screen.height-100);

	bool showCams = false;
	bool showGasStations = false;
	bool showParkings = false;
	bool showPitStops = false;
	bool showTires = false;
	bool showCrashes = false;
	bool showPolice = false;
	bool showMessages = false;

	IEnumerator Start () {
		showCams 		= PlayerPrefs.GetInt ("showCam") == 1 ? true : false;
		showGasStations = PlayerPrefs.GetInt ("showGasStation") == 1 ? true : false;
		showParkings 	= PlayerPrefs.GetInt ("showParking") == 1 ? true : false;
		showPitStops 	= PlayerPrefs.GetInt ("showPitStop") == 1 ? true : false;
		showTires 		= PlayerPrefs.GetInt ("showTire") == 1 ? true : false;
		showCrashes 	= PlayerPrefs.GetInt ("showCrash") == 1 ? true : false;
		showPolice 		= PlayerPrefs.GetInt ("showPolice") == 1 ? true : false;
		showMessages 	= PlayerPrefs.GetInt ("showMessage") == 1 ? true : false;

		while (!GpsTracking.GpsReady) {
			yield return new WaitForSeconds (1);
		}

		while (splashScreenVerticalPosition > -(Screen.height+1)) {
			splashScreenVerticalPosition--;
			yield return new WaitForSeconds (0.001f);
		}

	}

	void Update () {
		if (showCams 			!= (PlayerPrefs.GetInt ("showCam") == 1 ? true : false))
			PlayerPrefs.SetInt ("showCam", showCams ? 1 : 0);
		if (showGasStations 	!= (PlayerPrefs.GetInt ("showGasStation") == 1 ? true : false))
			PlayerPrefs.SetInt ("showGasStation", showGasStations ? 1 : 0);
		if (showParkings 		!= (PlayerPrefs.GetInt ("showParking") == 1 ? true : false))
			PlayerPrefs.SetInt ("showParking", showParkings ? 1 : 0);
		if (showPitStops 		!= (PlayerPrefs.GetInt ("showPitStop") == 1 ? true : false))
			PlayerPrefs.SetInt ("showPitStop", showPitStops ? 1 : 0);
		if (showTires			!= (PlayerPrefs.GetInt ("showTire") == 1 ? true : false))
			PlayerPrefs.SetInt ("showTire", showTires ? 1 : 0);
		if (showCrashes 		!= (PlayerPrefs.GetInt ("showCrash") == 1 ? true : false))
			PlayerPrefs.SetInt ("showCrash", showCrashes ? 1 : 0);
		if (showPolice 			!= (PlayerPrefs.GetInt ("showPolice") == 1 ? true : false))
			PlayerPrefs.SetInt ("showPolice", showPolice ? 1 : 0);
		if (showMessages 		!= (PlayerPrefs.GetInt ("showMessage") == 1 ? true : false))
			PlayerPrefs.SetInt ("showMessage", showMessages ? 1 : 0);
	}

	void OnGUI () {
		GUI.skin = FlatGUISkin;

		if (GUI.Button (new Rect (0,0,50,50), settingsButton)) {
			showSettings = !showSettings;
		}

		if (showSettings) {
			settingsRect = GUI.Window (0, settingsRect, SettingsContent, "Настройки");
		}

		if (splashScreenVerticalPosition > -Screen.height)
			GUI.DrawTexture (new Rect(0, splashScreenVerticalPosition, Screen.width, Screen.height), splashScreen);
	}

	void SettingsContent (int windowId) {
		showCams 		= GUILayout.Toggle (showCams, "Камеры");
		showGasStations = GUILayout.Toggle (showGasStations, "Заправки");
		showParkings 	= GUILayout.Toggle (showParkings, "Парковки");
		showPitStops 	= GUILayout.Toggle (showPitStops, "Питстопы");
		showTires 		= GUILayout.Toggle (showTires, "Шиномонтаж");
		showCrashes 	= GUILayout.Toggle (showCrashes, "ДТП");
		showPolice 		= GUILayout.Toggle (showPolice, "Кордоны");
		showMessages 	= GUILayout.Toggle (showMessages, "Сообщения");
	}
}