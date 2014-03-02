using UnityEngine;
using System.Collections;

 class ButtonOptions : Button {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	
	internal override void ButtonAction()
	{
		// If pointer to the menu/etc exists, then we enable it
		if(MenuToOpen)
		{
			// todo: to be removed and shifted to it's own logic class code
			//MainMenuManager.Instance.SetAllChildButtonsInput(false);
			MenuToOpen.SetActive(true);
			// Hide/DisableClick all other buttons
			EnableAllPauseButtonsInputs(false);
		}
	}


}
