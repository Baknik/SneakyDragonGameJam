using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour {

	[Header("Settings")]
	public float HorzMovementSpeed;
	public float VertMovementSpeed;
	public float AgroDistance;
	public float Damage;
	public float MaxHealth;

	[Header("References")]
	public Transform HealthBar;
	public Transform HealthBarFill;

	[Header("Runtime")]
	public float CurrentHealth;

	private GuardSwipeArea GuardSwipeArea;

	private Rigidbody2D Rigidbody2D;
	private Animator Animator;
	private SpriteRenderer SpriteRenderer;
	private float horzMoveInput;
	private float vertMoveInput;
	private bool attacking;
	private bool noticedPlayer;

	private Player Player;

	private void Awake() {
		this.Rigidbody2D = this.GetComponent<Rigidbody2D>();
		this.Animator = this.GetComponent<Animator>();
		this.SpriteRenderer = this.GetComponent<SpriteRenderer>();

		this.GuardSwipeArea = this.GetComponentInChildren<GuardSwipeArea>();

		this.Player = GameObject.FindObjectOfType<Player>();
	}

	// Use this for initialization
	void Start () {
		this.Animator.SetFloat("Horz Movement", 0f);
		this.Animator.SetFloat("Vert Movement", 0f);

		this.attacking = false;
		this.noticedPlayer = false;

		this.CurrentHealth = this.MaxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		// Sorting
		this.SpriteRenderer.sortingOrder = (int)((this.transform.position.y + 2f) * -1f);

		// Find direction to player
		Vector3 dirToPlayer = (this.Player.transform.position - this.transform.position).normalized;

		this.horzMoveInput = dirToPlayer.x * this.HorzMovementSpeed;
		this.vertMoveInput = dirToPlayer.y * this.VertMovementSpeed;
		this.Animator.SetFloat("Horz Movement", (Mathf.Abs(this.horzMoveInput) > Mathf.Abs(this.vertMoveInput)) ? this.horzMoveInput : 0f);
		this.Animator.SetFloat("Vert Movement", (Mathf.Abs(this.vertMoveInput) > Mathf.Abs(this.horzMoveInput)) ? this.vertMoveInput : 0f);

		Vector2 playerDirection = (this.Player.transform.position - this.transform.position).normalized;
		this.Animator.SetFloat("Attack Direction X", (Mathf.Abs(playerDirection.x) > Mathf.Abs(playerDirection.y)) ? playerDirection.x : 0f);
		this.Animator.SetFloat("Attack Direction Y", (Mathf.Abs(playerDirection.y) > Mathf.Abs(playerDirection.x)) ? playerDirection.y : 0f);

		float distanceToPlayer = Vector3.Distance(this.Player.transform.position, this.transform.position);
		this.Animator.SetFloat("Distance To Player", distanceToPlayer);
		if (distanceToPlayer <= this.AgroDistance)
		{
			this.Animator.SetTrigger("Agro");
			this.noticedPlayer = true;
		}

		// Update health bar
		this.HealthBar.gameObject.SetActive(this.CurrentHealth < this.MaxHealth);
		this.HealthBarFill.localScale = new Vector3(this.CurrentHealth / this.MaxHealth, 1f, 1f);
	}

	void FixedUpdate() {
		if (this.noticedPlayer && !this.attacking)
		{
			this.Rigidbody2D.velocity = new Vector2(this.horzMoveInput, this.vertMoveInput);
		}
	}

	public void StartAttack(float angle)
	{
		this.GuardSwipeArea.transform.parent.localRotation = Quaternion.Euler(0f, 0f, angle);
		this.GuardSwipeArea.gameObject.SetActive(true);
		this.attacking = true;
	}

	public void EnableHitbox()
	{
		this.GuardSwipeArea.HitboxActive = true;
	}

	public void DisableHitbox()
	{
		this.GuardSwipeArea.HitboxActive = false;
	}

	public void EndAttack()
	{
		this.GuardSwipeArea.gameObject.SetActive(false);
		this.attacking = false;
	}

	public void Hit(float damage)
	{
		this.CurrentHealth -= damage;
		this.CurrentHealth = Mathf.Clamp(this.CurrentHealth, 0f, this.MaxHealth);

		// Agro when hit
		this.Animator.SetTrigger("Agro");
		this.noticedPlayer = true;

		if (this.CurrentHealth <= 0f)
		{
			GameObject.Destroy(this.gameObject);
		}
	}
}
