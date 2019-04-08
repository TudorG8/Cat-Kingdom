using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWar : MonoBehaviour {
	[SerializeField] LayerMask rayMask;
	[SerializeField] MeshFilter meshFilter;
	void Start () {
		
	}
	
	void Update () {
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out hit, 100, rayMask)) {

		}
	}
}
