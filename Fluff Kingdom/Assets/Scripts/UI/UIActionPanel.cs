using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIActionPanel : MonoBehaviour {
	public enum Action {
		None, Move, Attack
	}
	[SerializeField] Texture2D movementSprite;
	[SerializeField] Texture2D attackSprite;
	[SerializeField] Action currentAction = Action.None;

	public void Stop() {
		for (int i = 0; i < UnitSelection.Instance.SelectedObjects.Count; i++) {
			UnitSelection.Instance.SelectedObjects [i].StopCurrentAction ();
		}
	}

	public void SelectMovement() {
		currentAction = Action.Move;
		Cursor.SetCursor(movementSprite, Vector2.zero, CursorMode.Auto);
	}

	public void SelectAttack() {
		currentAction = Action.Attack;
		Cursor.SetCursor(attackSprite, Vector2.zero, CursorMode.Auto);
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.A)) {
			SelectAttack ();
		}
		if ((Input.GetMouseButtonDown (0) || Input.GetMouseButtonDown (1)) && currentAction != Action.None) {
			CommandGiver.Instance.ExecuteClickCommand (currentAction);
			currentAction = Action.None;
			Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
		}
	}
}
