using UnityEngine;
using System.Collections;

class ConfirmationYesButton : Button {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	new void Update () {
		base.Update();
	}
	
	internal override void ButtonAction()
	{
		GameManager.isInGame = false;
		GameManager.LoadLevel("MainMenu");
	}
}
