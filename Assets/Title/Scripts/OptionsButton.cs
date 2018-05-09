using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsButton : TitleButton {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void OnClicked()
	{
		Debug.Log("Opening Options Menu...");
	}
}
