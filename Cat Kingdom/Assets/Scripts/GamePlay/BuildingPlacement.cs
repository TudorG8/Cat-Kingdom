using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingPlacement : Singleton<BuildingPlacement> {
	[SerializeField] GameObject selectedObject;
	[SerializeField] BuildingRecipe selectedRecipe;
	[SerializeField] bool shouldLookForSpot;
	[SerializeField] LayerMask rayMask;
	[SerializeField] float transparency;
	[SerializeField] float gridSize;

	public bool ShouldLookForSpot {
		get {
			return this.shouldLookForSpot;
		}
		set {
			shouldLookForSpot = value;
		}
	}

	void Awake() {
		InitiateSingleton ();
	}

	public void Deselect() {
		if (selectedObject != null) {
			Destroy (selectedObject);
			Building buildingScript = selectedObject.GetComponent<Building> ();
			buildingScript.Cancelled = true;
			Debug.Log ("deslected");
		}
		shouldLookForSpot = false;
		selectedObject = null;
	}

	void Update () {
		if (shouldLookForSpot == true) {
			CheckForPosition ();

			if (EventSystem.current.IsPointerOverGameObject ()) {
				return;
			}

			if (Input.GetMouseButtonDown (0)) {
				Building buildingScript = selectedObject.GetComponent<Building> ();
				if (CheckGrid (buildingScript, selectedObject.transform.position)) {
					InitializeRecipe (selectedObject, selectedRecipe);
					int woodCost  = Mathf.Clamp(selectedRecipe.GetCost ("Wood" ), 0, UIResourceManager.Instance.MaxResources);
					int stoneCost = Mathf.Clamp(selectedRecipe.GetCost ("Stone"), 0, UIResourceManager.Instance.MaxResources);
					int foodCost = Mathf.Clamp(selectedRecipe.GetCost ("Food"), 0, UIResourceManager.Instance.MaxResources);
					UIResourceManager.Instance.Wood  -= woodCost;
					UIResourceManager.Instance.Stone -= stoneCost;
					UIResourceManager.Instance.Food -= foodCost;
					selectedObject.GetComponent<Building> ().StartColliding ();
					shouldLookForSpot = false;
					selectedObject = null;

					if (UnitSelection.Instance.SelectedObjects.Count != 0) {
						Debug.Log (buildingScript);
						CommandGiver.Instance.HandleBuildCommand (buildingScript);
					}
				}
			}
			else if (Input.GetMouseButtonDown (1)) {
				Deselect ();
			}
			else if (Input.GetKeyDown (KeyCode.Q)) {
				Building building = selectedObject.GetComponent<Building> ();
				building.RotateModel (90);
			} 
			else if (Input.GetKeyDown (KeyCode.E)) {
				Building building = selectedObject.GetComponent<Building> ();
				building.RotateModel (-90);
			}
		}
	}

	public void BuildingSelected(BuildingRecipe recipe) {
		Deselect ();
		if (UIResourceManager.Instance.Wood >= recipe.GetCost ("Wood") &&
			UIResourceManager.Instance.Stone >= recipe.GetCost ("Stone")) {
			selectedObject = Instantiate (recipe.Prefab, new Vector3 (), recipe.Prefab.transform.rotation);
			selectedObject.GetComponent<Building> ().StopColliding ();
			shouldLookForSpot = true;
			CheckForPosition ();
			selectedRecipe = recipe;
		}
	}

	void CheckForPosition() {
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		if (Physics.Raycast (ray, out hit, 100, rayMask)) {
			Vector3 snappedPosition = SnapToGrid (hit.point);
			selectedObject.transform.position = snappedPosition;
		}
	}

	bool CheckGrid(Building building, Vector3 position) {
		int x = (int)(position.x);
		int z = (int)(position.z);

		bool canBuild = true;
		for (int i = x - building.Recipe.Tiles/ 2; i < x + building.Recipe.Tiles/ 2 + 1; i++) {
			for (int j = z - building.Recipe.Tiles/ 2; j < z + building.Recipe.Tiles/ 2 + 1; j++) {
				if (GameGrid.Instance.GetWithOffset(i, j)) {
					canBuild = false;
					break;
				}
			}
		}
		return canBuild;
	}

	Vector3 SnapToGrid(Vector3 position) {
		return new Vector3 (
			Mathf.Round (position.x / gridSize) * gridSize, 
			position.y, 
			Mathf.Round (position.z / gridSize) * gridSize
		);
	}

	void InitializeRecipe(GameObject obj, BuildingRecipe recipe) {
		Building building = obj.GetComponent<Building> ();
		if (building != null) {
			building.Recipe = recipe;
		}
		building.StartConstruction ();

		int x = (int)(obj.transform.position.x);
		int z = (int)(obj.transform.position.z);

		for (int i = x - building.Recipe.Tiles / 2; i < x + building.Recipe.Tiles/ 2 + 1; i++) {
			for (int j = z - building.Recipe.Tiles/ 2; j < z + building.Recipe.Tiles/ 2 + 1; j++) {
				GameGrid.Instance.SetWithOffset (i, j, true);
			}
		}
	}
}
