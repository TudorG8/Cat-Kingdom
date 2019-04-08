using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class UnitClassInformation : ScriptableObject {
	#if UNITY_EDITOR
	[MenuItem("Assets/Level Design/Create/Unit Class")]
	public static void CreateMyAsset() {
		UnitClassInformation asset = ScriptableObject.CreateInstance<UnitClassInformation>();

		AssetDatabase.CreateAsset(asset, "Assets/Data/Unit Class.asset");
		AssetDatabase.SaveAssets();

		EditorUtility.FocusProjectWindow();

		Selection.activeObject = asset;
	}
	#endif

	[SerializeField] UnitClass className;
	[SerializeField] int minDamage;
	[SerializeField] int maxDamage;
	[SerializeField] int hitPoints;
	[SerializeField] Sprite sprite;
	[SerializeField] GameObject mainHand;
	[SerializeField] GameObject offHand;
	[SerializeField] GameObject back;
	[SerializeField] GameObject hat;
	[SerializeField] float range;
	[SerializeField] RuntimeAnimatorController animator;

	public RuntimeAnimatorController Animator {
		get {
			return this.animator;
		}
		set {
			animator = value;
		}
	}

	public UnitClass ClassName {
		get {
			return this.className;
		}
		set {
			className = value;
		}
	}

	public int MinDamage {
		get {
			return this.minDamage;
		}
		set {
			minDamage = value;
		}
	}

	public int MaxDamage {
		get {
			return this.maxDamage;
		}
		set {
			maxDamage = value;
		}
	}

	public int HitPoints {
		get {
			return this.hitPoints;
		}
		set {
			hitPoints = value;
		}
	}

	public Sprite Sprite {
		get {
			return this.sprite;
		}
		set {
			sprite = value;
		}
	}

	public GameObject MainHand {
		get {
			return this.mainHand;
		}
		set {
			mainHand = value;
		}
	}

	public GameObject OffHand {
		get {
			return this.offHand;
		}
		set {
			offHand = value;
		}
	}

	public GameObject Back {
		get {
			return this.back;
		}
		set {
			back = value;
		}
	}

	public GameObject Hat {
		get {
			return this.hat;
		}
		set {
			hat = value;
		}
	}

	public float Range {
		get {
			return this.range;
		}
		set {
			range = value;
		}
	}
}
