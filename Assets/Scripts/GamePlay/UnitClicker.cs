using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class UnitClicker : MonoBehaviour {
	[SerializeField] LayerMask rayMask;
	[SerializeField] UnityEvent onNothingClick;

	void Update () {
		if (Input.GetMouseButtonDown (0)) {
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
				
			if (hit.collider.gameObject.layer == LayerMask.NameToLayer ("Building")) {
				Building building = hit.rigidbody.gameObject.GetComponent<Building> ();
				UISelectionController.Instance.HandleSelectedBuilding (building);
			} 
			else if (hit.collider.gameObject.layer == LayerMask.NameToLayer ("Resource")) {
				Resource resource = hit.rigidbody.gameObject.GetComponent<Resource> ();
				UISelectionController.Instance.HandleSelectedResource (resource);
			}
			else if (hit.collider.gameObject.layer == LayerMask.NameToLayer ("Unit")) {
				SelectableObject unit = hit.rigidbody.gameObject.GetComponent<SelectableObject> ();
				UnitSelection.Instance.SelectObjects (new List<SelectableObject> {unit  });
			}
			else {
				UnitSelection.Instance.SelectObjects (new List<SelectableObject> { });
				onNothingClick.Invoke ();
			}	
		}

	}
}
