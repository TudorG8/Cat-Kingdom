using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour {
	[SerializeField] float timeTaken;
	[SerializeField] int contentsPerTrip;
	[SerializeField] List<SelectableObject> workers;
	[SerializeField] WorkableSpotsGenerator spotGenerator;

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
}
