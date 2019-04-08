using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmySwitcher : Singleton<ArmySwitcher> {
	[SerializeField] int warriorCount;
	[SerializeField] int archerCount;
	[SerializeField] int spearmanCount;
	[SerializeField] Text titleText;
	[SerializeField] Text warriorText;
	[SerializeField] Text archerText;
	[SerializeField] Text spearmanText;
	[SerializeField] UnitClassInformation worker;
	[SerializeField] UnitClassInformation warrior;
	[SerializeField] UnitClassInformation archer;
	[SerializeField] UnitClassInformation spearman;


	[SerializeField] int currentlyAssignedWarriors;
	[SerializeField] int currentlyAssignedArchers;
	[SerializeField] int currentlyAssignedSpearman;

	public void AssignWorker(SelectableObject worker) {
		if (currentlyAssignedWarriors < warriorCount) {
			currentlyAssignedWarriors++;
			worker.UnitStats.UnitClass = warrior;
		}
		else if (currentlyAssignedArchers < archerCount) {
			currentlyAssignedArchers++;
			worker.UnitStats.UnitClass = archer;
		}
		else if (currentlyAssignedSpearman < spearmanCount) {
			currentlyAssignedSpearman++;
			worker.UnitStats.UnitClass = spearman;
		}

		worker.UnitStats.CurrentHitPoints = worker.UnitStats.UnitClass.HitPoints;
	}

	public void ResetWorker(SelectableObject worker) {
		worker.UnitStats.UnitClass = this.worker;
	}

	public void NewWorker() {
		warriorCount++;
	}

	public void Reset() {
		currentlyAssignedWarriors = 0;
		currentlyAssignedArchers = 0;
		currentlyAssignedSpearman = 0;
	}

	public void AddWarriors() {
		int maxUnits = UnitSelection.Instance.Workers.Count;

		if (warriorCount == maxUnits) {return;}

		if (archerCount > 0) {
			warriorCount++;
			archerCount--;
		} 
		else if (spearmanCount > 0) {
			warriorCount++;
			spearmanCount--;
		}
	}
	public void AddArchers() {
		int maxUnits = UnitSelection.Instance.Workers.Count;

		if (archerCount == maxUnits) {return;}

		if (warriorCount > 0) {
			archerCount++;
			warriorCount--;
		} 
		else if (spearmanCount > 0) {
			archerCount++;
			spearmanCount--;
		}
	}
	public void AddSpearman() {
		int maxUnits = UnitSelection.Instance.Workers.Count;

		if (spearmanCount == maxUnits) {return;}

		if (warriorCount > 0) {
			spearmanCount++;
			warriorCount--;
		} 
		else if (archerCount > 0) {
			spearmanCount++;
			archerCount--;
		}
	}
	public void MinusWarriors() {
		if (warriorCount == 0) {return;}

		warriorCount--;
		archerCount ++;
	}
	public void MinusArchers() {
		if (archerCount == 0) {return;}

		archerCount--;
		warriorCount ++;
	}
	public void MinusSpearman() {
		if (spearmanCount == 0) {return;}

		spearmanCount--;
		warriorCount ++;
	}

	void Awake() {
		InitiateSingleton ();
		Reset ();
	}

	void Start() {
		warriorCount = UnitSelection.Instance.Workers.Count;
	}

	void Update() {
		titleText   .text = "Available Workers (" + (warriorCount + archerCount + spearmanCount) + ")";
		warriorText .text = "Warriors (" + warriorCount  + ")";
		archerText  .text = "Archers ("  + archerCount   + ")";
		spearmanText.text = "Spearman (" + spearmanCount + ")";
	}
}
