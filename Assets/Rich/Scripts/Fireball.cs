using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {

	[Header("Settings")]
	public float Speed;

	[Header("Runtime")]
	public Vector2 Direction;

	private Rigidbody2D Rigidbody2D;

	private void Awake() {
		this.Rigidbody2D = this.GetComponent<Rigidbody2D>();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		this.Rigidbody2D.velocity = this.Direction.normalized * this.Speed;
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "World" || other.tag == "Enemy")
		{
			GameObject.Destroy(this.gameObject);
		}
	}
}
