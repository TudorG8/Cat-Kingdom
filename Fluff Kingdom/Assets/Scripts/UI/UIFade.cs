using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFade : MonoBehaviour {
	[SerializeField] Color fromColour;
	[SerializeField] Color toColour  ;

	[SerializeField] Image image;

	[SerializeField] float duration = 1;

	[SerializeField] bool playOnAwake;

	[SerializeField] float timePassed;

	public void FadeTo  () {
		StartCoroutine(FadeRoutine(fromColour, toColour));
	}

	public void FadeFrom() {
		StartCoroutine(FadeRoutine(toColour, fromColour));
	}

	IEnumerator FadeRoutine(Color initial, Color target) {
		Color current = initial;
		timePassed = 0f;
		while(timePassed < duration) {
			image.color = Color.Lerp (initial, target, timePassed / duration);
			timePassed += Time.unscaledDeltaTime;
			yield return new WaitForEndOfFrame ();
		}
	}

	void Start() {
		if (image == null) {
			image = UISelectionController.Instance.Image;
		}
		if (playOnAwake) {
			FadeFrom ();
		}
	}
}