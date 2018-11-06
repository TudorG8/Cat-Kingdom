using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    [SerializeField] Vector3 startingPosition;
	[SerializeField] Vector3 destination;
	[SerializeField] float speed;
	[SerializeField] bool shouldMove;
    [SerializeField] float percentageDone;

	public bool ShouldMove
	{
		get { return this.shouldMove; }
		set { this.shouldMove = value; }
	}

	public void MoveTowards(Vector3 destination)
	{
	    startingPosition = transform.position;
		this.destination = destination;
	    this.destination.y = startingPosition.y;
		shouldMove = true;
	    percentageDone = 0;

	}

	void Update ()
	{
		if (shouldMove)
		{
		    transform.position = Vector3.Lerp(startingPosition, destination, percentageDone);

			if (percentageDone > 1)
			{
				shouldMove = false;
			}

		    percentageDone += 1 / speed * Time.deltaTime;
		}
	}
}
