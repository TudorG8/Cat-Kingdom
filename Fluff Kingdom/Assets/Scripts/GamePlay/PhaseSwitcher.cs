using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseSwitcher : MonoBehaviour {
	[SerializeField] GameObject battlePhaseStatus;
	[SerializeField] GameObject buildPhaseStatus;
	[SerializeField] UIProgressBar battleProgressBar;
	[SerializeField] UIText buildTimer;
	[SerializeField] GamePhase currentPhase;
	[SerializeField] int buildTime;

	[SerializeField] float timeLeft;

	void Start() {
		ChangeToBuildPhase (false);
	}

	void ChangeToBuildPhase(bool changeUnits) {
		currentPhase = GamePhase.Build;
		timeLeft = buildTime;
		buildTimer       .gameObject.SetActive (true);
		buildPhaseStatus .SetActive (true);
		battleProgressBar.gameObject.SetActive (false);
		battlePhaseStatus.SetActive (false);
		if(changeUnits)
			CommandUnitsToSwitch (false);
	}

	void ChangeToBattlePhase(bool changeUnits) {
		currentPhase = GamePhase.Battle;
		buildTimer       .gameObject.SetActive (false);
		buildPhaseStatus .SetActive (false);
		battleProgressBar.gameObject.SetActive (true);
		battlePhaseStatus.SetActive (true);
		if(changeUnits)
			CommandUnitsToSwitch (true);
	}

	void Update() {
		if (currentPhase == GamePhase.Build) {
			timeLeft = Mathf.Clamp (timeLeft - Time.deltaTime, 0, float.MaxValue);
			int minutes = (int)(timeLeft / 60);
			int seconds = (int)(timeLeft % 60);

			buildTimer.Text.text = minutes.ToString ("D2") + ":" + seconds.ToString ("D2");

			if (Mathf.Approximately (timeLeft, 0)) {
				
				ChangeToBattlePhase (true);
			}
		} 
		else if (currentPhase == GamePhase.Battle) {
			int enemyCount = EnemySpawner.Instance.CurrentEnemyCount;

			battleProgressBar.Image.fillAmount = Mathf.Clamp01 ((float)enemyCount / EnemySpawner.Instance.EnemiesThisWave);

			if (enemyCount == 0) {
				ChangeToBuildPhase (true);
			}
		}
	}

	void CommandUnitsToSwitch(bool equipItems) {
		for (int i = 0; i < UnitSelection.Instance.Workers.Count; i++) {
			SelectableObject worker = UnitSelection.Instance.Workers [i];
			worker.Deselect ();
			worker.CanBeSelected = false;

			Building closestCastle = BuildingManager.Instance.GetClosestBuilding(BuildingType.Castle).GetComponent<Building>();

			Indicator indicator = closestCastle.SpotGenerator.GetClosestSpot (worker.transform.position);

			UnitMovement unitMovement = worker.GetComponent<UnitMovement> ();

			if (unitMovement != null) {
				worker.StopCurrentAction ();
				indicator.Connect (worker);
				worker.Indicator = indicator;
				unitMovement.MoveTowards (indicator.transform.position, (gameObject) => { 
					Vector3 lookPosition = closestCastle.transform.position;
					lookPosition.y = gameObject.transform.position.y;
					gameObject.transform.LookAt(lookPosition);

					worker.CanBeSelected = true;
					worker.UnitStats.EquipItems(equipItems);
				});
			}
		}
	}
}
