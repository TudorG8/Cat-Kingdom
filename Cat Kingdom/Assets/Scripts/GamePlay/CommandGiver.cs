using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CommandGiver : Singleton<CommandGiver> {
	[SerializeField] LayerMask rayMask;
	[SerializeField] float minimumDistance;

	public delegate void Action(SelectableObject selectedObject);

	void Awake() {
		InitiateSingleton ();
	}

	void Update () {
		if (Input.GetMouseButtonDown(1)) {
			ExecuteClickCommand ();
		}
	}

	public void ExecuteClickCommand(UIActionPanel.Action action = UIActionPanel.Action.None) {
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Vector3 destination;

		if (Physics.Raycast (ray, out hit, 100, rayMask)) {
			destination = hit.point;
		} 
		else {
			return;
		}

		if (EventSystem.current.IsPointerOverGameObject ()) {
			return;
		}

		if (UnitSelection.Instance.SelectedObjects.Count == 1 && UnitSelection.Instance.SelectedObjects [0].Side == Side.Enemy) {
			return;
		}	

		if (hit.collider.gameObject.layer == LayerMask.NameToLayer ("Building") 
			&& PhaseSwitcher.Instance.CurrentPhase == GamePhase.Build
			&& action == UIActionPanel.Action.None
		) {
			Building building = hit.rigidbody.gameObject.GetComponent<Building> ();
			if (building != null && !building.Cancelled) {
				HandleBuildCommand (building);
			}
		} 
		else if (hit.collider.gameObject.layer == LayerMask.NameToLayer ("Resource") 
			&& PhaseSwitcher.Instance.CurrentPhase == GamePhase.Build
			&& action == UIActionPanel.Action.None
		){
			Resource resource = hit.rigidbody.gameObject.GetComponent<Resource> ();

			if (resource != null) {
				HandleResourceCommand (resource);
			}
		}
		else if (hit.collider.gameObject.layer == LayerMask.NameToLayer ("Unit")) {
			
			SelectableObject unit = hit.rigidbody.gameObject.GetComponent<SelectableObject> ();

			if (unit != null && unit.Side == Side.Enemy) {
				HandleAttackCommand (unit.GetComponent<AttackableUnit>());
			}
		}
		else if (hit.collider.gameObject.layer == LayerMask.NameToLayer ("Ground")) {
			if (action == UIActionPanel.Action.Attack) {
				HandleAttackCommand (destination);
			} 
			else {
				HandleMoveToCommand (destination);
			}
		}
	}

	public void HandleAttackCommand(AttackableUnit target) {
		List<SelectableObject> selectedObjects = UnitSelection.Instance.SelectedObjects;

		for (int i = 0; i < selectedObjects.Count; i++) {
			SelectableObject selectedObject = selectedObjects [i];

			selectedObject.StopCurrentAction ();

			AttackModule attackModule = selectedObject.GetComponent<AttackModule> ();

			attackModule.AttackTarget (target, selectedObject.UnitStats.UnitClass.MinDamage, selectedObject.UnitStats.UnitClass.MaxDamage, selectedObject.UnitStats.UnitClass.Range);
		}
	}

	public void HandleAttackCommand(Vector3 position) {
		List<SelectableObject> selectedObjects = UnitSelection.Instance.SelectedObjects;

		for (int i = 0; i < selectedObjects.Count; i++) {
			SelectableObject selectedObject = selectedObjects [i];

			selectedObject.StopCurrentAction ();

			AttackModule attackModule = selectedObject.GetComponent<AttackModule> ();

			attackModule.AttackTarget (position, selectedObject.UnitStats.UnitClass.MinDamage, selectedObject.UnitStats.UnitClass.MaxDamage, selectedObject.UnitStats.UnitClass.Range);
		}
	}

	public void HandleResourceCommand(Resource resource) {
		List<SelectableObject> selectedObjects = UnitSelection.Instance.SelectedObjects;

		for (int i = 0; i < selectedObjects.Count; i++) {
			SelectableObject selectedObject = selectedObjects [i];

			selectedObject.StopCurrentAction ();

			Indicator indicator = resource.SpotGenerator.GetClosestSpot (selectedObject.transform.position);
			ResourceGatheringModule gatheringModule = selectedObject.GetComponent<ResourceGatheringModule> ();

			if (indicator == null)
				continue;

			if (gatheringModule == null)
				continue;

			if (resource.CanBeHarvested ()) {
				indicator.Connect (selectedObject);
				selectedObject.Indicator = indicator;
				gatheringModule.StartGathering (resource);
			} 
			else {
				Resource nextClosestResource = ResourceManager.Instance.GetNextClosestResource (resource.ResourceType, resource.transform.position, gatheringModule.MaxGatherDistance);


				if (nextClosestResource == null) {
					continue;
				}

				indicator = nextClosestResource.SpotGenerator.GetClosestSpot (selectedObject.transform.position);

				if (indicator == null) {
					continue;
				}
					
				indicator.Connect (selectedObject);
				selectedObject.Indicator = indicator;
				gatheringModule.StartGathering (nextClosestResource);
			}
		}
	}

	public void HandleBuildCommand(Building building) {
		List<SelectableObject> selectedObjects = UnitSelection.Instance.SelectedObjects;

		for (int i = 0; i < selectedObjects.Count; i++) {
			SelectableObject selectedObject = selectedObjects [i];

			Indicator indicator = building.SpotGenerator.GetClosestSpot (selectedObject.transform.position);

			if (indicator == null)
				continue;

			MovementModule unitMovement = selectedObject.GetComponent<MovementModule> ();
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

		UnitSelection.Instance.DeselectObjects ();
		UISelectionController.Instance.UpdateSelectedUnits (new List<SelectableObject>());
	}

	public void HandleMoveToCommand(Vector3 destination) {
		List<SelectableObject> selectedObjects = UnitSelection.Instance.SelectedObjects;

		Vector3 center = CalculateCenter (selectedObjects);

		for (int i = 0; i < selectedObjects.Count; i++) {
			SelectableObject selectedObject = selectedObjects [i];

			selectedObject.StopCurrentAction ();

			Vector3 direction = selectedObject.transform.position - center;
			direction.Normalize ();

			Vector3 actualDestination = destination + direction * minimumDistance;

			MovementModule unitMovement = selectedObject.GetComponent<MovementModule> ();
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
