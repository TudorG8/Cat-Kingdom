using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandGiver : Singleton<CommandGiver> {
	[SerializeField] LayerMask rayMask;
	void Update () {
		if (Input.GetMouseButtonDown(1))
		{
			List<SelectableObject> selectedObjects = UnitSelection.Instance.SelectedObjects;
			for (int i = 0; i < selectedObjects.Count; i++)
			{
				SelectableObject selectedObject = selectedObjects[i];
				UnitMovement movementModule = selectedObject.GetComponent<UnitMovement>();

				if (movementModule != null)
				{
					RaycastHit hit;
					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					Vector3 destination;

					if (Physics.Raycast(ray, out hit, 100, rayMask))
					{
						destination = hit.point;
						movementModule.MoveTowards(destination);
					}
				}
			}
		}
	}
}
