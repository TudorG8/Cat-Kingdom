using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(fix))]
public class ObjectBuilderEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		fix myScript = (fix)target;
		if(GUILayout.Button("Fix"))
		{
			myScript.Fix();
		}
	}
}