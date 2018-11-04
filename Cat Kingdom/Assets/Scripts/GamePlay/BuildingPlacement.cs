using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacement : Singleton<BuildingPlacement> {
	[SerializeField] GameObject selectedObject;
	[SerializeField] bool shouldLookForSpot;
	[SerializeField] LayerMask rayMask;
	[SerializeField] float transparency;
	[SerializeField] float gridSize;

	void Awake() {
		InitiateSingleton ();
	}

	void Update () {
		if (shouldLookForSpot == true) {
			CheckForPosition ();

			if (Input.GetMouseButtonDown (0)) {
				SetObjectToTransparency (selectedObject, 1f);
				shouldLookForSpot = false;
				selectedObject = null;
			} 
			else if (Input.GetMouseButtonDown (1)) {
				Destroy (selectedObject);
				shouldLookForSpot = false;
				selectedObject = null;
			}
		}
	}

	public void BuildingSelected(BuildingRecipe recipe) {
		selectedObject = Instantiate (recipe.Prefab, new Vector3(), recipe.Prefab.transform.rotation);
		SetObjectToTransparency (selectedObject, transparency);
		shouldLookForSpot = true;
		CheckForPosition ();
	}

	void CheckForPosition() {
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		if (Physics.Raycast (ray, out hit, 100, rayMask)) {
			Vector3 snappedPosition = SnapToGrid (hit.point);
			selectedObject.transform.position = snappedPosition;
		}
	}

	Vector3 SnapToGrid(Vector3 position) {
		return new Vector3 (
			Mathf.Round (position.x / gridSize) * gridSize, 
			position.y, 
			Mathf.Round (position.z / gridSize) * gridSize
		);
	}

	void SetObjectToTransparency(GameObject o, float value) {
		Renderer[] objectRenderers = o.GetComponentsInChildren<Renderer> ();
		for (int i = 0; i < objectRenderers.Length; i++) {
			Color objColor = objectRenderers [i].material.color;
			objColor.a = value;
			objectRenderers [i].material.color = objColor;
		} 
	}
}
