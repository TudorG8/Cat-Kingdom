using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextUpdater : MonoBehaviour {
	[SerializeField] Text text;
	[SerializeField] TextEmitter textEmitter;

	public TextEmitter TextEmitter {
		get {
			return this.textEmitter;
		}
		set {
			textEmitter = value;
		}
	}

	public Text Text {
		get {
			return this.text;
		}
		set {
			text = value;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (textEmitter != null) {
			text.text = textEmitter.Val;
		}
	}
}
