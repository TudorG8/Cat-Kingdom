using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISelectionController : Singleton<UISelectionController> {
	[SerializeField] GameObject selectedUnitPrefab;
	[SerializeField] RenderTexture renderTexturePrefab;
	[SerializeField] Image image;

	[SerializeField] TextUpdater textUpdater;

	[SerializeField] UIMultipleSelectionPanel multipleSelectionPanel;
	[SerializeField] UISingleSelectionPanel   singleSelectionPanel  ;

	public Image Image {
		get {
			return this.image;
		}
		set {
			image = value;
		}
	}

	public TextUpdater TextUpdater {
		get {
			return this.textUpdater;
		}
		set {
			textUpdater = value;
		}
	}

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

		panel.Description.transform.parent.gameObject.SetActive (false);
		panel.Damage.transform.parent.gameObject.SetActive (true);
		panel.JobPanel.gameObject.SetActive (true);
		panel.TrainWorkerPanel.gameObject.SetActive (false);
		panel.JobAssignmentPanel.gameObject.SetActive (false);

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
			Vector3 scale = newPanelObj.transform.localScale;
			newPanelObj.transform.SetParent (panel.SelectedUnitParent);
			newPanelObj.transform.localScale = scale;

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

	public void HandleSelectedBuilding(Building building) {
		multipleSelectionPanel.SelectedUnitParent.gameObject.SetActive (false);
		singleSelectionPanel  .SelectedUnitParent.gameObject.SetActive (true);

		BuildingRecipe buildingInformation = building.Recipe;
		UISingleSelectionPanel panel = singleSelectionPanel;

		panel.Description.transform.parent.gameObject.SetActive (true);
		panel.Damage.transform.parent.gameObject.SetActive (false);
		panel.JobPanel.gameObject.SetActive (false);
		panel.JobAssignmentPanel.gameObject.SetActive (false);

		if (building.Recipe.BuildingName == BuildingType.Castle) {
			panel.TrainWorkerPanel.gameObject.SetActive (true);
		}

		if (building.Recipe.BuildingName == BuildingType.Barracks) {
			panel.JobAssignmentPanel.gameObject.SetActive (true);
		}

		RenderTexture newRenderTexture = Instantiate (renderTexturePrefab);
		panel.RenderTexture = newRenderTexture;
		panel.Name.text = buildingInformation.BuildingName.ToString();
		panel.CurrentCamera = building.Camera;
		panel.CurrentCamera.targetTexture = newRenderTexture;
		panel.Health.text = building.HitPoints + " / " + buildingInformation.HitPoints;
		panel.Description.text = building.Recipe.Description;

		textUpdater.TextEmitter = building.TextEmitter;
	}

	public void HandleSelectedResource(Resource resource) {
		multipleSelectionPanel.SelectedUnitParent.gameObject.SetActive (false);
		singleSelectionPanel  .SelectedUnitParent.gameObject.SetActive (true);

		UISingleSelectionPanel panel = singleSelectionPanel;

		panel.Description.transform.parent.gameObject.SetActive (true);
		panel.Damage.transform.parent.gameObject.SetActive (false);
		panel.JobPanel.gameObject.SetActive (false);
		panel.TrainWorkerPanel.gameObject.SetActive (false);
		panel.JobAssignmentPanel.gameObject.SetActive (false);

		RenderTexture newRenderTexture = Instantiate (renderTexturePrefab);
		panel.RenderTexture = newRenderTexture;
		panel.Name.text = resource.ResourceName;
		panel.CurrentCamera = resource.Camera;
		panel.CurrentCamera.targetTexture = newRenderTexture;
		panel.Health.text = resource.CurrentResources + " / " + resource.MaximumResources + " (" + resource.ResourceType + ")";
		panel.Description.text = resource.Description;

		textUpdater.TextEmitter = resource.TextEmitter;
	}
}
