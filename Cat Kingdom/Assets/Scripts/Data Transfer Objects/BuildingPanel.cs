using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public class BuildingPanelEvent : UnityEvent<BuildingRecipe> {}

public class BuildingPanel : MonoBehaviour {
	[SerializeField] Text buildingName;
	[SerializeField] Image image;
	[SerializeField] BuildingRecipe recipe;
	[SerializeField] BuildingPanelEvent onClick;

	public Text BuildingName {
		get {
			return this.buildingName;
		}
	}

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
}
