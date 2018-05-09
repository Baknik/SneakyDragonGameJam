using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : TitleButton {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void OnClicked()
	{
		Debug.Log("Opening Play Scene...");
		SceneManager.LoadScene("FirstLevel");
	}
}
