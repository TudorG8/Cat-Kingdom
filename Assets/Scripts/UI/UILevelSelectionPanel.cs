using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILevelSelectionPanel : MonoBehaviour {
	public void OnNameValueChanged(string val) {
		if (val.Length == 0) {
			val = "No Name";
		}

		SessionData.Instance.Name = val;
	}
}
