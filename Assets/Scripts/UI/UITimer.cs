using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITimer : MonoBehaviour {
	[SerializeField] bool shouldCountDown = true;
	[SerializeField] Text countdownText;
	[SerializeField] float elapsedTime = 0f;
	[SerializeField] bool stopped;

	public bool Stopped {
		get {
			return this.stopped;
		}
		set {
			stopped = value;
		}
	}

	public float ElapsedTime {
		get {
			return this.elapsedTime;
		}
		set {
			elapsedTime = value;
		}
	}

	// Update is called once per frame
	void Update () {
		if (stopped)
			return;

		if (shouldCountDown) {
			elapsedTime += Time.deltaTime;
		}

		int minutes = (int)(elapsedTime / 60);
		int seconds = (int)(elapsedTime % 60);

		countdownText.text = minutes.ToString ("D2") + ":" + seconds.ToString ("D2");
	}
}
