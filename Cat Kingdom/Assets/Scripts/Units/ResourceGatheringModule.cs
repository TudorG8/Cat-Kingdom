using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ResourceGatheringModule : MonoBehaviour {
	[SerializeField] float contentsGathered;
	[SerializeField] bool isGathering;
	[SerializeField] FillBar progressBar;
	[SerializeField] Transform contentsPosition;
	[SerializeField] MovementModule movementModule;
	[SerializeField] GameObject deliverDestination;
	[SerializeField] SelectableObject selectableObject;
	[SerializeField] bool delivering;
	[SerializeField] bool walkingToTarget;
	[SerializeField] NavMeshAgent navMeshAgent;
	[SerializeField] ResourceType currentResource;
	[SerializeField] Resource resource;
	[SerializeField] float maxGatherDistance = 10f;

	public float MaxGatherDistance {
		get {
			return this.maxGatherDistance;
		}
		set {
			maxGatherDistance = value;
		}
	}

	public ResourceType CurrentResource {
		get {
			return this.currentResource;
		}
		set {
			currentResource = value;
		}
	}

	public bool IsGathering {
		get {
			return this.isGathering;
		}
	}

	void SelectGatheringPoint(ResourceType resourceType) {
		deliverDestination = BuildingManager.Instance.GetClosestBuilding(transform, GetAvailableDestinations(resourceType));
	}

	List<BuildingType> GetAvailableDestinations(ResourceType resourceType) {
		switch (resourceType) {
		case ResourceType.Wood:
			return new List<BuildingType> { BuildingType.Castle, BuildingType.WoodMill };
		case ResourceType.Stone:
			return new List<BuildingType> { BuildingType.Castle, BuildingType.Quarry };
		case ResourceType.Food:
			return new List<BuildingType> { BuildingType.Castle, BuildingType.GatherersHut };
		case ResourceType.Fluff:
			return new List<BuildingType> { BuildingType.Castle, BuildingType.FluffGatherer };
		}
		return new List<BuildingType> ();
	}

	public void StartGathering(Resource resource) {
		this.resource = resource;
		StartCoroutine (GatheringRoutine (resource));
		isGathering = true;
	}

	public void Reset(bool stopRoutines = true) {
		resource.RemoveWorker (selectableObject);
		contentsGathered = 0;
		foreach (Transform child in contentsPosition) {
			Destroy (child.gameObject);
		}
		progressBar.gameObject.SetActive (false);
		isGathering = false;
		if(stopRoutines)
			StopAllCoroutines ();
		selectableObject.DisconnectIndicator ();
	}

	public void CompleteGathering() {
		if (delivering) {
			movementModule.StopMoving ();
			walkingToTarget = false;
			delivering = false;
		}
	}

	IEnumerator GatheringRoutine(Resource resource) {
		Vector3 currentResourcePos = resource.transform.position;
		ResourceType type = resource.ResourceType;
		while (true) {
			if (!resource.AssignWorker (selectableObject)) {
				Resource nextClosestResource = ResourceManager.Instance.GetNextClosestResource (type, currentResourcePos, maxGatherDistance);
				Reset (false);
				if (nextClosestResource == null || !nextClosestResource.AssignWorker (selectableObject)) {
					Debug.Log ("no resource");
					break;
				}
				resource = nextClosestResource;
				Indicator indicator = resource.SpotGenerator.GetClosestSpot (selectableObject.transform.position);
				indicator.Connect (selectableObject);
				selectableObject.Indicator = indicator;
				Debug.Log (indicator.transform.position);
			}

			yield return WalkToTarget (selectableObject.Indicator.transform.position);

			yield return CollectResource (resource);
			SelectGatheringPoint (resource.ResourceType);
			resource.RemoveWorker (selectableObject);
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
		currentResource = resource.ResourceType;
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

		resource.CurrentResources -= (int)contentsGathered;

		GameObject loot = Instantiate (resource.Obj, contentsPosition.position, resource.transform.rotation);
		loot.transform.SetParent (contentsPosition);
		loot.transform.localRotation = Quaternion.Euler (new Vector3 (0, 180, 0));

		progressBar.gameObject.SetActive (false);

		delivering = true;
	}

	void DropOffResource() { 
		switch (currentResource) {
		case ResourceType.Wood:
			UIResourceManager.Instance.Wood += (int)contentsGathered;
			break;
		case ResourceType.Stone:
			UIResourceManager.Instance.Stone += (int)contentsGathered;
			break;
		case ResourceType.Food:
			UIResourceManager.Instance.Food += (int)contentsGathered;
			break;
		case ResourceType.Fluff:
			UIResourceManager.Instance.Fluff += (int)contentsGathered;
			break;
		}

		contentsGathered = 0;
		foreach (Transform child in contentsPosition) {
			Destroy (child.gameObject);
		}
	}
}
