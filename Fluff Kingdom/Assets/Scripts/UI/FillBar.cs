using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillBar : MonoBehaviour {
	[SerializeField] Image fillImage;

	public Image FillImage {
		get {
			return this.fillImage;
		}
	}
}
