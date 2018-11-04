using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject : MonoBehaviour {
	[SerializeField] GameObject selectionBase;
	[SerializeField] string name;
	[SerializeField] Camera displayCamera;

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

	void OnBecameVisible() {
		UnitSelection.Instance.NewObjectVisible (this);
	}

	void OnBecameInvisible() {
		UnitSelection.Instance.ObjectWentInvisible (this);
	}

	public void Select() {
		selectionBase.SetActive (true);
	}

	public void Deselect() {
		selectionBase.SetActive (false);
	}
}
