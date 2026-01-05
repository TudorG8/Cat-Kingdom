using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class OnAttackEvent : UnityEvent<Vector3> {}

[RequireComponent(typeof(MovementModule))]
public class AttackModule : MonoBehaviour {
	[SerializeField] MovementModule movementModule;
	[SerializeField] float attackDelay;
	[SerializeField] bool canAttack = true;
	[SerializeField] bool inRange = false;
	[SerializeField] int aggroRange = 10;
	[SerializeField] bool lookForNextTarget = false;
	[SerializeField] Vector3 position;
	[SerializeField] LayerMask attackMask;
	[SerializeField] OnAttackEvent onAttackStart;
	[SerializeField] OnAttackEvent onAttackEnd;
	[SerializeField] bool isPursuing;
	[SerializeField] Side attackableSide;

	public bool IsPursuing {
		get {
			return this.isPursuing;
		}
		set {
			isPursuing = value;
		}
	}

	public delegate void AttackAction();

	public void Reset() {
		StopAllCoroutines ();
		canAttack = true;
		inRange = false;
		this.lookForNextTarget = false;
		isPursuing = false;
		movementModule.StopMoving ();
	}

	public void AttackTarget(AttackableUnit target, int minDamage, int maxDamage, float minDistance) {
		StartCoroutine (AttackRoutine(target, minDamage, maxDamage, minDistance));
	}

	public void AttackTarget(Vector3 position, int minDamage, int maxDamage, float minDistance) {
		this.position = position;
		this.lookForNextTarget = true;

		StartCoroutine (AttackRoutine(null, minDamage, maxDamage, minDistance));
	}

	IEnumerator AttackRoutine(AttackableUnit target, int minDamage, int maxDamage, float minDistance) {
		if (target == null) {
			target = GetNextClosestEnemy (position);
		}

		if (target == null) {
			movementModule.MoveTowards (position);
			yield break;
		} 
			
		movementModule.MoveTowards (target.transform.position);

		while (true) {
			isPursuing = true;
			// If we were targetting a specific enemy and they died, search for the next enemy
			if (!lookForNextTarget && (target == null || !target.CanStillBeAttacked)) {
				lookForNextTarget = true;
			}

			if (lookForNextTarget) {
				AttackableUnit closestUnit = GetNextClosestEnemy (transform.position);
				if (closestUnit != null && closestUnit != target) {
					Debug.Log ("changing target " + closestUnit.GetComponent<SelectableObject>().gameObject.name);
					target = closestUnit;
				}
			}

			if (target == null || !target.CanStillBeAttacked) {
				Debug.Log ("broke free");
				Reset ();
				yield break;
			}

			float distance = Vector3.Distance (transform.position, target.transform.position);

			if (canAttack && distance < minDistance) {
				transform.LookAt (target.transform.position);
				StartCoroutine (Attack (target, () => {
					int damage = Random.Range (minDamage, maxDamage);
					target.GetDamaged (damage);
				}));
					
				inRange = true;
				movementModule.StopMoving ();
			}

			else if (inRange && distance > minDistance) {
				inRange = false;
				movementModule.MoveTowards (target.transform.position);
			}

			if(distance >= minDistance)
				movementModule.UpdateTarget (target.transform.position);

			yield return new WaitForEndOfFrame ();
		}
	}

	IEnumerator Attack(AttackableUnit target, AttackAction action) {
		Vector3 pos = target.transform.position;
		canAttack = false;
		yield return new WaitForSeconds (attackDelay - (35f / 60));
		onAttackStart.Invoke (pos);
		yield return new WaitForSeconds ((35f / 60));
		if (target.CanStillBeAttacked) {
			action ();
		}
		onAttackEnd.Invoke (pos);
		canAttack = true;
	}

	AttackableUnit GetNextClosestEnemy(AttackableUnit target) {
		return GetNextClosestEnemy (target.transform.position);
	}

	public AttackableUnit GetNextClosestEnemy(Vector3 position) { 
		SelectableObject closestEnemy = null;
		float distance = float.MaxValue;
		for (int i = 0; i < UnitSelection.Instance.VisibleObjects.Count; i++) {
			SelectableObject obj = UnitSelection.Instance.VisibleObjects [i];
			if (obj.Side == attackableSide && obj.AttackableUnit.CanStillBeAttacked) {
				float newDistance = Vector3.Distance (obj.transform.position, position);

				bool validObj = true;
				if (newDistance >= distance) {
					validObj = false;
				}
					
				if (newDistance >= aggroRange) {
					validObj = false;
				}

				if (validObj) {
					closestEnemy = obj;
					distance = newDistance;
				}
			}
		}
		if (closestEnemy == null) {
			return null;
		}
		return closestEnemy.AttackableUnit;
	}
}
