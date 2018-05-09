using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(AudioSource))]
public class TitleButton : MonoBehaviour {

	[Header("Settings")]
	public float HoverScale;

	[Header("Resources")]
	public AudioClip ButtonSelectSound;

	private AudioSource AudioSource;

	private void Awake() {
		this.AudioSource = this.GetComponent<AudioSource>();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnPointerEnter() {
		this.AudioSource.PlayOneShot(this.ButtonSelectSound);
		this.transform.DOScale(this.HoverScale, 0.1f);
	}

	public void OnPointerExit() {
		this.transform.DOScale(1f, 0.25f);
	}

	public virtual void OnClicked()
	{
		
	}
}
