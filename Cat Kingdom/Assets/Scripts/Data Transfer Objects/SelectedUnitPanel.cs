using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedUnitPanel : MonoBehaviour {
	[SerializeField] Text name;
	[SerializeField] RawImage image;
	[SerializeField] RenderTexture renderTexture;
	[SerializeField] Camera currentCamera;

	public Text Name {
		get {
			return this.name;
		}
	}

	public RawImage Image {
		get {
			return this.image;
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
}
