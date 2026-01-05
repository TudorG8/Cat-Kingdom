using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHighscore : MonoBehaviour {
	[SerializeField] Text name;
	[SerializeField] Text score;

	public Text Name {
		get {
			return this.name;
		}
		set {
			name = value;
		}
	}

	public Text Score {
		get {
			return this.score;
		}
		set {
			score = value;
		}
	}
}
