using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class TileWindow : EditorWindow {

	// static public bool editing = false;

	float offset;
	float offsetRange;
	bool addTiles;

	List<GameObject> prefabs = new List<GameObject>();
	GameObject prefab;

	bool selectionSet = false;
	VectorT selectionStart;
	VectorT selectionEnd;



    [MenuItem ("SneakyDragon/TileWindow")]
    public static void  ShowWindow () {
        EditorWindow.GetWindow(typeof(TileWindow));
    }

    void OnGUI () {

        if(GUILayout.Button("CleanUpTiles"))
        {
			Tile[] tiles = FindObjectsOfType<Tile>();
			
			for( int i = 0; i < tiles.Length; i++ ){
				for( int o = 0; o < tiles.Length; o++ ){
					if(( tiles[i] != null && tiles[o] != null) && tiles[i] != tiles[o] && tiles[i].gPosition.isEqualTo( tiles[o].gPosition ) ){
						DestroyImmediate( tiles[o].gameObject );
					}
				}
			}
        }

	
		offset = EditorGUILayout.FloatField("Offset", offset );
		offsetRange = EditorGUILayout.FloatField("OffsetRange", offsetRange );
		

		if(GUILayout.Button("Add Random Offset")){
			Tile tile;
			VectorT gpos;
			for( int i = 0; i < Selection.gameObjects.Length; i++ ){
				tile = Selection.gameObjects[i].GetComponent<Tile>();
				if( tile != null ){
					gpos = tile.gPosition;
					gpos.z = offset + Random.Range(-offsetRange, offsetRange);
					tile.SetPosition(gpos);
				}
				tile = null;
			}

		}

		if( addTiles == false && GUILayout.Button("AddTiles") ){
			addTiles = true;
			Tools.current = Tool.None;
			SceneView.onSceneGUIDelegate += OnSceneGUI;
		}

		prefab = (GameObject)EditorGUILayout.ObjectField( null, typeof( GameObject ), false );

		if( prefab != null ){
			prefabs.Add( prefab );
			prefab = null;
		}
		for( int i = 0; i < prefabs.Count; i++ ){
			prefabs[i] = (GameObject)EditorGUILayout.ObjectField( prefabs[i], typeof( GameObject ), false );
			if( prefabs[i] == null ){
				prefabs.RemoveAt( i );
				i--;
			}
		}
		

    }

	public void OnSceneGUI(SceneView sceneView){

		if( Tools.current != Tool.None && addTiles == true ){
			addTiles = false;
			SceneView.onSceneGUIDelegate -= OnSceneGUI;
		}

		HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

		Event e = Event.current;
		
		switch( e.type ){
			case EventType.MouseDown:
				if(e.button != 0){
					break;
				}

				selectionStart = VectorT.WorldToGrid( SceneViewToWorld( e.mousePosition ) );
				selectionSet = true;
				selectionEnd = selectionStart;
				
			break;
			case EventType.MouseDrag:
				if(e.button != 0){
					break;
				}
				selectionEnd = VectorT.WorldToGrid( SceneViewToWorld( e.mousePosition ) );
			break;
			case EventType.MouseUp:
				if(e.button != 0){
					break;
				}
				if( selectionSet == true ){
					selectionEnd = VectorT.WorldToGrid( SceneViewToWorld( e.mousePosition ) );
					AddTiles();
					selectionSet = false;
				}
			break;

		}

		if( selectionSet == false ){
			return;
		}

		Vector3[] verts = new Vector3[4];

		verts[0] = selectionStart.ToWorld();
		verts[1] = new VectorT(selectionStart.x, selectionEnd.y).ToWorld();
		verts[2] = selectionEnd.ToWorld();
		verts[3] = new VectorT(selectionEnd.x, selectionStart.y).ToWorld();
		
		Handles.DrawSolidRectangleWithOutline( verts, new Color(0,0.25f,1,0.25f), new Color(0,0,0.25f,0.5f) );
		HandleUtility.Repaint();

	}

	void AddTiles(){

		int temp;
		if( selectionStart.x > selectionEnd.x ){
			/// flip x
			temp = selectionStart.x;
			selectionStart.x = selectionEnd.x;
			selectionEnd.x = temp;
		}
		if( selectionStart.y > selectionEnd.y ){
			/// flip x
			temp = selectionStart.y;
			selectionStart.y = selectionEnd.y;
			selectionEnd.y = temp;
		}

		GameObject go;
		Tile goTile;

		for( int i = selectionStart.x; i < selectionEnd.x; i++ ){

			for( int o = selectionStart.y; o < selectionEnd.y; o++ ){

				int prefabIndex = Random.Range( 0, prefabs.Count );

				go = Instantiate( prefabs[prefabIndex] );
				goTile = go.GetComponent<Tile>();
				if( goTile != null ){
					
					goTile.SetPosition( new VectorT( i, o, offset + Random.Range(-offsetRange, offsetRange) ) );
				} else {
					go.transform.position = new VectorT( i, o, offset + Random.Range(-offsetRange, offsetRange) ).ToWorld();
				}

			}

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

	void Update(){


		if( Tools.current != Tool.None && addTiles == true ){
			addTiles = false;
			SceneView.onSceneGUIDelegate -= OnSceneGUI;
		}

	}
}
