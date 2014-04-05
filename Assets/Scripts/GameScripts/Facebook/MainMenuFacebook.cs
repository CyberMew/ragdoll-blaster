using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Facebook.MiniJSON;
using System;

public class MainMenuFacebook : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}

	public GUIStyle customStyle;
	
	void OnGUI()
	{
		// Workaround to disable the text and image when score is up
		if(Time.timeScale <= 0f)
		{
			return;
		}
		if (User.profilePicture != null && FBUtils.IsLoggedIn)
		{
			float texHeight = 200;
			if (Screen.height - User.profilePicture.height < texHeight)
			{
				texHeight = Screen.height - User.profilePicture.height;
			}
			GUI.Label(new Rect(Screen.width * 0.5f - User.profilePicture.width * 0.5f, (Screen.height - User.profilePicture.height) * 0.5f + 30f, User.profilePicture.width, User.profilePicture.height), User.profilePicture);
		}
		
		// todo: super unoptimize
		string text = "You should not be able to see this";
		// Center text on screen
		if(FBUtils.IsLoggedIn == false)
		{
			text = "Login to Facebook to get a Facebook face for your ragdoll and compare scores with friends!\n\n" +
				"We will not access or post to Facebook without your permission!\n" +
				"Only the basic permissions are granted to us.";
		}
		else
		{
			text = "Hello! You are currently logged in as:\n<b>" + User.fullname + "</b>";
		}
		Rect centralise = new Rect((Screen.width - customStyle.fixedWidth) * 0.5f, (Screen.height - customStyle.fixedHeight) * 0.5f - 80f, customStyle.fixedWidth, customStyle.fixedHeight);
		GUI.Label(centralise, text, customStyle);
	}
}
