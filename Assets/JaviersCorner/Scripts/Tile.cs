using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;


// [ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
public class Tile : MonoBehaviour {

	public SpriteRenderer sr;

	public VectorT gPosition = new VectorT();

	// Use this for initialization
	void Start () {
		sr = GetComponent<SpriteRenderer>();
	}
	

	void OnValidate(){

		SetPosition( gPosition );

	}
	
	
	void Update () {

	}

	public void SetPosition( VectorT vt ){

		gPosition = vt;

		this.transform.localPosition = vt.ToWorld();

		sr.sortingOrder = (int)Mathf.Round( -this.transform.localPosition.y + gPosition.z );

	}


	// void SnapToGrid(){

	// 	float yPos = Mathf.Round( this.transform.localPosition.y - gPosition.z );
	// 	float xPos = Mathf.Round( this.transform.localPosition.x /4 ) * 4;
	// 	if( yPos % 2 == 0 ){
	// 		/// Evens
	// 		if( xPos % 2 != 0 ){
	// 			if( this.transform.localPosition.x > xPos ){
	// 				xPos += 1;
	// 			} else {
	// 				xPos -= 1;
	// 			}
	// 		}
	// 	} else {
	// 		/// Odds
	// 		if( xPos % 2 == 0 ){
	// 			if( this.transform.localPosition.x > xPos ){
	// 				xPos += 2;
	// 			} else {
	// 				xPos -= 2;
	// 			}
	// 		}
	// 	}
	// 	// this.transform.localPosition = new Vector3( xPos, yPos + gPosition.z,  this.transform.localPosition.z );
	// 	this.transform.localPosition = new Vector3( xPos, yPos + gPosition.z,  gPosition.z * -0.001f );

	// 	VectorT gp = VectorT.WorldToGrid( this.transform.localPosition );
	// 	gPosition.x = gp.x;
	// 	gPosition.y = gp.y;

	// 	sr.sortingOrder = (int)Mathf.Round( -this.transform.localPosition.y + gPosition.z );

	// }




	void OnDrawGizmos(){

		if( !Selection.Contains(this.gameObject) || TileEditor.editing == false ){
			return;
		}

		// if( !( Selection.Contains(this.gameObject) && Tools.current == Tool.Move ) ){
		// 	return;
		// }

		Gizmos.color = new Color(0.2f,0.5f,1,0.15f);

		int drawRange = 2;

		VectorT basePos = gPosition;

		basePos.z = 0;

		for( int u = -drawRange * 2; u < drawRange * 2; u++ ){

			for( int v = -drawRange * 2; v < drawRange * 2; v++){

				Gizmos.DrawLine( new VectorT( u + basePos.x, v + basePos.y ).ToWorld() ,new VectorT( u + basePos.x, -v + basePos.y ).ToWorld() );
				Gizmos.DrawLine( new VectorT( u + basePos.x, v + basePos.y ).ToWorld() , new VectorT( -u + basePos.x, v + basePos.y ).ToWorld() );

			}
		}

		// // Gizmos.Draw
		Handles.color = Handles.xAxisColor;
		Handles.ArrowHandleCap(0, basePos.ToWorld(), Quaternion.Euler(27,90,0), 2, EventType.Repaint);
		Handles.color = Handles.yAxisColor;
		Handles.ArrowHandleCap(0, basePos.ToWorld(), Quaternion.Euler(27,-90,0), 2, EventType.Repaint);
		Handles.color = Handles.zAxisColor;
		Handles.ArrowHandleCap(0, basePos.ToWorld(), Quaternion.Euler(-90,0,0), 2, EventType.Repaint);

	}

	
}

[System.Serializable]
public struct VectorT {

	public int x;
	public int y;
	public float z;

	public bool isEqualTo( VectorT t ){

		if( this.x == t.x && this.y == t.y && this.z == t.z ){
			return true;
		} else {
			return false;
		}

	}

	// public VectorT (){
	// 	this.x = 0;
	// 	this.y = 0;
	// 	this.z = 0;
	// }
	public VectorT ( int x, int y ){
		this.x = x;
		this.y = y;
		this.z = 0;
	}
	public VectorT ( int x, int y, float z ){
		this.x = x;
		this.y = y;
		this.z = z;
	}

	public Vector3 ToWorld(){

		Vector2 xp = this.x * new Vector2(2, 1);
		Vector2 yp = this.y * new Vector2(-2, 1);

		Vector3 world = xp + yp;

		world.y += this.z;

		world.z = this.z * -0.001f;

		return world;

	}

	public static Vector3 GridToWorld( VectorT gridPos ){

		Vector3 wPos = new Vector3();

		/// ( gp.x * 2 ) + ( gp.y * -2 ) = x
		/// gp.x + gp.y = y

		Vector2 xp = gridPos.x * new Vector2(2, 1);
		Vector2 yp = gridPos.y * new Vector2(-2, 1);

		wPos = xp + yp;

		wPos.y += gridPos.z;

		wPos.z = gridPos.z * -0.001f;

		return wPos;

	}
	
	public static VectorT WorldToGrid( Vector3 wPos ){

		VectorT gPos = new VectorT();
		/// ( gp.x * 2 ) + ( gp.y * -2 ) = x
		/// gp.x + gp.y = y

		// // / gp.x * 2 = x - (gp.y * -2 )
		// // / gp.x = ( x - (gp.y * -2 )) / 2

		/// gp.y = y - gp.x

		/// substitute!!!

		///  ( gp.x * 2 ) + ( (y - gp.x) * -2 ) = x
		/// gp.x + ( (y - gp.x) * -2 ) / 2 = x / 2
		/// gp.x - (y - gp.x) = x / 2
		/// (gp.x * 2) - y = x / 2
		/// gp.x * 2 = (x / 2) + y
		/// gp.x = ((x / 2) + y ) / 2

		gPos.x = (int)Mathf.Round( ((wPos.x / 2) + wPos.y ) / 2 );
		gPos.y =  (int)Mathf.Round(wPos.y - gPos.x);

		return gPos;

	}

}