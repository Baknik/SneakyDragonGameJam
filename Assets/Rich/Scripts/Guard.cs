using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour {

	[Header("Settings")]
	public float HorzMovementSpeed;
	public float VertMovementSpeed;
	public float EngagementDistance;

	private Rigidbody2D Rigidbody2D;
	private Animator Animator;
	private float horzMoveInput;
	private float vertMoveInput;

	private Player Player;

	private void Awake() {
		this.Rigidbody2D = this.GetComponent<Rigidbody2D>();
		this.Animator = this.GetComponent<Animator>();

		this.Player = GameObject.FindObjectOfType<Player>();
	}

	// Use this for initialization
	void Start () {
		this.Animator.SetFloat("Horz Movement", 0f);
		this.Animator.SetFloat("Vert Movement", 0f);
	}
	
	// Update is called once per frame
	void Update () {
		// Find direction to player
		Vector3 dirToPlayer = (this.Player.transform.position - this.transform.position).normalized;

		this.horzMoveInput = dirToPlayer.x * this.HorzMovementSpeed;
		this.vertMoveInput = dirToPlayer.y * this.VertMovementSpeed;
		this.Animator.SetFloat("Horz Movement", this.horzMoveInput);
		this.Animator.SetFloat("Vert Movement", this.vertMoveInput);
	}

	void FixedUpdate() {
		this.Rigidbody2D.velocity = new Vector2(this.horzMoveInput, this.vertMoveInput);
	}
}
