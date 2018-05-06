using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

 [CustomEditor(typeof(Tile))]
 [CanEditMultipleObjects]
public class TileEditor : Editor {

	public static bool editing = false;

	private Tile targetTile;

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
			case EventType.MouseDown:

				// if()

			break;
			case EventType.MouseUp:

			break;
			case EventType.MouseDrag:

				

				if( e.button == 0 ){
					Vector3 mousePosition = e.mousePosition;
					Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);
					Plane p = new Plane( -Vector3.forward, Vector3.zero );
					float rayDist;
					if( p.Raycast( ray, out rayDist ) ){
						mousePosition = ray.GetPoint( rayDist );
					}

					targetTile = (Tile)target;

					VectorT newPos = VectorT.WorldToGrid( mousePosition );
					newPos.z = targetTile.gPosition.z;
					targetTile.SetPosition( newPos );

					// mousePosition 
				}

			break;
		}

	}
	
}

