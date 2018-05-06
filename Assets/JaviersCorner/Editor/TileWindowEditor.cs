using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(TileWindow))]
public class TileWindowEditor : Editor {



	void OnSceneGUI(){
		Debug.Log("hapening");
	}

}
