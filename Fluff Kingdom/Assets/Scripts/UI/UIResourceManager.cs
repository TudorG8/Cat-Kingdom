using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIResourceManager : Singleton<UIResourceManager> {
	[SerializeField] Text woodText;
	[SerializeField] Text stoneText;
	[SerializeField] Text fluffText;

	[SerializeField] int wood;
	[SerializeField] int stone;
	[SerializeField] int fluff;

	[SerializeField] int maxResources;

	public int Wood {
		get {
			return this.wood;
		}
		set {
			wood = Mathf.Clamp(value, 0, maxResources);
		}
	}

	public int Stone {
		get {
			return this.stone;
		}
		set {
			stone = Mathf.Clamp(value, 0, maxResources);
		}
	}

	public int Fluff {
		get {
			return this.fluff;
		}
		set {
			fluff = Mathf.Clamp(value, 0, maxResources);
		}
	}

	void Awake() {
		InitiateSingleton ();
	}

	void Update() {
		woodText.text = wood.ToString();
		stoneText.text = stone.ToString();
		fluffText.text = fluff.ToString();
	}
}
