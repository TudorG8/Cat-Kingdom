using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialState : MonoBehaviour {
	[SerializeField] GameObject buildingPrefab;
	[SerializeField] Transform initialPosition;
	void Start() {
		GameObject buildingObj = Instantiate(buildingPrefab, initialPosition.position, Quaternion.identity);
		Building building = buildingObj.GetComponent<Building> ();
		building.ConstructionBar.gameObject.SetActive (false);
		building.IsBuilding = false;
		building.PercentageDone = 100;
		BuildingManager.Instance.Buildings.Add (building);

		int x = (int)(buildingObj.transform.position.x);
		int z = (int)(buildingObj.transform.position.z);

		for (int i = x - building.Recipe.Tiles / 2; i < x + building.Recipe.Tiles / 2 + 1; i++) {
			for (int j = z - building.Recipe.Tiles/ 2; j < z + building.Recipe.Tiles/ 2 + 1; j++) {
				GameGrid.Instance.SetWithOffset (i, j, true);
			}
		}
	}
}
