using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacement : Singleton<BuildingPlacement> {
	[SerializeField] GameObject selectedObject;
	[SerializeField] BuildingRecipe selectedRecipe;
	[SerializeField] bool shouldLookForSpot;
	[SerializeField] LayerMask rayMask;
	[SerializeField] float transparency;
	[SerializeField] float gridSize;
	[SerializeField] List<Building> buildings;

	public List<Building> Buildings {
		get {
			return this.buildings;
		}
	}

	void Awake() {
		InitiateSingleton ();
	}

	void Update () {
		if (shouldLookForSpot == true) {
			CheckForPosition ();

			if (Input.GetMouseButtonDown (0)) {
				if (CheckGrid (selectedObject.GetComponent<Building> (), selectedObject.transform.position)) {
					InitializeRecipe (selectedObject, selectedRecipe);
					UIResourceManager.Instance.Wood -= selectedRecipe.GetCost ("Wood");
					shouldLookForSpot = false;
					selectedObject = null;
				}
			}
			else if (Input.GetMouseButtonDown (1)) {
				Destroy (selectedObject);
				shouldLookForSpot = false;
				selectedObject = null;
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
		if (UIResourceManager.Instance.Wood >= recipe.GetCost ("Wood")) {
			selectedObject = Instantiate (recipe.Prefab, new Vector3 (), recipe.Prefab.transform.rotation);
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
		for (int i = x - building.SpotGenerator.Tiles; i < x + building.SpotGenerator.Tiles; i++) {
			for (int j = z - building.SpotGenerator.Tiles; j < z + building.SpotGenerator.Tiles; j++) {
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

		for (int i = x - building.SpotGenerator.Tiles; i < x + building.SpotGenerator.Tiles; i++) {
			for (int j = z - building.SpotGenerator.Tiles; j < z + building.SpotGenerator.Tiles; j++) {
				GameGrid.Instance.SetWithOffset (i, j, true);
			}
		}
	}
}
