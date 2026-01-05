using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour {
	[SerializeField] Animator playBox;
	[SerializeField] Animator scoreBox;
	[SerializeField] Animator optionBox;

	[SerializeField] Animator currentBox;

	[SerializeField] bool canClick = true;
	[SerializeField] bool midAnimation = false;

	public bool CanClick {
		get {
			return this.canClick;
		}
		set {
			canClick = value;
		}
	}

	public void QuitGame() {
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}

	void LoadBox(Animator box) {
		if (!canClick || midAnimation)
			return;

		// Same screen, lift it up
		if (currentBox == box) {
			box.SetTrigger ("lift");
			currentBox = null;
		} 
		else {
			// Have to lift up the old one first.
			if (currentBox != null) {
				currentBox.SetTrigger ("lift");
				currentBox = box;
				StartCoroutine (DropAfterDelay (0.3f));
			} 
			else {
				box.SetTrigger ("drop");
				currentBox = box;
			}
		}

		canClick = false;
	}

	IEnumerator DropAfterDelay(float delay) {
		midAnimation = true;
		yield return new WaitForSeconds (delay);
		currentBox.SetTrigger ("drop");
		yield return new WaitForSeconds (delay);
		midAnimation = false;
	}

	public void LoadPlayBox() {
		LoadBox (playBox);
	}

	public void LoadScoreBox() {
		LoadBox (scoreBox);
	}

	public void LoadOptionsBox() {
		LoadBox (optionBox);
	}

	public void HardModeToggleChange(bool val) {
		SessionData.Instance.HardMode = val;
	}
}