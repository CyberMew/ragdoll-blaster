using UnityEngine;
using System.Collections;

class CloseMenuButton : Button {

	public bool TapAnywhereToClose = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(TapAnywhereToClose && InputManager.GetIsInputDown())
		{
			ButtonAction();
		}
		// Due to Unity bug(?) I have to workaround the problem manually
		if(InputManager.GetIsInputDown() && collider2D.OverlapPoint(Camera.main.ScreenToWorldPoint(InputManager.GetCurrentPositionScreenSpace())))
		{
		//	ButtonAction();
		}
	}
	
	internal override void ButtonAction()
	{
		gameObject.transform.parent.gameObject.SetActive(false);

	}

	/*
	void OnMouseUpAsButton()
	{
		// Set my parent game object to disabled
		gameObject.transform.parent.gameObject.SetActive(false);

		Debug.Log("Closing");
	}*/
}
