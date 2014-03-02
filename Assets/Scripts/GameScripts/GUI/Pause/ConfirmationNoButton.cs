using UnityEngine;
using System.Collections;

class ConfirmationNoButton : Button {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	//GameManager.isInGame 
	}

	
	internal override void ButtonAction()
	{
		// If pointer to the menu/etc exists, then we enable it
		if(ButtonMenuLogic)
		{
			// todo: to be removed and shifted to it's own logic class code
			//MainMenuManager.Instance.SetAllChildButtonsInput(false);
			ButtonMenuLogic.SetActive(true);
			// Disable/Hide all other buttons
			EnableAllPauseButtonsInputs(false);
		}
	}

}
