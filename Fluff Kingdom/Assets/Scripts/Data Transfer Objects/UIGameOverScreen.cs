using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameOverScreen : MonoBehaviour {
	[SerializeField] Text score;
	[SerializeField] Transform victoryTitle;
	[SerializeField] Transform defeatTitle;
	[SerializeField] Text fluffScore;
	[SerializeField] Text timeScore;

	public Text FluffScore {
		get {
			return this.fluffScore;
		}
		set {
			fluffScore = value;
		}
	}

	public Text TimeScore {
		get {
			return this.timeScore;
		}
		set {
			timeScore = value;
		}
	}

	public Transform VictoryTitle {
		get {
			return this.victoryTitle;
		}
		set {
			victoryTitle = value;
		}
	}

	public Transform DefeatTitle {
		get {
			return this.defeatTitle;
		}
		set {
			defeatTitle = value;
		}
	}

	public Text Score {
		get {
			return this.score;
		}
		set {
			score = value;
		}
	}
}
