using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Facebook.MiniJSON;

class FacebookButton : Button {
	
	public Sprite LogOutSpriteDefault = null;
	public Sprite LogOutSpriteHover = null;
	public Sprite LogInSpriteDefault = null;
	public Sprite LogInSpriteHover = null;

	private SpriteRenderer sr;

	public GameObject ScoreButton = null;

	// Use this for initialization
	void Start () {
		// The button starts out as we are not logged in
		// so if we are logged in, 
		sr = GetComponent<SpriteRenderer>();
		LogInSpriteDefault = sr.sprite;

		if(FBUtils.IsLoggedIn)
		{
			ScoreButton.SetActive(true);
			GetInformation();
		}
		else
		{
			ScoreButton.SetActive(false);
		}
		UpdateButtonSprite();
	}
	
	// Update is called once per frame
	new void Update () {
		base.Update();
		if(Input.GetKeyDown(KeyCode.A))
		{
			//FBUtils.PromptLogin("");
			FB.Login("email");
			FBUtils.IsLoggedIn = true;
		}
		if(Input.GetKeyDown(KeyCode.B))
		{
			FBUtils.RemoveAllPermissions(RemoveAllPermissions);
		}
	}
	
	internal override void ButtonAction()
	{
		if(FBUtils.IsLoggedIn)
		{
			// We treat this as logging out
			FBUtils.RemoveAllPermissions(RemoveAllPermissions);
			// Treat user as logged out first
			FBUtils.IsLoggedIn = false;
			ScoreButton.SetActive(false);
		}
		else
		{
			FBUtils.PromptLogin("", HandleLoginResponse);
			//collider2D.enabled = false;
		}
		UpdateButtonSprite();
	}

	void RemoveAllPermissions(FBResult result)
	{
		// Treat user as logged out
		FBUtils.IsLoggedIn = false;
		Debug.Log("Permissions Removed. This should only appear once!");
	}
	
	void HandleLoginResponse(FBResult result)
	{
		string lastResponse;
		if (!string.IsNullOrEmpty(result.Error))
		{
			lastResponse = "Error Response:\n" + result.Error;
			//collider2D.enabled = true;
			//FBUtils.ProcessError(result.Error);
		}
		else if (!FB.IsLoggedIn)
		{
			lastResponse = "Login cancelled by Player";
			//collider2D.enabled = true;
		}
		else
		{
			lastResponse = "Login was successful by " + FB.UserId + "!";
			// Get user fullname
			//FBUtils.GetProfileFullName(HandleNameResponse);
			if(FBUtils.IsLoggedIn == false)
			{
				Debug.Log("Logged-in status from FBUtils changed from false to true.");
			}
			FBUtils.IsLoggedIn = true;

			GetInformation();

			// Disable (now change) button
			UpdateButtonSprite();
			// Show score button
			ScoreButton.SetActive(true);
		}
		Debug.Log(lastResponse);
	}
	void HandleNameResponse(FBResult result)
	{
		if(!string.IsNullOrEmpty(result.Error))
		{
			string lastResponse = "Error Response:\n" + result.Error;
			Debug.Log(lastResponse);
			//FBUtils.ProcessError(result.Error);
			return;
		}
		
		Debug.Log(result.Text);

		var responseObject = Json.Deserialize(result.Text) as Dictionary<string, object>;
		//var dict = Json.Deserialize(result.Text) as Dictionary<string,string>;
		User.fullname = (string)responseObject["name"];
		Debug.Log(User.fullname );
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
	}
	// Download from the web
	IEnumerator DownloadProfilePicture(string url)
	{
		Debug.Log("Still dling profile picture...");
		// Start a download of the given URL
		WWW www = new WWW(url);
		// Wait until the download is done
		yield return www;
		
		//profilePicture = new Texture2D(22,22);
		
		www.LoadImageIntoTexture(User.profilePicture);
		//profilePicture = www.texture;	// Creates new texture in memory each time
		www.Dispose();
		
		Debug.Log ("Picture download successfully.");
		// Some post action to notify picture download is a success.
	}
	void OnDestroy()
	{
		// Stop coroutine
		//MissingReferenceException: The object of type 'FacebookButton' has been destroyed but you are still trying to access it.
		//		Your script should either check if it is null or you should not destroy the object.
		//			FacebookButton.HandlePictureResponse (.FBResult result) (at Assets/Scripts/GameScripts/Facebook/FacebookButton.cs:130)
		StopCoroutine("DownloadProfilePicture"); //fixme: shit doesn't fully work - still sometimes crash if coroutine already running
	}

