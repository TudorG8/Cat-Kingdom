using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGatheringOnStart : MonoBehaviour {
	[SerializeField] ResourceGathering resourceGathering;
	[SerializeField] Resource resource;

	void Start() {
		StartCoroutine (GatheringRoutine ());
	}

	
	IEnumerator GatheringRoutine() {
		yield return new WaitForSeconds (0f);
		SelectableObject worker = GetComponent<SelectableObject> ();
		Indicator indicator = resource.SpotGenerator.GetClosestSpot (worker.transform.position);

		indicator.Connect (worker);
		worker.Indicator = indicator;
		resourceGathering.StartGathering (resource);
	}
}