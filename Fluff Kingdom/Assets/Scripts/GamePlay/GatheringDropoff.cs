using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatheringDropoff : MonoBehaviour {
	[SerializeField] List<ResourceType> resourceTypes;
	[SerializeField] bool canBeDropped;

	public void Activate() {
		canBeDropped = true;
	}

	void OnTriggerEnter(Collider other) {
		if (!canBeDropped) {
			return;
		}

		Rigidbody rigidBody = other.attachedRigidbody;
		if (rigidBody == null) {
			return;
		}
		ResourceGathering gatheringModule = rigidBody.GetComponent<ResourceGathering> ();

		if (gatheringModule != null && resourceTypes.Contains( gatheringModule.CurrentResource)) {
			gatheringModule.CompleteGathering ();
		}
	}
}
