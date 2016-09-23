using UnityEngine;

namespace OMobile
{
	public class GyroCam : MonoBehaviour
	{

		#region Private Variables

		private bool gyroBool;
		private Gyroscope gyro;
		private Quaternion rotFix;
		private Transform mGyroCamParentTrans;
		private Transform mGyroCamGrandParentTrans;

		#endregion

		#region Unity Methods

		void Start ()
		{
			Transform currentParent = transform.parent;

			GameObject camParent = new GameObject ("GyroCamParent");
			mGyroCamParentTrans = camParent.transform;
			mGyroCamParentTrans.position = transform.position;
			transform.parent = mGyroCamParentTrans;

			GameObject camGrandparent = new GameObject ("GyroCamGrandParent");
			mGyroCamGrandParentTrans = camGrandparent.transform;
			mGyroCamGrandParentTrans.position = transform.position;
			mGyroCamParentTrans.parent = mGyroCamGrandParentTrans;
			mGyroCamGrandParentTrans.parent = currentParent;

			#if UNITY_3_4
			gyroBool = Input.isGyroAvailable;
			#else
			gyroBool = SystemInfo.supportsGyroscope;
			#endif
			gyroBool = true;

			if (gyroBool) {
				gyro = Input.gyro;
				gyro.enabled = true;
				Input.compass.enabled = true;

				SetupRotations ();
			} else {
				#if UNITY_EDITOR
				print ("NO GYRO");
				#endif
			}
		}

		void Update ()
		{
			if (gyroBool == gameObject.activeInHierarchy) {
				Quaternion quatMap = Quaternion.identity;
				#if UNITY_IPHONE
				quatMap = gyro.attitude;
				#elif UNITY_ANDROID
				quatMap = new Quaternion(gyro.attitude.x,gyro.attitude.y,gyro.attitude.z,gyro.attitude.w);
				#endif

				UpdateCompassRotation ();

				transform.localRotation = quatMap * rotFix;
				transform.eulerAngles = new Vector3 (transform.eulerAngles.x, transform.eulerAngles.y/2f, transform.eulerAngles.z);
			}
		}

		#endregion

		#region Public Methods

		public void ResetRotations ()
		{
			mGyroCamParentTrans.localRotation = Quaternion.identity;
			mGyroCamGrandParentTrans.localRotation = Quaternion.identity;
			transform.localRotation = Quaternion.identity;
		}

		public void SetupRotations ()
		{
			if (!gyroBool) {
				return;
			}

			mGyroCamParentTrans.transform.eulerAngles = new Vector3 (90, 180, 0);

			rotFix = new Quaternion (0, 0, 1, 0);

			//mGyroCamGrandParentTrans.localRotation = Quaternion.Euler (new Vector3 (0, Input.compass.magneticHeading, 0));
		}

		#endregion

		#region Private Methods

		private void UpdateCompassRotation ()
		{
			Vector3 rot = mGyroCamGrandParentTrans.localEulerAngles;
			rot.y = Input.compass.magneticHeading;
			mGyroCamGrandParentTrans.localRotation = Quaternion.Lerp (mGyroCamGrandParentTrans.localRotation, Quaternion.Euler (rot), Time.deltaTime * 8f);
		}

		#endregion

		void OnGUI () {
			GUILayout.Label(transform.eulerAngles.x + " " + transform.eulerAngles.y + " " + transform.eulerAngles.z);
			GUILayout.Label(mGyroCamParentTrans.eulerAngles.x + " " + mGyroCamParentTrans.eulerAngles.y + " " + mGyroCamParentTrans.eulerAngles.z);
			GUILayout.Label(mGyroCamGrandParentTrans.eulerAngles.x + " " + mGyroCamGrandParentTrans.eulerAngles.y + " " + mGyroCamGrandParentTrans.eulerAngles.z);
		}
	}
}