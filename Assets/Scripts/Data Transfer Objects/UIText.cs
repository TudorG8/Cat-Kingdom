using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIText : MonoBehaviour {
	[SerializeField] Text text;

	public Text Text {
		get {
			return this.text;
		}
		set {
			text = value;
		}
	}
}
