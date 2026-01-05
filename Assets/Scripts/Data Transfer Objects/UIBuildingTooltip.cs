using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBuildingTooltip : MonoBehaviour {
	[SerializeField] Transform costs;
	[SerializeField] Text name;
	[SerializeField] Text description;

	public Transform Costs {
		get {
			return this.costs;
		}
		set {
			costs = value;
		}
	}

	public Text Name {
		get {
			return this.name;
		}
		set {
			name = value;
		}
	}

	public Text Description {
		get {
			return this.description;
		}
		set {
			description = value;
		}
	}
}
