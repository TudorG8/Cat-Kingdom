using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialState : MonoBehaviour {
	[SerializeField] GameObject buildingPrefab;
	[SerializeField] Transform initialPosition;
	void Start() {
		GameObject buildingObj = Instantiate(buildingPrefab, initialPosition.position, Quaternion.identity);
		Building buildingScript = buildingObj.GetComponent<Building> ();
		buildingScript.ConstructionBar.gameObject.SetActive (false);
		buildingScript.IsBuilding = false;
		buildingScript.PercentageDone = 100;
		BuildingPlacement.Instance.Buildings.Add (buildingScript);

		int x = (int)(buildingObj.transform.position.x);
		int z = (int)(buildingObj.transform.position.z);

		for (int i = x - buildingScript.SpotGenerator.Tiles; i < x + buildingScript.SpotGenerator.Tiles; i++) {
			for (int j = z - buildingScript.SpotGenerator.Tiles; j < z + buildingScript.SpotGenerator.Tiles; j++) {
				GameGrid.Instance.SetWithOffset (i, j, true);
			}
		}
	}
}
