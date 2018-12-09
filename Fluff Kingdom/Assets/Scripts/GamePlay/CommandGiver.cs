using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandGiver : Singleton<CommandGiver> {
	[SerializeField] LayerMask rayMask;
	[SerializeField] float minimumDistance;

	public delegate void Action(SelectableObject selectedObject);

	void Update () {
		if (Input.GetMouseButtonDown(1)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			Vector3 destination;

			if (Physics.Raycast (ray, out hit, 100, rayMask)) {
				destination = hit.point;
			} 
			else {
				return;
			}

			if (hit.collider.gameObject.layer == LayerMask.NameToLayer ("Building")) {
				Building building = hit.rigidbody.gameObject.GetComponent<Building> ();

				if (building != null) {
					HandleBuildCommand (building);
				}
			} 
			else if (hit.collider.gameObject.layer == LayerMask.NameToLayer ("Resource")) {
				Resource resource = hit.rigidbody.gameObject.GetComponent<Resource> ();

				if (resource != null) {
					HandleResourceCommand (resource);
				}
			}
			else if (hit.collider.gameObject.layer == LayerMask.NameToLayer ("Ground")) {
				HandleMoveToCommand (destination);
			}
		}
	}

	void HandleResourceCommand(Resource resource) {
		List<SelectableObject> selectedObjects = UnitSelection.Instance.SelectedObjects;

		for (int i = 0; i < selectedObjects.Count; i++) {
			SelectableObject selectedObject = selectedObjects [i];

			Indicator indicator = resource.SpotGenerator.GetClosestSpot (selectedObject.transform.position);

			if (indicator == null)
				continue;

			ResourceGathering gatheringModule = selectedObject.GetComponent<ResourceGathering> ();
			if (gatheringModule != null) {
				selectedObject.StopCurrentAction ();
				indicator.Connect (selectedObject);
				selectedObject.Indicator = indicator;
				gatheringModule.StartGathering (resource);
			}
		}
	}

	void HandleBuildCommand(Building building) {
		List<SelectableObject> selectedObjects = UnitSelection.Instance.SelectedObjects;

		for (int i = 0; i < selectedObjects.Count; i++) {
			SelectableObject selectedObject = selectedObjects [i];

			Indicator indicator = building.SpotGenerator.GetClosestSpot (selectedObject.transform.position);

			if (indicator == null)
				continue;

			UnitMovement unitMovement = selectedObject.GetComponent<UnitMovement> ();
			if (unitMovement != null) {
				selectedObject.StopCurrentAction ();
				indicator.Connect (selectedObject);
				selectedObject.Indicator = indicator;
				unitMovement.MoveTowards (indicator.transform.position, (gameObject) => { 
					building.BuilderAssigned(selectedObject);
					Vector3 lookPosition = building.transform.position;
					lookPosition.y = gameObject.transform.position.y;
					gameObject.transform.LookAt(lookPosition);
				});
			}
		}
	}

	void HandleMoveToCommand(Vector3 destination) {
		List<SelectableObject> selectedObjects = UnitSelection.Instance.SelectedObjects;

		Vector3 center = CalculateCenter (selectedObjects);

		for (int i = 0; i < selectedObjects.Count; i++) {
			SelectableObject selectedObject = selectedObjects [i];

			selectedObject.StopCurrentAction ();

			Vector3 direction = selectedObject.transform.position - center;
			direction.Normalize ();

			Vector3 actualDestination = destination + direction * minimumDistance;

			UnitMovement unitMovement = selectedObject.GetComponent<UnitMovement> ();
			if (unitMovement != null) {
				unitMovement.MoveTowards (actualDestination, (gameObject) => {
					Vector3 lookPosition = destination;
					lookPosition.y = gameObject.transform.position.y;
					gameObject.transform.LookAt(lookPosition);
				});
			}
		}
	}

	Vector3 CalculateCenter(List<SelectableObject> selectedObjects) {
		Vector3 center = new Vector3 ();
		for (int i = 0; i < selectedObjects.Count; i++) {
			center += selectedObjects [i].transform.position;
		}
		center /= selectedObjects.Count;

		return center;
	}
}
