using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIResourceManager : Singleton<UIResourceManager> {
	[SerializeField] Text woodText;

	[SerializeField] int wood;

	public int Wood {
		get {
			return this.wood;
		}
		set {
			wood = value;
		}
	}

	void Awake() {
		InitiateSingleton ();
	}

	void Update() {
		woodText.text = wood.ToString();
	}
}
