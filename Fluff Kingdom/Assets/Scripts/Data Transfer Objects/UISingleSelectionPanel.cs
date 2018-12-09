using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISingleSelectionPanel : MonoBehaviour {
	[SerializeField] Text name;
	[SerializeField] RawImage image;
	[SerializeField] RenderTexture renderTexture;
	[SerializeField] Camera currentCamera;
	[SerializeField] Text damage;
	[SerializeField] Text health;
	[SerializeField] Text jobName;
	[SerializeField] Image jobImage;
	[SerializeField] Transform selectedUnitParent;

	public Text Name {
		get {
			return this.name;
		}
		set {
			name = value;
		}
	}

	public RawImage Image {
		get {
			return this.image;
		}
		set {
			image = value;
		}
	}

	public RenderTexture RenderTexture {
		get {
			return this.renderTexture;
		}
		set {
			renderTexture = value;
			image.texture = value;
		}
	}

	public Camera CurrentCamera {
		get {
			return this.currentCamera;
		}
		set {
			currentCamera = value;
		}
	}

	public Text Damage {
		get {
			return this.damage;
		}
		set {
			damage = value;
		}
	}

	public Text Health {
		get {
			return this.health;
		}
		set {
			health = value;
		}
	}

	public Text JobName {
		get {
			return this.jobName;
		}
		set {
			jobName = value;
		}
	}

	public Image JobImage {
		get {
			return this.jobImage;
		}
		set {
			jobImage = value;
		}
	}

	public Transform SelectedUnitParent {
		get {
			return this.selectedUnitParent;
		}
		set {
			selectedUnitParent = value;
		}
	}
}
