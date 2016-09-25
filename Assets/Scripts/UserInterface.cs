using UnityEngine;
using System.Collections;

public class UserInterface : MonoBehaviour {

	public GUISkin FlatGUISkin;

	public Texture splashScreenTex;
	public Texture settingsButtonTex;
	public Texture addCrashTex;
	public Texture addPoliceTex;
	public Texture addMessageTex;

	public string[] gasolineTypes;

	float splashScreenVerticalPosition = 0;
	bool showSettings = false;
	Rect settingsRect = new Rect(50, 50, Screen.width-100, Screen.height-100);

	bool showMessageTextInput = false;

	bool showCams = false;
	bool showGasStations = false;
	bool showParkings = false;
	bool showPitStops = false;
	bool showTires = false;
	bool showCrashes = false;
	bool showPolice = false;
	bool showMessages = false;

	string messageTextToSend = "";

	int gasolineType = 0;

	void Start () {
		showCams 		= PlayerPrefs.GetInt ("showCam") == 1 ? true : false;
		showGasStations = PlayerPrefs.GetInt ("showGasStation") == 1 ? true : false;
		showParkings 	= PlayerPrefs.GetInt ("showParking") == 1 ? true : false;
		showPitStops 	= PlayerPrefs.GetInt ("showPitStop") == 1 ? true : false;
		showTires 		= PlayerPrefs.GetInt ("showTire") == 1 ? true : false;
		showCrashes 	= PlayerPrefs.GetInt ("showCrash") == 1 ? true : false;
		showPolice 		= PlayerPrefs.GetInt ("showPolice") == 1 ? true : false;
		showMessages 	= PlayerPrefs.GetInt ("showMessage") == 1 ? true : false;
		gasolineType 	= PlayerPrefs.GetInt ("gasolineType");
	}

	void Update () {

		if (GpsTracking.GpsReady) {
			if (splashScreenVerticalPosition > -(Screen.height + 1)) {
				splashScreenVerticalPosition -= 4;
			}
		}

		if (showCams != (PlayerPrefs.GetInt ("showCam") == 1 ? true : false)) {
			PlayerPrefs.SetInt ("showCam", showCams ? 1 : 0);
			ObjectsPlacer.settingsChanged = true;
		}
		if (showGasStations != (PlayerPrefs.GetInt ("showGasStation") == 1 ? true : false)) {
			PlayerPrefs.SetInt ("showGasStation", showGasStations ? 1 : 0);
			ObjectsPlacer.settingsChanged = true;
		}
		if (showParkings != (PlayerPrefs.GetInt ("showParking") == 1 ? true : false)) {
			PlayerPrefs.SetInt ("showParking", showParkings ? 1 : 0);
			ObjectsPlacer.settingsChanged = true;
		}
		if (showPitStops != (PlayerPrefs.GetInt ("showPitStop") == 1 ? true : false)) {
			PlayerPrefs.SetInt ("showPitStop", showPitStops ? 1 : 0);
			ObjectsPlacer.settingsChanged = true;
		}
		if (showTires != (PlayerPrefs.GetInt ("showTire") == 1 ? true : false)) {
			PlayerPrefs.SetInt ("showTire", showTires ? 1 : 0);
			ObjectsPlacer.settingsChanged = true;
		}
		if (showCrashes != (PlayerPrefs.GetInt ("showCrash") == 1 ? true : false)) {
			PlayerPrefs.SetInt ("showCrash", showCrashes ? 1 : 0);
			ObjectsPlacer.settingsChanged = true;
		}
		if (showPolice != (PlayerPrefs.GetInt ("showPolice") == 1 ? true : false)) {
			PlayerPrefs.SetInt ("showPolice", showPolice ? 1 : 0);
			ObjectsPlacer.settingsChanged = true;
		}
		if (showMessages != (PlayerPrefs.GetInt ("showMessage") == 1 ? true : false)) {
			PlayerPrefs.SetInt ("showMessage", showMessages ? 1 : 0);
			ObjectsPlacer.settingsChanged = true;
		}
		if (gasolineType != PlayerPrefs.GetInt ("gasolineType")) {
			PlayerPrefs.SetInt ("gasolineType", gasolineType);
			ObjectsPlacer.settingsChanged = true;
		}
	}

	void OnGUI () {
		GUI.skin = FlatGUISkin;

		if (GUI.Button (new Rect (0, 0, Screen.width/7f, Screen.width/7f), settingsButtonTex)) {
			showSettings = !showSettings;
		}

		if (GUI.Button (new Rect (Screen.width-Screen.width/7f, Screen.height-Screen.width/7f, Screen.width/7f, Screen.width/7f), addCrashTex)) {
			SettingsController.AddTraficAccident ();
		}

		if (GUI.Button (new Rect (Screen.width-(Screen.width/7f*2f), Screen.height-Screen.width/7f, Screen.width/7f, Screen.width/7f), addPoliceTex)) {
			SettingsController.AddPolice ();
		}

		if (GUI.Button (new Rect (Screen.width-(Screen.width/7f*3f), Screen.height-Screen.width/7f, Screen.width/7f, Screen.width/7f), addMessageTex)) {
			showMessageTextInput = true;
		}

		if (showSettings) {
			settingsRect = GUI.Window (0, settingsRect, SettingsContent, "Настройки");
		}

		if (showMessageTextInput) {
			messageTextToSend = GUI.TextField (new Rect(Screen.width*0.1f, Screen.height*0.3f, Screen.width*0.8f, 40), messageTextToSend);

			if (GUI.Button (new Rect(Screen.width*0.1f, Screen.height*0.6f, Screen.width/2f, 50), "Отправить")) {
				SettingsController.AddMessage (messageTextToSend);
				showMessageTextInput = false;
				messageTextToSend = "";
			}

			if (GUI.Button (new Rect(Screen.width - (Screen.width*0.1f + Screen.width/2f), Screen.height*0.6f, Screen.width/2f, 50), "Отмена")) {
				showMessageTextInput = false;
				messageTextToSend = "";
			}

		}

		if (splashScreenVerticalPosition > -Screen.height)
			GUI.DrawTexture (new Rect(0, splashScreenVerticalPosition, Screen.width, Screen.height), splashScreenTex);
	}

	void SettingsContent (int windowId) {
		GUILayout.Label ("Топливо:");
		gasolineType 	= GUILayout.SelectionGrid (gasolineType, gasolineTypes, 4);

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
