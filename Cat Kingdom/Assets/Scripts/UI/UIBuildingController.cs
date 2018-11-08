using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBuildingController : MonoBehaviour {
	[SerializeField] List<BuildingRecipe> recipes;
	[SerializeField] GameObject buildingPanelPrefab;
	[SerializeField] Transform buildingPanel;
	[SerializeField] LayerMask rayMask;
	[SerializeField] List<BuildingPanel> panels;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < recipes.Count; i++) {
			BuildingRecipe recipe = recipes [i];
			GameObject newPanel = Instantiate (buildingPanelPrefab, buildingPanel.position, buildingPanelPrefab.transform.rotation);
			newPanel.transform.SetParent (buildingPanel, false);

			BuildingPanel buildingPanelScript = newPanel.GetComponent<BuildingPanel> ();
			buildingPanelScript.BuildingName.text = recipe.BuildingName;
			buildingPanelScript.Image.sprite = recipe.Image;
			buildingPanelScript.OnClick.AddListener	(BuildingPlacement.Instance.BuildingSelected);
			buildingPanelScript.Recipe = recipe;
			buildingPanelScript.Cost.text = "Wood: " + buildingPanelScript.Recipe.GetCost ("Wood").ToString ();

			panels.Add (buildingPanelScript);
		}
	}

	void Update() {
		for (int i = 0; i < panels.Count; i++) {
			if (UIResourceManager.Instance.Wood < panels [i].Recipe.GetCost("Wood")) {
				panels [i].Image.color = Color.red;
			} 
			else {
				panels [i].Image.color = Color.white;
			}
		}
	}
}
