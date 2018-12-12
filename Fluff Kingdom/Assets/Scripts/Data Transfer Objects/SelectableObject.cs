using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SelectableObject : MonoBehaviour {
	[SerializeField] GameObject selectionBase;
	[SerializeField] string name;
	[SerializeField] Camera displayCamera;
	[SerializeField] Indicator indicator;
	[SerializeField] UnitStats unitStats;
	[SerializeField] bool canBeSelected = true;
	[SerializeField] Side side;
	[SerializeField] UnityEvent onDeath;
	[SerializeField] AttackableUnit attackableUnit;

	[SerializeField] TextEmitter textEmitter;
	[SerializeField] ProgressBarUpdater progressBarUpdater;

	public AttackableUnit AttackableUnit {
		get {
			return this.attackableUnit;
		}
		set {
			attackableUnit = value;
		}
	}

	public Side Side {
		get {
			return this.side;
		}
		set {
			side = value;
		}
	}

	public GameObject SelectionBase {
		get {
			return this.selectionBase;
		}
	}

	public string Name {
		get {
			return this.name;
		}
	}

	public Camera DisplayCamera {
		get {
			return this.displayCamera;
		}
	}

	public  Indicator Indicator {
		get {
			return this.indicator;
		}
		set {
			indicator = value;
		}
	}

	public UnitStats UnitStats {
		get {
			return this.unitStats;
		}
		set {
			unitStats = value;
		}
	}

	public bool CanBeSelected {
		get {
			return this.canBeSelected;
		}
		set {
			canBeSelected = value;
		}
	}

	void Start() {
		indicator = null;
	}

	void Update() {
		textEmitter.Val = unitStats.CurrentHitPoints + " / " + unitStats.UnitClass.HitPoints;
		progressBarUpdater.UpdateToValue ((float)unitStats.CurrentHitPoints / unitStats.UnitClass.HitPoints);
	}

	public void StopCurrentAction() {
		MovementModule movementModule = GetComponent<MovementModule> ();
		if (movementModule != null ) {
			movementModule.StopMoving ();
		}

		ResourceGatheringModule gatheringModule = GetComponent<ResourceGatheringModule> ();
		if (gatheringModule != null && gatheringModule.IsGathering) {
			gatheringModule.Reset ();
		}

		AttackModule attackModule = GetComponent<AttackModule> ();
		if (attackModule != null) {
			attackModule.Reset ();
		}

		DisconnectIndicator ();
	}

	public void DisconnectIndicator() {
		if(indicator != null)
			indicator.Disconnect ();
		indicator = null;
	}

	public void OnBecameVisible() {
		UnitSelection.Instance.NewObjectVisible (this);
	}

	public void OnBecameInvisible() {
		UnitSelection.Instance.ObjectWentInvisible (this);
	}

	public void Select() {
		if (canBeSelected) {
			selectionBase.SetActive (true);
		}
	}

	public void Deselect() {
		if(selectionBase) 
			selectionBase.SetActive (false);
	}

	public void TakeDamage(int damage) {
		unitStats.CurrentHitPoints -= damage;

		if (unitStats.CurrentHitPoints == 0) {
			if (side == Side.Ally) {
				UnitSelection.Instance.Workers.Remove (this);
			} 
			onDeath.Invoke ();
		}
	}
}
