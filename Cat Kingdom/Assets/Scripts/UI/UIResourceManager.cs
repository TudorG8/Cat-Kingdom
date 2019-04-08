using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIResourceManager : Singleton<UIResourceManager> {
	[SerializeField] Text woodText;
	[SerializeField] Text stoneText;
	[SerializeField] Text foodText;
	[SerializeField] Text fluffText;
	[SerializeField] Text people;

	[SerializeField] int wood;
	[SerializeField] int stone;
	[SerializeField] int food;
	[SerializeField] int fluff;
	[SerializeField] int maximumPeople;

	[SerializeField] int peopleLimit;
	[SerializeField] int maxResources;

	public int MaximumPeople {
		get {
			return this.maximumPeople;
		}
		set {
			maximumPeople = Mathf.Clamp (value, 0, peopleLimit);
		}
	}

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

	public int Food {
		get {
			return this.food;
		}
		set {
			food = Mathf.Clamp(value, 0, maxResources);
		}
	}


	public int MaxResources {
		get {
			return this.maxResources;
		}
		set {
			maxResources = value;
		}
	}

	void Awake() {
		InitiateSingleton ();
	}

	void Update() {
		woodText.text = wood.ToString();
		stoneText.text = stone.ToString();
		foodText.text = food.ToString ();
		fluffText.text = fluff.ToString();

		people.text = UnitSelection.Instance.Workers.Count + "/" + maximumPeople;
	}
}
