using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class AttackEvent : UnityEvent<int> {}

public class AttackableUnit : MonoBehaviour {
	[SerializeField] bool canStillBeAttacked;
	[SerializeField] AttackEvent onDamageTaken;

	public bool CanStillBeAttacked {
		get {
			return this.canStillBeAttacked;
		}
		set {
			canStillBeAttacked = value;
		}
	}

	public void GetDamaged(int damage) {
		onDamageTaken.Invoke (damage);
	}
}
