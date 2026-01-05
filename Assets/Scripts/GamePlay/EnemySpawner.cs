using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Singleton<EnemySpawner> {
	[SerializeField] float multiplier;
	[SerializeField] int enemiesThisWave;
	[SerializeField] int distance;
	[SerializeField] List<EnemyBehaviour> enemies;
	[SerializeField] List<Transform> points;
	[SerializeField] GameObject enemy;

	public List<EnemyBehaviour> Enemies {
		get {
			return this.enemies;
		}
		set {
			enemies = value;
		}
	}

	public int CurrentEnemyCount {
		get {
			return enemies.Count;
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

	public void SpawnWave() {
		for (int i = 0; i < points.Count; i++) {
			int enemyCount = enemiesThisWave / points.Count;



			int cols = (int)Mathf.Sqrt (enemyCount);

			int enemiesCurrentlySpawned = 0;
			int x = 0;
			int z = 0;
			while (enemiesCurrentlySpawned < enemyCount) {
				GameObject enemyObj = Instantiate (enemy, points [i].position + new Vector3 (x * 2, 0, z * 2), Quaternion.identity);
				EnemyBehaviour behaviour = enemyObj.GetComponent<EnemyBehaviour> ();
				enemies.Add (behaviour);
				x++;

				if (x == cols) {
					x = 0;
					z++;
				}

				enemiesCurrentlySpawned++;
			}
		}
	}

	public void OnWaveDone() {
		enemiesThisWave = (int)(enemiesThisWave * multiplier);

		if (SessionData.Instance.HardMode) {
			enemiesThisWave = (int)(enemiesThisWave * multiplier);
		}
	}

	void Awake() {
		InitiateSingleton ();
	}
}
