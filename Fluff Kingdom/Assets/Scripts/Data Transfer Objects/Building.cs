using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

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
	[SerializeField] Camera camera;
	[SerializeField] int hitPoints;
	[SerializeField] TextEmitter textEmitter;
	[SerializeField] ProgressBarUpdater progressBarUpdater;
	[SerializeField] UnityEvent buildingFinished;
	[SerializeField] UnityEvent buildingDestroyed;
	[SerializeField] bool cancelled;
	[SerializeField] UnityEvent onDeath;

	public bool Cancelled {
		get {
			return this.cancelled;
		}
		set {
			cancelled = value;
		}
	}

	public TextEmitter TextEmitter {
		get {
			return this.textEmitter;
		}
		set {
			textEmitter = value;
		}
	}

	public int HitPoints {
		get {
			return this.hitPoints;
		}
		set {
			hitPoints = Mathf.Clamp (value, 0, int.MaxValue);
		}
	}

	public Camera Camera {
		get {
			return this.camera;
		}
		set {
			camera = value;
		}
	}

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

	void Awake() {
		spotGenerator.Tiles = recipe.Tiles;
		hitPoints = recipe.HitPoints;
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

	public void TakeDamage(int damage) {
		hitPoints -= damage;

		if (hitPoints == 0) {
			onDeath.Invoke ();
		}
	}

	void Update() {
		textEmitter.Val = HitPoints + " / " + recipe.HitPoints;
		progressBarUpdater.UpdateToValue ((float)hitPoints / recipe.HitPoints);
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
				buildingFinished.Invoke ();

			}
		}
	}
}
