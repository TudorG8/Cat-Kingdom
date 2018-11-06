using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UnitSelection : Singleton<UnitSelection> {
	[SerializeField] Vector3 startPosition;
	[SerializeField] bool createSelectionBox;
	[SerializeField] Texture2D texture;
	[SerializeField] Color mainColor;
	[SerializeField] Color borderColor;
	[SerializeField] int borderSize;
	[SerializeField] List<SelectableObject> visibleObjects;
	[SerializeField] List<SelectableObject> selectedObjects;
	[SerializeField] Rect currentRect;

	public List<SelectableObject> SelectedObjects
	{
		get { return selectedObjects; }
	}

	void Start() {
		InitiateSingleton ();
		InitializeTexture ();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			startPosition = Input.mousePosition;
			createSelectionBox = true;
		}
		if (Input.GetMouseButtonUp (0)) {
			createSelectionBox = false;
			SelectObjects ();
		}
	}

	public void NewObjectVisible(SelectableObject obj) {
		visibleObjects.Add (obj);
	}

	public void ObjectWentInvisible(SelectableObject obj) {
		visibleObjects.Remove (obj);
	}

	void InitializeTexture() {
		texture = new Texture2D( 1, 1 );
		texture.SetPixel( 0, 0, Color.white );
		texture.Apply();
	}

	/**
	 * Create a Rect between two positions. 
	 * Top left corner will be the minimum of the two and the Bottom right corner will be the maximum of the two.
	 */
	Rect GetRectFromPositions(Vector3 point1, Vector3 point2) {
		float height = Screen.height;

		// Unity points are at the top left and Rects are the bottom left.
		// Need to reverse them
		point1.y = height - point1.y;
		point2.y = height - point2.y;

		Vector3 topLeftCorner  = Vector3.Min (point1, point2);
		Vector3 botRightCorner = Vector3.Max (point1, point2);

		return Rect.MinMaxRect(topLeftCorner.x, topLeftCorner.y, botRightCorner.x, botRightCorner.y);
	}

	void DrawBoxFromRect(Rect rect, Color color) {
		GUI.color = color;
		GUI.DrawTexture (rect, texture);
		GUI.color = Color.white;
	}

	void DrawBorderAroundRectBox(Rect rect, Color color, int borderSize) {
		DrawBoxFromRect (new Rect (rect.xMin, rect.yMin, rect.width, borderSize ), color);
		DrawBoxFromRect (new Rect (rect.xMin, rect.yMax - borderSize, rect.width, borderSize ), color);

		DrawBoxFromRect (new Rect (rect.xMin, rect.yMin, borderSize, rect.height), color);
		DrawBoxFromRect (new Rect (rect.xMax - borderSize, rect.yMin, borderSize, rect.height), color);
	}

	void OnGUI(){
		if( createSelectionBox ) {
			currentRect = GetRectFromPositions (startPosition, Input.mousePosition);
			DrawBoxFromRect (currentRect, mainColor);
			DrawBorderAroundRectBox (currentRect, borderColor, borderSize);
		}
	}

	void DeselectObjects() {
		for (int i = 0; i < selectedObjects.Count; i++) {
			SelectableObject obj = selectedObjects [i];
			obj.Deselect ();
		}

		selectedObjects.Clear ();
	}

	void SelectObjects() {
		DeselectObjects ();

		for (int i = 0; i < visibleObjects.Count; i++) {
			SelectableObject obj = visibleObjects [i]; 
			Vector3 position = Camera.main.WorldToScreenPoint (obj.transform.position);
			position.y = Screen.height - position.y;
			if (currentRect.Contains (position)) {
				obj.Select ();
				selectedObjects.Add (obj);
			}
		}

		UISelectionController.Instance.UpdateSelectedUnits (selectedObjects);
	}
}
