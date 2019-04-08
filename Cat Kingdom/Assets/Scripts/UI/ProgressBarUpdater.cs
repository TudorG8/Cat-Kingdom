using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBarUpdater : MonoBehaviour {
	[SerializeField] FillBar progressBar;

	public FillBar ProgressBar {
		get {
			return this.progressBar;
		}
		set {
			progressBar = value;
		}
	}

	public void UpdateToValue(float value) {
		progressBar.FillImage.fillAmount = Mathf.Clamp01 (value);
	}
}
