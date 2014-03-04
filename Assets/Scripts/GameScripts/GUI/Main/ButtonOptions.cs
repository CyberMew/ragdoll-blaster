﻿using UnityEngine;
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
			MainMenuManager.Instance.ActiveButtons(false);
			MenuToOpen.SetActive(true);
			LeanTween.moveLocalY(MenuToOpen, -0.1f, 0.5f)
					 .setEase( LeanTweenType.easeInOutExpo);
					 

		}
	}


}
