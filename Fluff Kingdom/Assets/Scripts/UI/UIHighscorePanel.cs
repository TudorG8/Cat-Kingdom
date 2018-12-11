using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHighscorePanel : MonoBehaviour {
	[SerializeField] Transform parent;
	[SerializeField] GameObject highscorePrefab;

	void Start() {
		List<RecordKeeper.Data.Record> records = RecordKeeper.Instance.RecordData.Records;

		for (int i = 0; i < records.Count; i++) {
			GameObject newObj = Instantiate(highscorePrefab, new Vector3(), highscorePrefab.transform.rotation);
			Vector3 scale = newObj.transform.localScale;
			newObj.transform.SetParent (parent, false);
			newObj.transform.localScale = scale;

			UIHighscore highscoreScript = newObj.GetComponent<UIHighscore> ();
			highscoreScript.Name.text = records [i].Name + ": ";
			highscoreScript.Score.text = records [i].Val.ToString();
		}
	}
}
