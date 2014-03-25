using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Facebook.MiniJSON;
using System;

public class MainMenuFacebook : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// Make sure it's ok to initialize facebook first
		//Invoke("GetUrlAndInitFB", 0.1f); this will not be called while it is still disabled

//		profilePicture = new Texture2D(1,1);
	}

	// Update is called once per frame
	void Update () {
		/*if(Input.GetKeyDown(KeyCode.A))
		{
			Debug.Log(GC.GetTotalMemory(true));
			FBUtils.PromptLogin("", HandleLoginResponse);
		}
		if(Input.GetKeyDown(KeyCode.B))
		{
			Debug.Log(GC.GetTotalMemory(true));
			if(FB.IsLoggedIn)
			{
				FBUtils.GetProfilePicture(HandlePictureResponse);
			}
		}
		if(Input.GetKeyDown(KeyCode.C))
		{
//			FBUtils.Test();
			FBUtils.GetProfileFullName(HandleNameResponse);
		}
		if(Input.GetKeyDown(KeyCode.D))
		{

			Application.ExternalEval(
				"UnityObject2.instances[0].getUnity().SendMessage('Facebook', 'InitializeForFacebook', window.location.href);"
				);
			Application.ExternalEval(
				@"UnityObject2.instances[0].getUnity().SendMessage(""Facebook"", ""InitializeForFacebook"", window.location.href);"
				);
		}*/
	}


	// Handle response from Facebook regarding the login prompt
	/*void HandleLoginResponse(FBResult result)
	{
		// We can handle the result here ourselves, or leave it to the default callback in FBUtils.cs to do it for us

		string lastResponse = "";
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
	}*/
	/*void HandlePictureResponse(FBResult result)
	{
		// We can handle the result here ourselves, or leave it to the default callback in FBUtils.cs to do it for us

		// Some platforms return the empty string instead of null.
		if(!string.IsNullOrEmpty(result.Error))
		{
			string lastResponse = "Error Response:\n" + result.Error;
			Debug.Log(lastResponse);
		}
		else
		{
			var dict = Json.Deserialize(result.Text) as Dictionary<string,object>;
			Dictionary<string,object> dataDict = dict["data"] as Dictionary<string,object>;
			string url = ((string) dataDict["url"]).Trim();
			//string url = "https://fbcdn-profile-a.akamaihd.net/hprofile-ak-frc3/t1.0-1/p130x130/1538949_10151808558707493_1942918915_n.jpg";
			Debug.Log(url);
			if(url.EndsWith(".png") || url.EndsWith(".jpg"))
			{
				StartCoroutine("DownloadProfilePicture", url);
			}
			else
			{
				Debug.Log("Picture download requires .png or .jpg file!");
			}
		}
	}*/
	//string fullname="";
	void HandleNameResponse(FBResult result)
	{
		if(!string.IsNullOrEmpty(result.Error))
		{
			string lastResponse = "Error Response:\n" + result.Error;
			Debug.Log(lastResponse);
			return;
		}

		Debug.Log(result.Text);
		/*profile = Util.DeserializeJSONProfile(result.Text);
		GameStateManager.Username = profile["first_name"];
		///
		/// 
		var responseObject = Json.Deserialize(response) as Dictionary<string, object>;
		object nameH;
		var profile = new Dictionary<string, string>();
		if (responseObject.TryGetValue("first_name", out nameH))
		{
			profile["first_name"] = (string)nameH;
		}
		return profile;
		///
		GameStateManager.Username = profile["first_name"];
		//Debug.Log(Util.DeserializeJSONProfile)*/
	//	var responseObject = Json.Deserialize(result.Text) as Dictionary<string, object>;
	//	var dict = Json.Deserialize(result.Text) as Dictionary<string,string>;
	//	User.fullname = (string)responseObject["name"];
	//	Debug.Log(User.fullname );
	}

	/*private Texture2D profilePicture;

	// Download from the web
	IEnumerator DownloadProfilePicture(string url)
	{
		// Start a download of the given URL
		WWW www = new WWW(url);
		// Wait until the download is done
		yield return www;

		//profilePicture = new Texture2D(22,22);

		www.LoadImageIntoTexture(profilePicture);
		//profilePicture = www.texture;	// Creates new texture in memory each time
		www.Dispose();

		Debug.Log ("Picture download successfully.");
		// Some post action to notify picture download is a success.
	}*/

	public GUIStyle customStyle;
	
	void OnGUI()
	{
		
		if (User.profilePicture != null && FBUtils.IsLoggedIn)
		{
			float texHeight = 200;
			if (Screen.height - User.profilePicture.height < texHeight)
			{
				texHeight = Screen.height - User.profilePicture.height;
			}
			GUI.Label(new Rect(Screen.width * 0.5f - User.profilePicture.width * 0.5f, (Screen.height - User.profilePicture.height) * 0.5f, User.profilePicture.width, User.profilePicture.height), User.profilePicture);
		}
		GUI.TextField(new Rect(5, 0, 500, 50), FB.AccessToken);

		
		// todo: super unoptimize
		string text = "You should not be able to see this";
		// Center text on screen
		if(FBUtils.IsLoggedIn == false)
		{
			text = @"Login to Facebook to get a Facebook face for your ragdoll!

					We will not access or post to Facebook without your permission!
					Only the lowest permissions are granted to us.";
		}
		else
		{
			text = "Hello! You are currently logged in as\n<b>" + User.fullname + "</b>.";
		}
		Rect centralise = new Rect((Screen.width - customStyle.fixedWidth) * 0.5f, (Screen.height - customStyle.fixedHeight) * 0.5f - 30f, customStyle.fixedWidth, customStyle.fixedHeight);
		GUI.Label(centralise, text, customStyle);
	}
}
