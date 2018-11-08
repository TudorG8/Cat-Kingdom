using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkableSpotsGenerator : MonoBehaviour {
	[SerializeField] Dictionary<Indicator, bool> possibleWorkerLocations;
	[SerializeField] int tiles;
	[SerializeField] Transform indicatorParent;
	[SerializeField] GameObject indicatorPrefab;
	[SerializeField] Building building;

	[SerializeField] int maximumWorkers;

	public int MaximumWorkers {
		get {
			return this.maximumWorkers;
		}
	}

	public Building Building {
		get {
			return this.building;
		}
	}

	public int Tiles {
		get {
			return this.tiles;
		}
	}

	void Start() {
		Vector3 center = transform.position;

		maximumWorkers = tiles * 2;

		possibleWorkerLocations = new Dictionary<Indicator, bool> ();

		Vector3 position = center - new Vector3((tiles - 1) / 2, 0, tiles / 2 + 0.5f);
		for (int i = 0; i < tiles - 1; i++) { 
			GameObject indicator = Instantiate (indicatorPrefab, position, Quaternion.identity);
			indicator.transform.SetParent (indicatorParent);

			Indicator indicatorScript = indicator.GetComponent<Indicator> ();
			indicatorScript.Generator = this;

			position += new Vector3 (1, 0, 0);

			possibleWorkerLocations.Add (indicatorScript, false);
		}

		position = center - new Vector3((tiles - 1) / 2, 0, - (tiles / 2 + 0.5f));
		for (int i = 0; i < tiles - 1; i++) { 
			GameObject indicator = Instantiate (indicatorPrefab, position, Quaternion.identity);
			indicator.transform.SetParent (indicatorParent);

			Indicator indicatorScript = indicator.GetComponent<Indicator> ();
			indicatorScript.Generator = this;

			position += new Vector3 (1, 0, 0);

			possibleWorkerLocations.Add (indicatorScript, false);
		}

		position = center - new Vector3( - (tiles / 2 + 0.5f), 0, (tiles - 1) / 2);
		for (int i = 0; i < tiles - 1; i++) { 
			GameObject indicator = Instantiate (indicatorPrefab, position, Quaternion.identity);
			indicator.transform.SetParent (indicatorParent);

			Indicator indicatorScript = indicator.GetComponent<Indicator> ();
			indicatorScript.Generator = this;

			position += new Vector3 (0, 0, 1);

			possibleWorkerLocations.Add (indicatorScript, false);
		}

		position = center - new Vector3(tiles / 2 + 0.5f, 0, (tiles - 1) / 2);
		for (int i = 0; i < tiles - 1; i++) { 
			GameObject indicator = Instantiate (indicatorPrefab, position, Quaternion.identity);
			indicator.transform.SetParent (indicatorParent);

			Indicator indicatorScript = indicator.GetComponent<Indicator> ();
			indicatorScript.Generator = this;

			position += new Vector3 (0, 0, 1);

			possibleWorkerLocations.Add (indicatorScript, false);
		}
	}

	public Indicator GetClosestSpot(Vector3 position) {
		Indicator closest = null;
		float distance = float.MaxValue;

		foreach (KeyValuePair<Indicator, bool> location in possibleWorkerLocations) {
			if (!location.Value) {
				float newDistance = Vector3.Distance (position, location.Key.transform.position);
				if (newDistance < distance) {
					distance = newDistance;
					closest = location.Key;
				}
			}
		}

		return closest;
	}

	public bool MarkSpotAs(Indicator position, bool taken) {
		if (!possibleWorkerLocations.ContainsKey (position)) {
			return false;
		}

		possibleWorkerLocations [position] = taken;

		return true;
	}
}