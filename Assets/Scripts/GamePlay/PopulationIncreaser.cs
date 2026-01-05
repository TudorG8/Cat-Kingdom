using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationIncreaser : MonoBehaviour {
	[SerializeField] int amount;

	public void Increase() {
		UIResourceManager.Instance.MaximumPeople += amount;
	}

	public void Decrease() {
		UIResourceManager.Instance.MaximumPeople -= amount;
	}
}
