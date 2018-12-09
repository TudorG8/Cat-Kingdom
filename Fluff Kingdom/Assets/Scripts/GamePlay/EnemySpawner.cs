using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Singleton<EnemySpawner> {
	[SerializeField] int currentEnemyCount;
	[SerializeField] int enemiesThisWave;

	public int CurrentEnemyCount {
		get {
			return this.currentEnemyCount;
		}
		set {
			currentEnemyCount = value;
		}
	}

	public int EnemiesThisWave {
		get {
			return this.enemiesThisWave;
		}
		set {
			enemiesThisWave = value;
		}
	}

	void Awake() {
		InitiateSingleton ();
	}
}
