using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour {
	[SerializeField] Transform obj;

	[SerializeField] bool x;
	[SerializeField] bool y;
	[SerializeField] bool z;

	void Update() {
		Vector3 targetPosition = obj.position;

		if (!x) {targetPosition.x = transform.position.x;}
		if (!y) {targetPosition.y = transform.position.y;}
		if (!z) {targetPosition.z = transform.position.z;}

		transform.position = targetPosition;
	}
}
