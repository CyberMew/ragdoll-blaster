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
		if(ButtonMenuLogic)
		{
			MainMenuManager.Instance.SetAllChildButtonsInput(false);
			ButtonMenuLogic.SetActive(true);
		}
	}


}
