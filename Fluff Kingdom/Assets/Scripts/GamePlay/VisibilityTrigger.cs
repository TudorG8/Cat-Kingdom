using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VisibilityTrigger : MonoBehaviour {
	[SerializeField] UnityEvent onBecomeVisible;
	[SerializeField] UnityEvent onBecomeInvisible;

	void OnBecameVisible() {
		onBecomeVisible.Invoke ();
	}

	void OnBecameInvisible() {
		onBecomeInvisible.Invoke ();
	}
}
