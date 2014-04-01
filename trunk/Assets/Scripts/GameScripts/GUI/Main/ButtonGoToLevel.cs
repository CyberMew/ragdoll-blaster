using UnityEngine;
using System.Collections;

class ButtonGoToLevel : Button {

	public string LevelToLoad = "Credits";

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	new void Update () {
		base.Update();
	
	}


	internal override void ButtonAction()
	{
		if(LevelToLoad.Length == 0)
		{
			Application.LoadLevel(Application.loadedLevel);
		}
		else
		{
			Application.LoadLevel(LevelToLoad);
		}
		//transform.GetChild(0).gameObject.SetActive(true);	// Suppose this should be slower, albeit more dynamic
		if(MenuToOpen)
		{
		//	MainMenuManager.Instance.ActiveButtons(false);
		//	MenuToOpen.SetActive(true);
		}
	}
}
