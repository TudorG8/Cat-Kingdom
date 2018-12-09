using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Building : MonoBehaviour {
	
	[SerializeField] bool isBuilding;
	[SerializeField] float percentageDone;
	[SerializeField] FillBar constructionBar;
	[SerializeField] float distanceToMove;
	[SerializeField] BuildingRecipe recipe;
	[SerializeField] Transform topPoint;
	[SerializeField] List<SelectableObject> workers;
	[SerializeField] WorkableSpotsGenerator spotGenerator;
	[SerializeField] Transform model;
	[SerializeField] Collider collider;
	[SerializeField] NavMeshObstacle navMeshObstacle;

	public void StopColliding() {
		collider.enabled = false;
		navMeshObstacle.enabled = false;
	}

	public void StartColliding() {
		collider.enabled = true;
		navMeshObstacle.enabled = true;
	}

	public void RotateModel(int angles) {
		model.Rotate (0, angles, 0);
	}

	public BuildingRecipe Recipe {
		get {
			return this.recipe;
		}
		set {
			recipe = value;
		}
	}

	public WorkableSpotsGenerator SpotGenerator {
		get {
			return this.spotGenerator;
		}
	}

	public bool IsBuilding {
		get {
			return this.isBuilding;
		}
		set {
			isBuilding = value;
		}
	}

	public float PercentageDone {
		get {
			return this.percentageDone;
		}
		set {
			percentageDone = value;
		}
	}

	public FillBar ConstructionBar {
		get {
			return this.constructionBar;
		}
	}

	public void StartConstruction() {
		Vector3 destination = topPoint.position;
		distanceToMove = destination.y;

		Vector3 position = model.position;
		position.y = -destination.y;
		model.position = position;

		isBuilding = true;
	}

	public bool BuilderAssigned(SelectableObject worker) {
		if (workers.Count >= spotGenerator.MaximumWorkers) {
			return false;
		}

		workers.Add (worker);
		if (!isBuilding) {
			isBuilding = true;
		}

		return true;
	}

	public void BuilderRemoved(SelectableObject worker) {
		workers.Remove (worker);
		if (workers.Count == 0) {
			isBuilding = false;
		}
	}

	void OnTriggerEnter(Collider other) {
		Rigidbody rigidBody = other.attachedRigidbody;
		if (rigidBody == null || !name.Contains("Castle")) {
			return;
		}
		ResourceGathering gatheringModule = rigidBody.GetComponent<ResourceGathering> ();

		if (gatheringModule != null) {
			gatheringModule.CompleteGathering ();
		}
	}

	void Update() {
		if (isBuilding && percentageDone < 100) {
			percentageDone += 100 / recipe.ConstructionTime * Time.deltaTime * workers.Count;

			Vector3 position = model.position;
			position.y = -distanceToMove + distanceToMove * percentageDone / 100;
			model.position = position;

			constructionBar.FillImage.fillAmount = percentageDone / 100;

			if (percentageDone > 100) {
				constructionBar.gameObject.SetActive (false);
				isBuilding = false;
				BuildingManager.Instance.Buildings.Add (this);
			}
		}
	}
}
