using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBuildingController : MonoBehaviour {
	[SerializeField] List<BuildingRecipe> recipes;
	[SerializeField] GameObject buildingPanelPrefab;
	[SerializeField] Transform buildingPanel;
	[SerializeField] LayerMask rayMask;

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
		}
	}
}
