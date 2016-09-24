using UnityEngine;
using System.Collections;

public class RotateToCharacter : MonoBehaviour {

	Transform characterTransform;
    public TextMesh distance;

	void Start () {
		characterTransform = Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt (characterTransform);

		float distanceToPlayer = Vector3.Distance(transform.position, characterTransform.position);

        distance.text = ((int)distanceToPlayer).ToString();

        transform.localScale = new Vector3 (distanceToPlayer/54f+2f, distanceToPlayer/36f+3f, distanceToPlayer/36f+3f);
		transform.position = new Vector3 (transform.position.x, distanceToPlayer/5f, transform.position.z);
	}
}