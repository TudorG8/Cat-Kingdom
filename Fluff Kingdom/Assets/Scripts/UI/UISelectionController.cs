using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISelectionController : Singleton<UISelectionController> {
	[SerializeField] GameObject selectedUnitPrefab;
	[SerializeField] Transform selectedUnitParent;
	[SerializeField] List<SelectedUnitPanel> selectedPanels;
	[SerializeField] RenderTexture renderTexturePrefab;

	void Start() {
		InitiateSingleton ();
	}

	public void UpdateSelectedUnits(List<SelectableObject> selectedObjects) {
		for (int i = 0; i < selectedPanels.Count; i++) {
			selectedPanels [i].gameObject.SetActive (false);

			if (selectedPanels [i].CurrentCamera != null) {
				selectedPanels [i].CurrentCamera.targetTexture = null;
				selectedPanels [i].CurrentCamera = null;
			}
		}

		for(int i = selectedPanels.Count; i <  selectedObjects.Count; i++) {
			GameObject newPanelObj = Instantiate (selectedUnitPrefab, selectedUnitParent.position, selectedUnitPrefab.transform.rotation);
			newPanelObj.transform.SetParent (selectedUnitParent);

			RenderTexture newRenderTexture = Instantiate (renderTexturePrefab);

			SelectedUnitPanel selectedUnitPanelScript = newPanelObj.GetComponent<SelectedUnitPanel> ();
			selectedUnitPanelScript.RenderTexture = newRenderTexture;

			selectedPanels.Add (selectedUnitPanelScript);
		}

		for (int i = 0; i < selectedObjects.Count; i++) {
			SelectableObject selectedObject = selectedObjects [i];
			selectedPanels [i].gameObject.SetActive (true);
			selectedPanels [i].Name.text = selectedObject.Name;
			selectedPanels [i].CurrentCamera = selectedObject.DisplayCamera;

			selectedPanels [i].CurrentCamera.targetTexture = selectedPanels [i].RenderTexture;
		}
	}
}
