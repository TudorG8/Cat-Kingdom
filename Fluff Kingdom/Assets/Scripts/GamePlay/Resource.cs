using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour {
	[SerializeField] string resourceName;
	[SerializeField] string description;
	[SerializeField] float timeTaken;
	[SerializeField] int contentsPerTrip;
	[SerializeField] List<SelectableObject> workers;
	[SerializeField] WorkableSpotsGenerator spotGenerator;
	[SerializeField] ResourceType resourceType;
	[SerializeField] GameObject obj;
	[SerializeField] int tiles;
	[SerializeField] int maximumResources;
	[SerializeField] Camera camera;
	[SerializeField] ProgressBarUpdater progressBarUpdater;
	[SerializeField] TextEmitter textEmitter;
	[SerializeField] DeathChecker deathChecker;

	[SerializeField] int currentResources;

	public string Description {
		get {
			return this.description;
		}
		set {
			description = value;
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

	public int MaximumResources {
		get {
			return this.maximumResources;
		}
		set {
			maximumResources = value;
		}
	}

	public int CurrentResources {
		get {
			return this.currentResources;
		}
		set {
			currentResources = Mathf.Clamp(value, 0, maximumResources);
			if (currentResources != maximumResources) {
				progressBarUpdater.ProgressBar.gameObject.SetActive (true);	
			}
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

	public string ResourceName {
		get {
			return this.resourceName;
		}
		set {
			resourceName = value;
		}
	}

	public ResourceType ResourceType {
		get {
			return this.resourceType;
		}
		set {
			resourceType = value;
		}
	}

	public GameObject Obj {
		get {
			return this.obj;
		}
		set {
			obj = value;
		}
	}

	public float TimeTaken {
		get {
			return this.timeTaken;
		}
	}

	public WorkableSpotsGenerator SpotGenerator {
		get {
			return this.spotGenerator;
		}
	}

	public int ContentsPerTrip {
		get {
			return this.contentsPerTrip;
		}
	}

	public bool CanBeHarvested() {
		return ((workers.Count + 1) * contentsPerTrip <= currentResources);
	}

	void Awake()  {
		spotGenerator.Tiles = tiles;
		currentResources = maximumResources;
		deathChecker.Val = currentResources;
	}

	void Update() {
		progressBarUpdater.UpdateToValue ((float)currentResources / maximumResources);
		deathChecker.Val = currentResources;
		textEmitter.Val = CurrentResources + " / " + MaximumResources + " (" + ResourceType + ")";
	}

	public bool AssignWorker(SelectableObject worker) {
		if ((workers.Count + 1) * contentsPerTrip > currentResources) {
			return false;
		}
			
		workers.Add (worker);

		return true;
	}

	public void RemoveResource() {
		ResourceManager.Instance.RemoveResource (this);
	}

	public void RemoveWorker(SelectableObject worker) {
		workers.Remove (worker);
	}

	void Start() {
		int x = (int)(transform.position.x);
		int z = (int)(transform.position.z);

		int half = (int)Mathf.Round((float)tiles / 2);
		for (int i = x - half; i < x + half + 1; i++) {
			for (int j = z - half; j < z + half + 1; j++) {
				GameGrid.Instance.SetWithOffset (i, j, true);
			}
		}
	}
}
