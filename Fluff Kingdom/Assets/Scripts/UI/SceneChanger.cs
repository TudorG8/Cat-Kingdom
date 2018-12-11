using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {
	[SerializeField] UIFade uiFade;

	public void LoadLevelOne () {
		uiFade.FadeTo ();
		StartCoroutine(LoadLevelAfterDelay ("GrassyHills", 1f));
	}

	public void LoadLevelTwo () {
		uiFade.FadeTo ();
		StartCoroutine(LoadLevelAfterDelay ("DarkTimes", 1f));
	}

	public void LoadMainMenu () {
		Debug.Log ("uh");
		uiFade.FadeTo ();
		StartCoroutine(LoadLevelAfterDelay ("MainMenu", 1f));
	}

	IEnumerator LoadLevelAfterDelay(string scene, float delay) {
		yield return new WaitForSeconds (delay);
		SceneManager.LoadScene (scene);
	}

	void Awake() {
		DontDestroyOnLoad (this);

		if (FindObjectsOfType<SceneChanger> ().Length > 1) {
			Destroy (gameObject);
		}
	}
}
