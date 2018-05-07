using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Sort : MonoBehaviour {

	private SpriteRenderer SpriteRenderer;

	private void Awake() {
		this.SpriteRenderer = this.GetComponent<SpriteRenderer>();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		this.SpriteRenderer.sortingOrder = (int)((this.transform.position.y + 2f) * -1f);
	}
}
