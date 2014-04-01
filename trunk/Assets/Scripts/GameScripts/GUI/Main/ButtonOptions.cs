using UnityEngine;
using System.Collections;

 class ButtonOptions : Button {

	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	new void Update () {
		base.Update();


	
	}

	
	internal override void ButtonAction()
	{
		// If pointer to the menu/etc exists, then we enable it
		//if(MenuToOpen)
		{
			MainMenuManager.Instance.SetMainMenuState(MainMenuManager.MainMenuState.Options);
		}
	}



}
