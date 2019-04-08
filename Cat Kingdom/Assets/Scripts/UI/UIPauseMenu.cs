using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPauseMenu : MonoBehaviour {
	[SerializeField] Transform pausePanel;
	public void PauseGame() {
		pausePanel.gameObject.SetActive (true);
		Time.timeScale = 0f;
	}

	public void ResumeGame() {
		pausePanel.gameObject.SetActive (false);
		Time.timeScale = 1f;
	}
}
