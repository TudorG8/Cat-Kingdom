using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISelectionController : Singleton<UISelectionController> {
	[SerializeField] GameObject selectedUnitPrefab;
	[SerializeField] RenderTexture renderTexturePrefab;


	[SerializeField] UIMultipleSelectionPanel multipleSelectionPanel;
	[SerializeField] UISingleSelectionPanel   singleSelectionPanel  ;

	void Start() {
		InitiateSingleton ();
	}

	public void UpdateSelectedUnits(List<SelectableObject> selectedObjects) {
		if (selectedObjects.Count != 1) {
			HandleMultipleSelectedUnits (selectedObjects);
		} 
		else {
			HandleSingleSelectedUnit (selectedObjects [0]);
		}
	}

	void HandleSingleSelectedUnit(SelectableObject selectedObject) {
		multipleSelectionPanel.SelectedUnitParent.gameObject.SetActive (false);
		singleSelectionPanel  .SelectedUnitParent.gameObject.SetActive (true);

		UnitStats unitInformation = selectedObject.UnitStats;
		UISingleSelectionPanel panel = singleSelectionPanel;

		RenderTexture newRenderTexture = Instantiate (renderTexturePrefab);
		panel.RenderTexture = newRenderTexture;
		panel.Name.text = selectedObject.Name;
		panel.CurrentCamera = selectedObject.DisplayCamera;
		panel.CurrentCamera.targetTexture = newRenderTexture;
		panel.Damage.text = unitInformation.UnitClass.MinDamage + " - " + unitInformation.UnitClass.MaxDamage;
		panel.Health.text = unitInformation.CurrentHitPoints + " / " + unitInformation.UnitClass.HitPoints;
		panel.JobName.text = unitInformation.UnitClass.ClassName.ToString ();
		panel.JobImage.sprite = unitInformation.UnitClass.Sprite;
	}

	void HandleMultipleSelectedUnits(List<SelectableObject> selectedObjects) {
		multipleSelectionPanel.SelectedUnitParent.gameObject.SetActive (true);
		singleSelectionPanel  .SelectedUnitParent.gameObject.SetActive (false);

		UIMultipleSelectionPanel panel = multipleSelectionPanel;
		for (int i = 0; i < panel.SelectedPanels.Count; i++) {
			panel.SelectedPanels [i].gameObject.SetActive (false);

			if (panel.SelectedPanels [i].CurrentCamera != null) {
				panel.SelectedPanels [i].CurrentCamera.targetTexture = null;
				panel.SelectedPanels [i].CurrentCamera = null;
			}
		}

		for(int i = panel.SelectedPanels.Count; i <  selectedObjects.Count; i++) {
			GameObject newPanelObj = Instantiate (selectedUnitPrefab, panel.SelectedUnitParent.position, selectedUnitPrefab.transform.rotation);
			newPanelObj.transform.SetParent (panel.SelectedUnitParent);

			RenderTexture newRenderTexture = Instantiate (renderTexturePrefab);

			SelectedUnitPanel selectedUnitPanelScript = newPanelObj.GetComponent<SelectedUnitPanel> ();
			selectedUnitPanelScript.RenderTexture = newRenderTexture;

			panel.SelectedPanels.Add (selectedUnitPanelScript);
		}

		for (int i = 0; i < selectedObjects.Count; i++) {
			SelectableObject selectedObject = selectedObjects [i];
			panel.SelectedPanels [i].gameObject.SetActive (true);
			panel.SelectedPanels [i].Name.text = selectedObject.Name;
			panel.SelectedPanels [i].CurrentCamera = selectedObject.DisplayCamera;

			panel.SelectedPanels [i].CurrentCamera.targetTexture = panel.SelectedPanels [i].RenderTexture;
		}
	}
}
