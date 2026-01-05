using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIProgressBar : MonoBehaviour {
	[SerializeField] Image image;

	public Image Image {
		get {
			return this.image;
		}
		set {
			image = value;
		}
	}
}
