using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class UIBuildingController : MonoBehaviour {
	[SerializeField] List<BuildingRecipe> recipes;
	[SerializeField] GameObject buildingPanelPrefab;
	[SerializeField] GameObject toolTip;
	[SerializeField] GameObject woodCost;
	[SerializeField] GameObject stoneCost;
	[SerializeField] GameObject foodCost;
	[SerializeField] Transform buildingPanel;
	[SerializeField] Transform tooltipPanel;
	[SerializeField] LayerMask rayMask;
	[SerializeField] List<BuildingPanel> panels;

	// Use this for initialization
	void Start () {
		foreach (Transform child in tooltipPanel) { Destroy (child.gameObject);}

		for (int i = 0; i < recipes.Count; i++) {
			BuildingRecipe recipe = recipes [i];
			GameObject newPanel = Instantiate (buildingPanelPrefab, buildingPanel.position, buildingPanelPrefab.transform.rotation);
			Vector3 scale = newPanel.transform.localScale;
			newPanel.transform.SetParent (buildingPanel, false);
			newPanel.transform.localScale = scale;
			newPanel.transform.SetAsLastSibling ();

			GameObject newTooltip = Instantiate (toolTip, tooltipPanel.position, toolTip.transform.rotation);
			newTooltip.transform.SetParent (tooltipPanel, false);
			newTooltip.gameObject.SetActive (false);

			Texture2D texture = AssetPreview.GetAssetPreview (recipe.Prefab);

			UIBuildingTooltip tooltipScript = newTooltip.GetComponent<UIBuildingTooltip> ();
			tooltipScript.Name.text = recipe.BuildingName.ToString();
			tooltipScript.Description.text = recipe.Description;

			int woodCostAmount  = recipe.GetCost ("Wood");
			int stoneCostAmount = recipe.GetCost ("Stone");
			int foodCostAmount = recipe.GetCost ("Food");
			if (woodCostAmount != -1) {
				GameObject woodCostObj = Instantiate (woodCost, tooltipPanel.position, woodCost.transform.rotation);
				scale = woodCostObj.transform.localScale;
				woodCostObj.transform.SetParent (tooltipScript.Costs);
				woodCostObj.transform.localScale = scale;

				UIText textScript = woodCostObj.GetComponent<UIText> ();
				textScript.Text.text = woodCostAmount.ToString();
			}
			if (stoneCostAmount != -1) {
				GameObject stoneCostObj = Instantiate (stoneCost, tooltipPanel.position, stoneCost.transform.rotation);
				scale = stoneCostObj.transform.localScale;
				stoneCostObj.transform.SetParent (tooltipScript.Costs);
				stoneCostObj.transform.localScale = scale;

				UIText textScript = stoneCostObj.GetComponent<UIText> ();
				textScript.Text.text = stoneCostAmount.ToString();
			}
			if (foodCostAmount != -1) {
				GameObject foodCostObj = Instantiate (foodCost, tooltipPanel.position, foodCost.transform.rotation);
				scale = foodCostObj.transform.localScale;
				foodCostObj.transform.SetParent (tooltipScript.Costs);
				foodCostObj.transform.localScale = scale;

				UIText textScript = foodCostObj.GetComponent<UIText> ();
				textScript.Text.text = foodCostAmount.ToString();
			}


			BuildingPanel buildingPanelScript = newPanel.GetComponent<BuildingPanel> ();
			buildingPanelScript.Image.sprite = Sprite.Create(texture, new Rect(0, 0, 128, 128), new Vector2(0.5f, 0.5f), 100.0f);
			buildingPanelScript.OnClick.AddListener (((arg0) => { 
				BuildingPlacement.Instance.ShouldLookForSpot = false;
			}));
			buildingPanelScript.OnClick.AddListener	(BuildingPlacement.Instance.BuildingSelected);

			buildingPanelScript.MouseEvents.OnMouseOver.AddListener (() => {
				newTooltip.gameObject.SetActive(true);
				newTooltip.transform.position = newPanel.transform.position - new Vector3(175, 0);
			});
			buildingPanelScript.MouseEvents.OnMouseExit.AddListener (() => {
				newTooltip.gameObject.SetActive(false);
			});

			buildingPanelScript.Recipe = recipe;

			panels.Add (buildingPanelScript);
		}
	}

	void Update() {
		for (int i = 0; i < panels.Count; i++) {
			if (UIResourceManager.Instance.Wood  < panels [i].Recipe.GetCost("Wood") || 
				UIResourceManager.Instance.Stone < panels [i].Recipe.GetCost("Stone") ||
				UIResourceManager.Instance.Food < panels [i].Recipe.GetCost("Food")) {
				panels [i].Image.color = Color.red;
			} 
			else {
				panels [i].Image.color = Color.white;
			}
		}
	}
}
