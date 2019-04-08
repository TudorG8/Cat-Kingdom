using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionData : Singleton<SessionData> {
	[SerializeField] string name = "No Name";
	[SerializeField] bool hardMode;

	public bool HardMode {
		get {
			return this.hardMode;
		}
		set {
			hardMode = value;
		}
	}

	public string Name {
		get {
			return this.name;
		}
		set {
			name = value;
		}
	}

	void Awake() {
		DontDestroyOnLoad (this);

		InitiateSingleton ();
	}
}
