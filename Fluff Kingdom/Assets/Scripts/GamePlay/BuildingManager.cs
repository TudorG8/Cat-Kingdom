using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : Singleton<BuildingManager> {
	[SerializeField] List<Building> buildings;

	public List<Building> Buildings {
		get {
			return this.buildings;
		}
		set {
			buildings = value;
		}
	}

	public GameObject GetClosestBuilding(BuildingType type) {
		Building building = null;
		float distance = float.MaxValue;
		for (int i = 0; i < buildings.Count; i++) {
			if (buildings [i].Recipe.BuildingName == type) {
				float newDistance = Vector3.Distance(transform.position, buildings[i].transform.position);

				if (newDistance < distance) {
					distance = newDistance;
					building = buildings [i];
				}
			}
		}
		return building.gameObject;
	}

	void Awake() {
		InitiateSingleton ();
	}
}
