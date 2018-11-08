using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    [SerializeField] Vector3 startingPosition;
	[SerializeField] Vector3 destination;
	[SerializeField] float speed;
	[SerializeField] bool shouldMove;
	[SerializeField] AfterMoveAction afterMoveAction;
	[SerializeField] Transform model;
	[SerializeField] Rigidbody rigidbody;

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

		transform.LookAt (this.destination);
		shouldMove = true;
	}

	public void StopMoving() {
		shouldMove = false;
		rigidbody.velocity = Vector3.zero;
		rigidbody.angularVelocity = Vector3.zero;
	}

	void FixedUpdate () {
		if (shouldMove) {
			transform.LookAt (this.destination);

			float distance = Vector3.Distance (transform.position, destination);

			float movement = speed * Time.deltaTime;

			if (distance > movement) {
				rigidbody.velocity = transform.forward * movement;
			} 
			else {
				rigidbody.velocity = transform.forward * distance * Time.deltaTime;
				//rigidbody.MovePosition(transform.position + transform.forward * distance * Time.deltaTime);
				shouldMove = false;
				if (afterMoveAction != null) {
					afterMoveAction (gameObject);
				}
			}
		}
	}
}
