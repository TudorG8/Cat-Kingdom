using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerSpawner : Singleton<WorkerSpawner> {
	[SerializeField] GameObject workerPrefab;

	void Awake() {
		InitiateSingleton ();
	}

	public void SpawnNewWorker() {
		if (UnitSelection.Instance.Workers.Count >= UIResourceManager.Instance.MaximumPeople || PhaseSwitcher.Instance.CurrentPhase != GamePhase.Build)
			return;

		Vector3 position = BuildingManager.Instance.GetClosestBuilding (transform, new List<BuildingType> { BuildingType.Castle }).transform.position;

		GameObject workerObj = Instantiate (workerPrefab, position, Quaternion.identity);
		SelectableObject workerScript = workerObj.GetComponent<SelectableObject> ();

		workerObj.transform.position = position;

		UnitSelection.Instance.Workers.Add (workerScript);
		ArmySwitcher.Instance.NewWorker ();
	}
}
