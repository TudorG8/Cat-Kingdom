using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject : MonoBehaviour {
	[SerializeField] GameObject selectionBase;
	[SerializeField] string name;
	[SerializeField] Camera displayCamera;
	[SerializeField] Indicator indicator;

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

	void Start() {
		indicator = null;
	}

	public void StopCurrentAction() {
		ResourceGathering gatheringModule = GetComponent<ResourceGathering> ();
		if (gatheringModule != null && gatheringModule.IsGathering) {
			gatheringModule.Reset ();
		}

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
		selectionBase.SetActive (true);
	}

	public void Deselect() {
		selectionBase.SetActive (false);
	}
}
