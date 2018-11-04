using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	[SerializeField] float scrollableSize;
	[SerializeField] float scrollSpeed;


	void Update () {
		Vector3 position = transform.position;

		if (Input.mousePosition.y > Screen.height - scrollableSize) {
			position.z += scrollSpeed * Time.deltaTime;
		}

		if (Input.mousePosition.y < scrollableSize) {
			position.z -= scrollSpeed * Time.deltaTime;
		}

		if (Input.mousePosition.x > Screen.width - scrollableSize) {
			position.x += scrollSpeed * Time.deltaTime;
		}

		if (Input.mousePosition.x < scrollableSize) {
			position.x -= scrollSpeed * Time.deltaTime;
		}

		transform.position = position;
	}
}
