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
	[SerializeField] Text description;
	[SerializeField] Text health;
	[SerializeField] Text jobName;
	[SerializeField] Image jobImage;
	[SerializeField] Transform selectedUnitParent;
	[SerializeField] Transform jobPanel;
	[SerializeField] Transform trainWorkerPanel;
	[SerializeField] Transform jobAssignmentPanel;

	public Transform JobAssignmentPanel {
		get {
			return this.jobAssignmentPanel;
		}
		set {
			jobAssignmentPanel = value;
		}
	}

	public Transform TrainWorkerPanel {
		get {
			return this.trainWorkerPanel;
		}
		set {
			trainWorkerPanel = value;
		}
	}

	public Text Description {
		get {
			return this.description;
		}
		set {
			description = value;
		}
	}

	public Transform JobPanel {
		get {
			return this.jobPanel;
		}
		set {
			jobPanel = value;
		}
	}

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
