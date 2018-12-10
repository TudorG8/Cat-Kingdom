using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public class BuildingPanelEvent : UnityEvent<BuildingRecipe> {}

public class BuildingPanel : MonoBehaviour {
	[SerializeField] Image image;
	[SerializeField] BuildingRecipe recipe;
	[SerializeField] BuildingPanelEvent onClick;
	[SerializeField] OnMouseEvents mouseEvents;

	public Image Image {
		get {
			return this.image;
		}
	}

	public BuildingRecipe Recipe {
		get {
			return this.recipe;
		}
		set {
			this.recipe = value;
		}
	}

	public BuildingPanelEvent OnClick {
		get {
			return this.onClick;
		}
	}

	public void OnClickEvent() {
		onClick.Invoke (recipe);
	}

	public OnMouseEvents MouseEvents {
		get {
			return this.mouseEvents;
		}
		set {
			mouseEvents = value;
		}
	}
}
