using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballBall : MonoBehaviour {

	[Header("Settings")]
	public float SpinSpeed;

	private SpriteRenderer SpriteRenderer;

	private void Awake() {
		this.SpriteRenderer = this.GetComponent<SpriteRenderer>();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// Sorting
		this.SpriteRenderer.sortingOrder = (int)((this.transform.position.y + 2f) * -1f);

		this.transform.Rotate(Vector3.forward, this.SpinSpeed * Time.deltaTime);
	}
}
