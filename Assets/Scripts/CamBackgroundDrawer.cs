using UnityEngine;
using System.Collections;

public class CamBackgroundDrawer : MonoBehaviour {

	public Camera mainCamera;
	public Camera backgroundCamera;
	public GameObject backgroundCameraBackground;

	WebCamTexture deviceCameraTexture;

	void Start () {
		deviceCameraTexture = new WebCamTexture();
		deviceCameraTexture.requestedWidth = Screen.width/10;
		deviceCameraTexture.requestedHeight = Screen.height/10;
		Renderer camBgRenderer = backgroundCameraBackground.GetComponent<Renderer>();
		camBgRenderer.material.mainTexture = deviceCameraTexture;
		deviceCameraTexture.Play();

		float pos = (backgroundCamera.nearClipPlane + 0.01f);
		backgroundCameraBackground.transform.position = backgroundCamera.transform.position + backgroundCamera.transform.forward * pos;
		float h = Mathf.Tan(backgroundCamera.fieldOfView * Mathf.Deg2Rad * 0.5f) * pos * 2f;
		backgroundCameraBackground.transform.localScale = new Vector3(h * backgroundCamera.aspect, h, 0f);
	}

	void Update()
	{
		
	}

}