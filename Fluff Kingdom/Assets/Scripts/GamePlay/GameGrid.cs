using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : Singleton<GameGrid> {
	[SerializeField] bool[,] occupiedTiles = new bool[128, 128];
	[SerializeField] Transform parent;
	[SerializeField] GameObject filled;
	[SerializeField] GameObject empty;

	void Awake() {
		InitiateSingleton ();
	}

	void Update() {
		if (false) {
			foreach (Transform child in parent) {
				Destroy (child.gameObject);
			}

			for (int i = -64; i < 64; i++) { 
				for (int j = -64; j < 64; j++) { 
					if (GetWithOffset (i, j)) {
						GameObject o = Instantiate (filled, new Vector3 (i, 0, j), Quaternion.identity);
						o.transform.SetParent (parent);
					} else {
						GameObject o = Instantiate (empty, new Vector3 (i, 0, j), Quaternion.identity);
						o.transform.SetParent (parent);
					}
				}
			}
		}
	}

	public bool[,] OccupiedTiles {
		get {
			return this.occupiedTiles;
		}
	}

	public bool GetWithOffset(int x, int y) {
		return occupiedTiles [x + 64, y + 64];
	}

	public void SetWithOffset(int x, int y, bool value) {
		occupiedTiles [x + 64, y + 64] = value;
	}
}