	void OnGUI()
	{
		//GUI.Label(new Rect(0,Screen.height - 50,100,100), lastResponse);
	}
	
	void OnEnable()
	{
		// If we are already logged in, don't need the button anymore (that is when there is only a login button, now there's a logout button, we cannot disable it)
		if(FBUtils.IsLoggedIn)
		{
			//collider2D.enabled = false;
			//renderer.enabled = false;

			GetInformation();
		}
	}

	void GetInformation()
	{
		//todo: check and make sure we have the ncessary permissions
		// if permissions for the following are missing, reprompt

		string apiQuery = "me?fields=id,name,name_format,first_name,last_name";
		FB.API(apiQuery, Facebook.HttpMethod.GET, HandleNameResponse);
		// Get user profile picture
		//FBUtils.GetProfilePicture(HandlePictureResponse);
		apiQuery = "me/picture?width=128&height=128&redirect=false";
		FB.API(apiQuery, Facebook.HttpMethod.GET, HandlePictureResponse);

		// Get and remove any notifications for the app, if any
		apiQuery = "me/apprequests?fields=id,application";
		FB.API(apiQuery, Facebook.HttpMethod.GET, HandleAppRequests);
	}
	void HandleAppRequests(FBResult result)
	{
		if(!string.IsNullOrEmpty(result.Error))
		{
			string lastResponse = "Error Response:\n" + result.Error;
			Debug.Log(lastResponse);
			//FBUtils.ProcessError(result.Error);
			return;
		}
		// Loop through and delete all requests sent to me, if any
		var dict = Json.Deserialize(result.Text) as Dictionary<string,object>;
		//Dictionary<string,object> dataDict = dict["data"] as Dictionary<string,object>;
		List<object> notifications = (List<object>) dict["data"];
		foreach(object evt in notifications)
		{
			var appEvent = (Dictionary<string, object>) evt;
			//Debug.Log(appEvent["id"] + " " + appEvent["created_time"]);

			var appInfoObj = appEvent["application"];
			var appInfo = (Dictionary<string, object>) appInfoObj;

			// Check if the app is came from our game app
			if(((string)appInfo["id"]) == FB.AppId)
			{
				// Delete all the requests
				Debug.Log("Deleting request id: " + appEvent["id"]);
				FB.API((string)appEvent["id"], Facebook.HttpMethod.DELETE, null);
			}
		}
		/*
		object friendsH;
		Debug.Log(result.Text);
		var friends = new List<object>();
		if (dataDict.TryGetValue("friends", out friendsH))
		{
			friends = (List<object>)(((Dictionary<string, object>)friendsH)["data"]);

			foreach(object evt in friends)
			{
				var eventDict = (Dictionary<string, object>)evt;
				Debug.Log((string)eventDict["id"]);
				var appDict = (Dictionary<string, object>)eventDict["application"];
				Debug.Log((string)appDict["name"]);

			}
		}*/
			// todo when game over do this
		/*if (FB.IsLoggedIn && !string.IsNullOrEmpty(GameStateManager.FriendID))
		{
			var querySmash = new Dictionary<string, string>();
			querySmash["profile"] = GameStateManager.FriendID;
			FB.API ("/me/" + FB.AppId + ":smash", Facebook.HttpMethod.POST, 
			        delegate(FBResult r) { Util.Log("Result: " + r.Text); }, querySmash);
		}*/
	}

	void UpdateButtonSprite()
	{
		if(FBUtils.IsLoggedIn)
		{
			sr.sprite = LogOutSpriteDefault;
			HoverSprite = LogOutSpriteHover;
			PushedSprite = LogOutSpriteHover;
			DefaultSprite = LogOutSpriteDefault;
		}
		else
		{
			sr.sprite = LogInSpriteDefault;
			HoverSprite = LogInSpriteHover;
			PushedSprite = LogInSpriteHover;
			DefaultSprite = LogInSpriteDefault;
		}
	}
}
