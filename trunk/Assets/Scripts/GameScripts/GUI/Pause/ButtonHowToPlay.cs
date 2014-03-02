using UnityEngine;
using System.Collections;

class ButtonHowToPlay: Button {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	internal override void ButtonAction()
	{
		//transform.GetChild(0).gameObject.SetActive(true);	// Suppose this should be slower, albeit more dynamic
		if(MenuToOpen)
		{
			MenuToOpen.SetActive(true);
			
			EnableAllPauseButtonsInputs(false);
		}
	}
}
