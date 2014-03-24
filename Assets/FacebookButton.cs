using UnityEngine;
using System.Collections;

class FacebookButton : Button {
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	internal override void ButtonAction()
	{
		FBUtils.PromptLogin("", HandleLoginResponse);
	}

	void HandleLoginResponse(FBResult result)
	{
		string lastResponse;
		if (result.Error != null)
		{
			lastResponse = "Error Response:\n" + result.Error;
		}
		else if (!FB.IsLoggedIn)
		{
			lastResponse = "Login cancelled by Player";
		}
		else
		{
			lastResponse = "Login was successful by " + FB.UserId + "!";
		}
		Debug.Log(lastResponse);
	}
}
