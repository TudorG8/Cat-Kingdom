using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameChecker : MonoBehaviour {
	[SerializeField] UIGameOverScreen gameOverScreen;
	[SerializeField] UITimer uiTimer;
	[SerializeField] bool over;
	[SerializeField] int maxMinutesForScore = 20;

	void Awake() {
		Time.timeScale = 1;
	}

	void Update() {
		if (over) {
			return;
		}

		CheckForDefeat ();
		CheckForVictory ();
	}

	void CheckForDefeat() {
		Building castle = BuildingManager.Instance.Castle;

		if (castle.HitPoints <= 0) {
			uiTimer.Stopped = true;
			PhaseSwitcher.Instance.Stopped = true;

			int score = UIResourceManager.Instance.Fluff * 10;

			string gatheredFluff = (UIResourceManager.Instance.Fluff * 10).ToString();
			gameOverScreen.gameObject.SetActive (true);
			gameOverScreen.Score.text = gatheredFluff;
			gameOverScreen.DefeatTitle .gameObject.SetActive (true);
			gameOverScreen.VictoryTitle.gameObject.SetActive (false);
			gameOverScreen.FluffScore.text = gatheredFluff;
			gameOverScreen.TimeScore.text = "0";
			gameOverScreen.Score.text = gatheredFluff;

			StopUnits ();
			RecordKeeper.Instance.AddNewRecord (SessionData.Instance.Name, score);
			RecordKeeper.Instance.Save ();

			over = true;
			Time.timeScale = 0;
		}


	}

	void StopUnits() {
		for (int i = 0; i < UnitSelection.Instance.SelectedObjects.Count; i++) {
			SelectableObject worker = UnitSelection.Instance.SelectedObjects [i];
			worker.StopCurrentAction ();
		}
	}

	void CheckForVictory() {
		List<Resource> remainingFluff = ResourceManager.Instance.GetAllOfType (ResourceType.Fluff);
		if (remainingFluff == null || remainingFluff.Count == 0) {
			uiTimer.Stopped = true;
			PhaseSwitcher.Instance.Stopped = true;


			int elapsedTime = (int)(uiTimer.ElapsedTime % 60);
			int timeScore = Mathf.Max ((maxMinutesForScore * 60) - elapsedTime, 0, int.MinValue);

			int score = UIResourceManager.Instance.Fluff * 10 + timeScore;

			if (SessionData.Instance.HardMode) {
				score = (int)(score * 1.5f);
			}

			int gatheredFluff = UIResourceManager.Instance.Fluff * 10;
			gameOverScreen.gameObject.SetActive (true);
			gameOverScreen.Score.text = gatheredFluff.ToString();
			gameOverScreen.DefeatTitle .gameObject.SetActive (false);
			gameOverScreen.VictoryTitle.gameObject.SetActive (true);
			gameOverScreen.FluffScore.text = gatheredFluff.ToString();
			gameOverScreen.TimeScore.text = timeScore.ToString();
			gameOverScreen.Score.text = (gatheredFluff + timeScore).ToString();

			StopUnits ();
			RecordKeeper.Instance.AddNewRecord (SessionData.Instance.Name, score);
			RecordKeeper.Instance.Save ();

			over = true;
			Time.timeScale = 0;
		}
	}
}
