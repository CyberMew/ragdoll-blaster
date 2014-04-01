using UnityEngine;
using System.Collections;

class OptionBack : Button {

	// Use this for initialization
	private GameObject opWindow;
	void Start () {
		opWindow = this.transform.parent.gameObject;
	}
	
	// Update is called once per frame
	new void Update () {
		base.Update();
	
	}

	internal override void ButtonAction()
	{
		
		if(opWindow)
		{
			if(opWindow)
			{
				LeanTween.moveLocalY(opWindow, 10f, 0.5f)
					.setEase( LeanTweenType.easeInOutExpo);
			}
			
			Invoke("ShowMainMenu", 0.5f);
		}
	



	}

	public void ShowMainMenu()
	{
		if(!GameManager.isInGame)
			MainMenuManager.Instance.SetMainMenuState(MainMenuManager.MainMenuState.ShowMenu);

			
	}



}
