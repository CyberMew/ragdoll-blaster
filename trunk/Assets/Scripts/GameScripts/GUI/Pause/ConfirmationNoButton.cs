using UnityEngine;
using System.Collections;

class ConfirmationNoButton : Button {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

	
	internal override void ButtonAction()
	{
		// If pointer to the menu/etc exists, then we want to close the whole menu disable the whole thing
		//if(ButtonMenuLogic)
		{
			// todo: to be removed and shifted to it's own logic class code
			//MainMenuManager.Instance.SetAllChildButtonsInput(false);
			//ButtonMenuLogic.SetActive(true);
			// Disable/Hide all other buttons
			//EnableAllPauseButtonsInputs(true);
		}

		// Want to disable it's parent (aka current menu)
		gameObject.transform.parent.gameObject.SetActive(false);
	}

}
