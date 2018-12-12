using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeathChecker : MonoBehaviour {
	[SerializeField] float val;
	[SerializeField] float threshold;
	[SerializeField] UnityEvent onDeath;

	public float Val {
		get {
			return this.val;
		}
		set {
			val = value;
		}
	}

	void Update () {
		if (val < threshold) {
			onDeath.Invoke ();
		}
	}

	public void Destroy(float delay) {
		Destroy (gameObject);
	}
}
