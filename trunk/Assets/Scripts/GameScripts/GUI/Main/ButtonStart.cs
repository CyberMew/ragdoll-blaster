using UnityEngine;
using System.Collections;

 class ButtonStart : Button {

	// Use this for initialization
	void Start () {
	


	}
	
	// Update is called once per frame
	new void Update () {
		base.Update();
	
	}

	internal override void ButtonAction()
	{
		MainMenuManager.Instance.SetMainMenuState(MainMenuManager.MainMenuState.Start);
	}


}
