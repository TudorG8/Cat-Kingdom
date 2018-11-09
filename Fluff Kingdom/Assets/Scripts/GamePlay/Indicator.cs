using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour {
	[SerializeField] WorkableSpotsGenerator generator;
	[SerializeField] SelectableObject selectedObject;

	public SelectableObject SelectedObject {
		get {
			return this.selectedObject;
		}
		set {
			selectedObject = value;
		}
	}

	public WorkableSpotsGenerator Generator {
		get {
			return this.generator;
		}
		set {
			generator = value;
		}
	}

	public void Disconnect() {
		generator.MarkSpotAs (this, false);
		if(generator.Building != null)
			generator.Building.BuilderRemoved (selectedObject);
	}

	public void Connect(SelectableObject selectedObject) {
		this.selectedObject = selectedObject;
		generator.MarkSpotAs (this, true);
	}
}
