using UnityEngine;
using System.Collections;

class ButtonRestart : Button {
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	new void Update () {
		base.Update();
		
	}
	
	internal override void ButtonAction()
	{
		// Without prompt, reload the level
		Application.LoadLevel(Application.loadedLevel);
		//if(MenuToOpen)
		{
			//EnableAllPauseButtonsInputs(false, false);
			//transform.parent.gameObject.SetActive(false);
		}
	}
}
