using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedUnitPanel : MonoBehaviour {
	[SerializeField] Text name;
	[SerializeField] Image image;

	public Text Name {
		get {
			return this.name;
		}
	}

	public Image Image {
		get {
			return this.image;
		}
	}
}
