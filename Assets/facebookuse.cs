using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Facebook.MiniJSON;
using System;

public class facebookuse : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// Make sure it's ok to initialize facebook first
		Invoke("GetUrlAndInitFB", 1f);

		profilePicture = new Texture2D(1,1);
	}

	void GetUrlAndInitFB()
	{
		/*Application.ExternalEval(
			"var u = UnityObject2.instances[0].getUnity();" +
			"if(u == null) { console.log('Unity is not yet loaded'); } " +
			"console.log('testing console log anyway'); " +
			"u.SendMessage('" + gameObject.name + "', 'InitializeForFacebook', document.URL);"
			);*/
	//	Application.ExternalEval(
		//	"UnityObject2.instances[0].getUnity().SendMessage('" + gameObject.name + "', 'InitializeForFacebook', window.top.location.href);"
		//	);
		Application.ExternalEval("UnityObject2.instances[0].getUnity().SendMessage(\"" + name + "\", \"InitializeForFacebook\", document.URL);");
	}

	void InitializeForFacebook(string url)
	{
		Debug.Log("Finally gotten the url, can proceed to init: "  + url);

		// Processing for the url
		//todo: verify if we are in facebook!
		FBUtils.currentURL = url;

		GameManager.InitializeFacebook();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.A))
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
		}
		if(Input.GetKeyDown(KeyCode.D))
		{

			Application.ExternalEval(
				"UnityObject2.instances[0].getUnity().SendMessage('Facebook', 'InitializeForFacebook', window.location.href);"
				);
			Application.ExternalEval(
				@"UnityObject2.instances[0].getUnity().SendMessage(""Facebook"", ""InitializeForFacebook"", window.location.href);"
				);
		}
	}


	// Handle response from Facebook regarding the login prompt
	void HandleLoginResponse(FBResult result)
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
	}
	void HandlePictureResponse(FBResult result)
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
			/*var dict = Json.Deserialize(result.Text) as Dictionary<string,object>;
			Dictionary<string,object> dataDict = dict["data"] as Dictionary<string,object>;
			string url = ((string) dataDict["url"]).Trim();*/
			string url = "https://fbcdn-profile-a.akamaihd.net/hprofile-ak-frc3/t1.0-1/p130x130/1538949_10151808558707493_1942918915_n.jpg";
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
	}

	private Texture2D profilePicture;

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
	}
	
	void OnGUI()
	{
		
		if (profilePicture != null)
		{
			float texHeight = 200;
			if (Screen.height - profilePicture.height < texHeight)
			{
				texHeight = Screen.height - profilePicture.height;
			}
			GUI.Label(new Rect(Screen.width * 0.5f - profilePicture.width * 0.5f, (Screen.height - profilePicture.height) * 0.5f, profilePicture.width, profilePicture.height), profilePicture);
		}
		GUI.TextField(new Rect(5, 0, 500, 50), FB.AccessToken);
	}
}
