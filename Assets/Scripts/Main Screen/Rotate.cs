using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {
	[SerializeField] float rotation;

	void Update() {
		transform.RotateAround(Vector3.zero, Vector3.up, rotation * Time.deltaTime);
	}
}
