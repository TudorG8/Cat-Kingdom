using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ResourceGathering : MonoBehaviour {
	[SerializeField] float contentsGathered;
	[SerializeField] bool isGathering;
	[SerializeField] FillBar progressBar;
	[SerializeField] GameObject contents;
	[SerializeField] UnitMovement movementModule;
	[SerializeField] GameObject deliverDestination;
	[SerializeField] SelectableObject selectableObject;
	[SerializeField] bool delivering;
	[SerializeField] bool walkingToTarget;
	[SerializeField] NavMeshAgent navMeshAgent;

	public bool IsGathering {
		get {
			return this.isGathering;
		}
	}

	void SelectGatheringPoint() {
		deliverDestination = BuildingManager.Instance.GetClosestBuilding(BuildingType.Castle);
	}

	public void StartGathering(Resource resource) {
		SelectGatheringPoint ();
		StartCoroutine (GatheringRoutine (resource));
		isGathering = true;
	}

	public void Reset() {
		contentsGathered = 0;
		contents.SetActive (false);
		progressBar.gameObject.SetActive (false);
		isGathering = false;
		StopAllCoroutines ();
	}

	public void CompleteGathering() {
		if (delivering) {
			movementModule.StopMoving ();
			walkingToTarget = false;
			delivering = false;
		}
	}

	IEnumerator GatheringRoutine(Resource resource) {
		while (true) {
			yield return WalkToTarget (selectableObject.Indicator.transform.position);
			yield return CollectResource (resource);
			yield return WalkToTarget (deliverDestination.transform.position);
			DropOffResource ();
		}
	}

	IEnumerator WalkToTarget(Vector3 target) {
		walkingToTarget = true;
		movementModule.MoveTowards (target, ((GameObject obj) => {
			walkingToTarget = false;
		}));

		while (walkingToTarget) {
			yield return new WaitForEndOfFrame ();
		}
	}

	IEnumerator CollectResource(Resource resource) {
		progressBar.gameObject.SetActive (true);
		float timeToHarvest = resource.TimeTaken;
		float currentTime = 0f;

		while (currentTime < timeToHarvest) {
			currentTime += Time.deltaTime;
			contentsGathered += resource.ContentsPerTrip * Time.deltaTime;
			progressBar.FillImage.fillAmount = currentTime / timeToHarvest;

			Vector3 lookPosition = resource.transform.position;
			lookPosition.y = gameObject.transform.position.y;
			gameObject.transform.LookAt(lookPosition);

			transform.position = selectableObject.Indicator.transform.position;
			yield return new WaitForEndOfFrame ();
		}

		contentsGathered = resource.ContentsPerTrip;

		contents.SetActive (true);
		progressBar.gameObject.SetActive (false);

		delivering = true;
	}

	void DropOffResource() {
		UIResourceManager.Instance.Wood += (int)contentsGathered;
		contentsGathered = 0;
		contents.SetActive (false);
	}
}
