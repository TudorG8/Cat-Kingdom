using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BuildingRecipe : ScriptableObject {
	[System.Serializable]
	public class Pair {
		[SerializeField] string type;
		[SerializeField] int cost;

		public string Type {
			get {
				return this.type;
			}
		}

		public int Cost {
			get {
				return this.cost;
			}
		}
	}

	#if UNITY_EDITOR
	[MenuItem("Assets/Level Design/Create/Crafting Recipe")]
	public static void CreateMyAsset() {
		BuildingRecipe asset = ScriptableObject.CreateInstance<BuildingRecipe>();

		AssetDatabase.CreateAsset(asset, "Assets/Prefabs/Building Recipe.asset");
		AssetDatabase.SaveAssets();

		EditorUtility.FocusProjectWindow();

		Selection.activeObject = asset;
	}
	#endif

	[SerializeField] string buildingName;
	[SerializeField] Sprite image;
	[SerializeField] int maximumWorkers;
	[SerializeField] int tiles;
	[SerializeField] int constructionTime;
	[SerializeField] GameObject prefab;
	[SerializeField] List<Pair> materials;

	public int GetCost(string type) {
		for (int i = 0; i < materials.Count; i++) {
			if (materials [i].Type == type) {
				return materials [i].Cost;
			}
		}
		return -1;
	}

	public string BuildingName {
		get {
			return this.buildingName;
		}
	}

	public List<Pair> Materials {
		get {
			return this.materials;
		}
	}

	public Sprite Image {
		get {
			return this.image;
		}
	}

	public GameObject Prefab {
		get {
			return this.prefab;
		}
	}

	public int ConstructionTime {
		get {
			return this.constructionTime;
		}
	}

	public int MaximumWorkers {
		get {
			return this.maximumWorkers;
		}
	}

	public int Tiles {
		get {
			return this.tiles;
		}
	}
}
