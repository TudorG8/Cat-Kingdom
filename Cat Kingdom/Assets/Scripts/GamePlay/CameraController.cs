using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	[SerializeField] float scrollableSize;
	[SerializeField] float scrollSpeed;
	[SerializeField] float mouseWheelSpeed;
	[SerializeField] int bounds;
	[SerializeField] int minY, maxY;

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

		float mouseWheelInput = Input.GetAxis ("Mouse ScrollWheel");

		position.x = Mathf.Clamp (position.x, -bounds, bounds);
		position.y -= mouseWheelInput * mouseWheelSpeed * Time.deltaTime;
		position.y = Mathf.Clamp (position.y, minY, maxY);
		position.z = Mathf.Clamp (position.z, -bounds, bounds);

		transform.position = position;
	}
}
