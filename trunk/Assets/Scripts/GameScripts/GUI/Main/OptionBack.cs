using UnityEngine;
using System.Collections;

class OptionBack : Button {

	// Use this for initialization
	private GameObject opWindow;
	void Start () {
		opWindow = this.transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	internal override void ButtonAction()
	{
		
		if(opWindow)
		{
			//MainMenuManager.Instance.MM_STATES = MainMenuManager.MainMenuState.GoBack;

			LeanTween.moveLocalY(opWindow, 1f, 0.5f)
				.setEase( LeanTweenType.easeInOutExpo);

			Invoke("ShowMainMenu", 0.5f);
		}
	



	}

	public void ShowMainMenu()
	{
		if(opWindow)
			opWindow.SetActive(false);
		MainMenuManager.Instance.ActiveButtons(true);
	}



}
