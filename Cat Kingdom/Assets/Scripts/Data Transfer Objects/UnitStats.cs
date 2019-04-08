using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStats : MonoBehaviour {
	[SerializeField] int currentHitPoints;
	[SerializeField] UnitClassInformation unitClass;
	[SerializeField] Transform mainHandSpot;
	[SerializeField] Transform offHandSpot;
	[SerializeField] Transform backSpot;
	[SerializeField] Transform hatSpot;
	[SerializeField] Animator animator;

	void Start() {
		currentHitPoints = unitClass.HitPoints;
	}

	public int CurrentHitPoints {
		get {
			return this.currentHitPoints;
		}
		set {
			currentHitPoints = Mathf.Clamp(value, 0, int.MaxValue);
		}
	}

	public UnitClassInformation UnitClass {
		get {
			return this.unitClass;
		}
		set {
			unitClass = value;
		}
	}

	public void EquipItems(bool equipItems) {
		foreach (Transform child in mainHandSpot) {Destroy (child.gameObject);}
		foreach (Transform child in offHandSpot) {Destroy (child.gameObject);}
		foreach (Transform child in backSpot) {Destroy (child.gameObject);}
		foreach (Transform child in hatSpot) {Destroy (child.gameObject);}

		if (equipItems) {
			GameObject mainHand = Instantiate (unitClass.MainHand, mainHandSpot.transform.position, mainHandSpot.transform.rotation);
			mainHand.transform.SetParent (mainHandSpot);

			GameObject offHand = Instantiate (unitClass.OffHand, offHandSpot.transform.position, mainHandSpot.transform.rotation);
			offHand.transform.SetParent (offHandSpot);

			GameObject hat = Instantiate (unitClass.Hat, hatSpot.transform.position, mainHandSpot.transform.rotation);
			hat.transform.SetParent (hatSpot);

			GameObject back = Instantiate (unitClass.Back, backSpot.transform.position, mainHandSpot.transform.rotation);
			back.transform.SetParent (backSpot);

			animator.runtimeAnimatorController = unitClass.Animator;
		}
	}
}
