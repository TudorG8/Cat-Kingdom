using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseSwitcher : Singleton<PhaseSwitcher> {
	[SerializeField] GameObject battlePhaseStatus;
	[SerializeField] GameObject buildPhaseStatus;
	[SerializeField] UIProgressBar battleProgressBar;
	[SerializeField] UIText buildTimer;
	[SerializeField] GamePhase currentPhase;
	[SerializeField] int buildTime;
	[SerializeField] ProgressBarUpdater progressBarUpdater;

	[SerializeField] float timeLeft;
	[SerializeField] bool stopped;

	public bool Stopped {
		get {
			return this.stopped;
		}
		set {
			stopped = value;
		}
	}

	public GamePhase CurrentPhase {
		get {
			return this.currentPhase;
		}
		set {
			currentPhase = value;
		}
	}

	void Awake() {
		InitiateSingleton ();
	}

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

		EnemySpawner.Instance.SpawnWave ();
	}

	void Update() {
		if (stopped)
			return;

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
				EnemySpawner.Instance.OnWaveDone ();
			}
		}
	}

	void CommandUnitsToSwitch(bool equipItems) {
		if (!equipItems) {
			ArmySwitcher.Instance.Reset ();
		}
		for (int i = 0; i < UnitSelection.Instance.Workers.Count; i++) {
			SelectableObject worker = UnitSelection.Instance.Workers [i];
			worker.Deselect ();
			worker.CanBeSelected = false;

			Building closestCastle = BuildingManager.Instance.GetClosestBuilding(worker.transform, new List<BuildingType> { BuildingType.Castle }).GetComponent<Building>();

			Indicator indicator = closestCastle.SpotGenerator.GetClosestSpot (worker.transform.position);

			MovementModule unitMovement = worker.GetComponent<MovementModule> ();

			if (unitMovement != null) {
				worker.StopCurrentAction ();
				indicator.Connect (worker);
				worker.Indicator = indicator;
				unitMovement.MoveTowards (indicator.transform.position, (gameObject) => { 
					Vector3 lookPosition = closestCastle.transform.position;
					lookPosition.y = gameObject.transform.position.y;
					gameObject.transform.LookAt(lookPosition);

					if(equipItems) {
						ArmySwitcher.Instance.AssignWorker(worker);
					}

					if(!equipItems) {
						ArmySwitcher.Instance.ResetWorker(worker);
					}

					worker.CanBeSelected = true;
					worker.UnitStats.EquipItems(equipItems);
				});
			}
		}
	}
}
