using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Player : MonoBehaviour {

	[Header("Settings")]
	public float HorzMovementSpeed;
	public float VertMovementSpeed;
	public Vector2 FireballStartPositionOffset;
	public float MaxMana;
	public float CastManaCost;
	public float MaxDrunkiness;
	public float SoberingRate;
	public float DrinkDrunkiness;
	public float FireballDrunkinessOffset;
	public float MaxHealth;

	[Header("Prefabs")]
	public Fireball FireballPrefab;

	[Header("References")]
	public Slider ManaSlider;
	public Slider DrunkinessSlider;
	public Slider HealthSlider;
	public Transform EndGamePanel;

	[Header("Runtime")]
	public float CurrentMana;
	public float CurrentDrunkiness;
	public float CurrentHealth;

	private Rigidbody2D Rigidbody2D;
	private Animator Animator;
	private SpriteRenderer SpriteRenderer;
	private float horzMoveInput;
	private float vertMoveInput;
	private bool castingFireball;
	private bool castInput;
	private bool drinkingPotion;

	private void Awake() {
		this.Rigidbody2D = this.GetComponent<Rigidbody2D>();
		this.Animator = this.GetComponent<Animator>();
		this.SpriteRenderer = this.GetComponent<SpriteRenderer>();
	}

	// Use this for initialization
	void Start () {
		Time.timeScale = 1f;

		this.Animator.SetFloat("Horz Movement", 0f);
		this.Animator.SetFloat("Vert Movement", 0f);
		this.Animator.SetFloat("Casting Direction X", 0f);
		this.Animator.SetFloat("Casting Direction Y", 0f);
		this.Animator.SetBool("Casting", false);

		this.castingFireball = false;
		this.drinkingPotion = false;

		this.CurrentMana = this.MaxMana;
		this.CurrentDrunkiness = 0f;
		this.CurrentHealth = this.MaxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		// Sorting
		this.SpriteRenderer.sortingOrder = (int)((this.transform.position.y + 2f) * -1f);
		
		// Sobering
		this.CurrentDrunkiness -= this.SoberingRate * Time.deltaTime;
		this.CurrentDrunkiness = Mathf.Clamp(this.CurrentDrunkiness, 0f, this.MaxDrunkiness);

		this.horzMoveInput = Input.GetAxis("Horizontal") * this.HorzMovementSpeed;
		this.vertMoveInput = Input.GetAxis("Vertical") * this.VertMovementSpeed;
		this.Animator.SetFloat("Horz Movement", this.horzMoveInput);
		this.Animator.SetFloat("Vert Movement", this.vertMoveInput);
		// this.transform.position = Vector3.Lerp(this.transform.position, this.transform.position + new Vector3(horzMovement, vertMovement, 0f), Time.deltaTime);

		if (Input.GetAxis("Fire1") > 0f && this.CurrentMana >= this.CastManaCost)
		{
			if (!this.castingFireball)
			{
				this.CurrentMana -= this.CastManaCost;
				this.castingFireball = true;
				this.Animator.SetTrigger("Cast Input");
				this.Animator.SetBool("Casting", this.castingFireball);
				Fireball newFireball = Instantiate(this.FireballPrefab, this.transform.position + (Vector3)this.FireballStartPositionOffset, Quaternion.identity);
				Vector2 fireballCastDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - newFireball.transform.position).normalized;
				fireballCastDirection = Quaternion.Euler(0, 0, (UnityEngine.Random.value - 0.5f) * (this.CurrentDrunkiness * this.FireballDrunkinessOffset)) * fireballCastDirection;
				newFireball.Direction = fireballCastDirection;
				this.Animator.SetFloat("Cast Direction X", (Mathf.Abs(newFireball.Direction.x) > Mathf.Abs(newFireball.Direction.y)) ? newFireball.Direction.x : 0f);
				this.Animator.SetFloat("Cast Direction Y", (Mathf.Abs(newFireball.Direction.y) > Mathf.Abs(newFireball.Direction.x)) ? newFireball.Direction.y : 0f);
			}
		}
		else
		{
			this.castingFireball = false;
			this.Animator.SetBool("Casting", this.castingFireball);
			this.Animator.SetFloat("Cast Direction X", 0f);
			this.Animator.SetFloat("Cast Direction Y", 0f);
			// this.Animator.ResetTrigger("Cast Input");
		}

		// Drink potion
		if (Input.GetAxis("Jump") > 0f)
		{
			if (!this.drinkingPotion)
			{
				this.drinkingPotion = true;
				this.CurrentMana = this.MaxMana;
				this.CurrentDrunkiness += this.DrinkDrunkiness;
				this.CurrentDrunkiness = Mathf.Clamp(this.CurrentDrunkiness, 0f, this.MaxDrunkiness);
			}
		}
		else
		{
			this.drinkingPotion = false;
		}

		// Update mana slider
		this.ManaSlider.value = this.CurrentMana / this.MaxMana;

		// Update drunkiness slider
		this.DrunkinessSlider.value = this.CurrentDrunkiness / this.MaxDrunkiness;

		// Update health slider
		this.HealthSlider.value = this.CurrentHealth / this.MaxHealth;
	}

	void FixedUpdate() {
		if (this.castingFireball)
		{
			this.Rigidbody2D.velocity = Vector2.zero;
		}
		else
		{
			this.Rigidbody2D.velocity = new Vector2(this.horzMoveInput, this.vertMoveInput);
		}
	}

	public void Damage(float amount)
	{
		this.CurrentHealth -= amount;
		this.CurrentHealth = Mathf.Clamp(this.CurrentHealth, 0f, this.MaxHealth);
		if (this.CurrentHealth <= 0f)
		{
			Sequence endGameSequence = DOTween.Sequence();
			endGameSequence.Append(this.EndGamePanel.DOScale(new Vector3(1f, 1f, 1f), 0.5f).SetEase(Ease.OutBack));
			endGameSequence.AppendCallback(StopTime);
			endGameSequence.Play();
		}
	}

	public void StopTime()
	{
		Time.timeScale = 0f;
	}
}
