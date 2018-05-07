using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {

	[Header("Settings")]
	public float Speed;
	public float Damage;

	[Header("Runtime")]
	public Vector2 Direction;

	private Rigidbody2D Rigidbody2D;
	private SpriteRenderer SpriteRenderer;

	private void Awake() {
		this.Rigidbody2D = this.GetComponent<Rigidbody2D>();
		this.SpriteRenderer = this.GetComponent<SpriteRenderer>();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// Sorting
		this.SpriteRenderer.sortingOrder = (int)((this.transform.position.y + 2f) * -1f);

		this.Rigidbody2D.velocity = this.Direction.normalized * this.Speed;
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.tag != "Player")
		{
			GameObject.Destroy(this.gameObject);
		}
		if (other.tag == "Enemy")
		{
			Guard guard = other.GetComponent<Guard>();
			if (guard)
			{
				guard.Hit(this.Damage);
			}
		}
	}
}
