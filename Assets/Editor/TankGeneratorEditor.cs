using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TankGenerator))]
public class TankGeneratorEditor : Editor {

	public override void OnInspectorGUI(){
		DrawDefaultInspector();
		TankGenerator tankGenerator = (TankGenerator) target;

		// if(GUILayout.Button("Start")){
		// 	Time.timeScale = 1;
		// } 

		// if(GUILayout.Button("Stop")){
		// 	Time.timeScale = 0;
		// } 

		if(GUILayout.Button("generate")){
			tankGenerator.InitVertices();
			tankGenerator.GenerateTank();
		} 

		// if(GUILayout.Button("dalete")){
		// 	raftParent.TransactionDelete();
		// }
	}
}
