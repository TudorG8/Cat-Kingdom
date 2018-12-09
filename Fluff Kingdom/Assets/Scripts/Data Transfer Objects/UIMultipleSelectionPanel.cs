using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMultipleSelectionPanel : MonoBehaviour {
	[SerializeField] Transform selectedUnitParent;
	[SerializeField] List<SelectedUnitPanel> selectedPanels;

	public Transform SelectedUnitParent {
		get {
			return this.selectedUnitParent;
		}
		set {
			selectedUnitParent = value;
		}
	}

	public List<SelectedUnitPanel> SelectedPanels {
		get {
			return this.selectedPanels;
		}
		set {
			selectedPanels = value;
		}
	}
}
