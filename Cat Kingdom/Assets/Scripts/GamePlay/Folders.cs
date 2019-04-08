using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Folders : Singleton<Folders> {
	[SerializeField] Transform effects;

	public Transform Effects {
		get {
			return this.effects;
		}
		set {
			effects = value;
		}
	}

	void Awake() {
		InitiateSingleton ();
	}
}
