using UnityEngine;
using System.Collections;

class ButtonCredits : Button {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	new void Update () {
		base.Update();

	}

	internal override void ButtonAction()
	{
		//transform.GetChild(0).gameObject.SetActive(true);	// Suppose this should be slower, albeit more dynamic
		if(MenuToOpen)
		{
			//todo: remove this and move to another class specific for mainmenu behaviours
			//if(GameManager.currLevel == -1);
			//	MainMenuManager.Instance.SetAllChildButtonsInput(false);
			MenuToOpen.SetActive(true);
			EnableAllPauseButtonsInputs(false, false);
		}
	}
}
