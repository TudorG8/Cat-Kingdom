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

	public void FadeTo  () {
		StartCoroutine(FadeRoutine(fromColour, toColour));
	}

	public void FadeFrom() {
		StartCoroutine(FadeRoutine(toColour, fromColour));
	}

	IEnumerator FadeRoutine(Color initial, Color target) {
		Color current = initial;
		float timePassed = 0f;
		while(timePassed < duration) {
			image.color = Color.Lerp (initial, target, timePassed / duration);
			timePassed += Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}
	}

	void Awake() {
		if (playOnAwake) {
			FadeTo ();
		}
	}
}