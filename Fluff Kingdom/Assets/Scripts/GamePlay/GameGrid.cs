using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : Singleton<GameGrid> {
	[SerializeField] bool[,] occupiedTiles = new bool[128, 128];

	void Awake() {
		InitiateSingleton ();
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
