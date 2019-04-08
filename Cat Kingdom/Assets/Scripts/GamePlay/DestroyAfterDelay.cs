using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterDelay : MonoBehaviour {
	[SerializeField] float delay;

	void Awake() {
		Destroy (gameObject, delay);
	}
}
