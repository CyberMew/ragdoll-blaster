using UnityEngine;
using System.Collections;

class ButtonGoToLevel : Button {

	public string LevelToLoad = "Credits";

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}


	internal override void ButtonAction()
	{
		Application.LoadLevel(LevelToLoad);
		//transform.GetChild(0).gameObject.SetActive(true);	// Suppose this should be slower, albeit more dynamic
		if(MenuToOpen)
		{
		//	MainMenuManager.Instance.ActiveButtons(false);
		//	MenuToOpen.SetActive(true);
		}
	}
}
