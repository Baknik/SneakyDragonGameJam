using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

 [CustomEditor(typeof(Tile))]
 [CanEditMultipleObjects]
public class TileEditor : Editor {

	public static bool editing = false;

	private Tile targetTile;
	private Tile[] targetTiles;

	private Vector3 initMousePos;

	string initTargetName;
	string[] initNames;

	void OnSceneGUI(){

		Event e = Event.current;

		if( e.type == EventType.KeyDown ){

			if( e.keyCode == KeyCode.G ){
				editing = true;
				Tools.current = Tool.None;
			}

		}

		if( Tools.current != Tool.None ){
			editing = false;
		}
		

		if( editing == false ){
			return;
		}

		HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

		switch( e.type ){
			// case EventType.
			case EventType.MouseDrag:

				
				if( e.button == 0 ){

					targetTile = (Tile)target;

					VectorT newPos = VectorT.WorldToGrid( SceneViewToWorld( e.mousePosition ) );
					newPos.z = targetTile.gPosition.z;
					targetTile.SetPosition( newPos );

				}

			break;
		}

	}

	Vector3 SceneViewToWorld( Vector2 mousePos, float z = 0){

		Plane plane = new Plane( -Vector3.forward, Vector3.forward * z );
		float rayDist;

		Vector3 mousePosition = mousePos;
		Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);
		if( plane.Raycast( ray, out rayDist ) ){
			mousePosition = ray.GetPoint( rayDist );
		}

		return mousePosition;

	}
	
}

