using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public struct opacityPoint {

	public Vector3 position;

	public float distance;

}

public class FadeOnNear : MonoBehaviour {

	public float distance = 10;

	public opacityPoint[] points;

	public SpriteRenderer[] spriteRenderers;

	private Player player;

	private Tween tween;

	private float opacity;

	private bool fadedOut = false;

	// Use this for initialization
	void Start () {

		player = FindObjectOfType<Player>();
		
	}
	
	// Update is called once per frame
	void Update () {

		bool withinRange = false;
		for( int i = 0; i < points.Length; i++ ){

			if( Vector3.Distance( player.transform.position, this.transform.TransformPoint( points[i].position ) ) < points[i].distance ){
				withinRange = true;
				break;
			}

		}

		if( fadedOut != withinRange ){
			fadedOut = withinRange;
			if( withinRange == true ){
				tween.Kill();
				tween = DOTween.To( delegate(){ return opacity; }, delegate( float x ){ opacity = x; } , 0, 0.5f );
			} else {
				tween.Kill();
				tween = DOTween.To( delegate(){ return opacity; }, delegate( float x ){ opacity = x; } , 1, 0.5f );
			}
		}

		Color tempCol;

		for( int i = 0; i < spriteRenderers.Length; i++ ){

			tempCol = spriteRenderers[i].color;

			tempCol.a = opacity;

			spriteRenderers[i].color = tempCol;

		}

	}

	void OnDrawGizmos(){

		Gizmos.color = new Color(0,0.75f,0.25f,0.25f);

		for( int i = 0; i < points.Length; i++ ){

			Gizmos.DrawSphere( this.transform.TransformPoint( points[i].position ), points[i].distance );

		}

	}

}
