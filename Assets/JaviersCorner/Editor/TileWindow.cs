using UnityEngine;
using UnityEditor;
using System.Collections;

public class TileWindow : EditorWindow {

	// static public bool editing = false;

    [MenuItem ("SneakyDragon/TileWindow")]
    public static void  ShowWindow () {
        EditorWindow.GetWindow(typeof(TileWindow));
    }
    
    void OnGUI () {
        // The actual window code goes here

		// DrawDefaultInspector();
        
		// GUILayout.Toggle( editing, "Edit" );
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
    }
}
