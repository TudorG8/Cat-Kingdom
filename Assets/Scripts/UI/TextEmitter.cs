using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextEmitter : MonoBehaviour {
	[SerializeField] string val;

	public string Val {
		get {
			return this.val;
		}
		set {
			val = value;
		}
	}
}
