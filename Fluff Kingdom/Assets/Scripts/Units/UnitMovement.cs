using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    [SerializeField] Vector3 startingPosition;
	[SerializeField] Vector3 destination;
	[SerializeField] float speed;
	[SerializeField] bool shouldMove;
	[SerializeField] AfterMoveAction afterMoveAction;
	[SerializeField] Transform model;
	[SerializeField] Rigidbody rigidbody;
	[SerializeField] NavMeshAgent navMeshAgent;

	public delegate void AfterMoveAction(GameObject obj);

	public bool ShouldMove {
		get { return this.shouldMove; }
		set { this.shouldMove = value; }
	}

	public void MoveTowards(Vector3 destination, AfterMoveAction action = null) {
	    startingPosition = transform.position;
		this.destination = destination;
	    this.destination.y = startingPosition.y;
		afterMoveAction = action;

		shouldMove = true;

		navMeshAgent.destination = destination;

		navMeshAgent.isStopped = false;
	}

	public void StopMoving() {
		shouldMove = false;
		rigidbody.velocity = Vector3.zero;
		rigidbody.angularVelocity = Vector3.zero;
	}

	void FixedUpdate () {
		if (shouldMove) {
			//transform.LookAt (this.destination);

			float distance = Vector3.Distance (transform.position, destination);

			if (distance < 0.1f) {
				if (afterMoveAction != null) {
					afterMoveAction (gameObject);
					navMeshAgent.isStopped = true;
					transform.LookAt (this.destination);
				}
				shouldMove = false;
			}
		}
	}
}
