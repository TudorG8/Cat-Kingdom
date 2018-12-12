using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotation : MonoBehaviour {
	[SerializeField] Quaternion rotationSnapshot;
	// Use this for initialization
	void Start () {
		rotationSnapshot = transform.rotation;
	}

	// Update is called once per frame
	void Update () {
		transform.rotation = rotationSnapshot;
	}
}
