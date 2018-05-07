using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardSwipeArea : MonoBehaviour {

	[Header("Runtime")]
	public bool HitboxActive;

	private Guard Guard;
	public SpriteRenderer SpriteRenderer;

	private void Awake() {
		this.Guard = this.GetComponentInParent<Guard>();
		this.SpriteRenderer = this.GetComponent<SpriteRenderer>();
	}

	// Use this for initialization
	void Start () {
		this.HitboxActive = false;
		this.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player")
		{
			Player player = other.gameObject.GetComponent<Player>();
			if (player)
			{
				player.Damage(this.Guard.Damage);
			}
		}
	}
}
