using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshData : MonoBehaviour {
	[SerializeField] Vector3[] vertices;
	[SerializeField] Color[] colours;
	
	public static MeshData FromMesh(Mesh filter) {
		MeshData meshData = new MeshData();
        meshData.vertices = filter.vertices;
        meshData.colours = filter.colors;

        return meshData;
    }
}
