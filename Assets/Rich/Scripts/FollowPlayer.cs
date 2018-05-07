using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

	[Header("Settings")]
	public float FollowSpeed;

	[Header("References")]
	public Player Player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void LateUpdate() {
		Vector3 currentVelocity = Vector3.zero;
		Vector3 newPosition = Vector3.SmoothDamp(this.transform.position, this.Player.transform.position, ref currentVelocity, Time.fixedDeltaTime * this.FollowSpeed);
		newPosition = new Vector3(newPosition.x, newPosition.y, -10f);
		this.transform.position = newPosition;
	}
}
