using UnityEngine;
using System.Collections;
//Notice that we're using the Facebook implementation of MiniJSON:
using Facebook.MiniJSON;
using System.Collections.Generic;

public class FBUtils : ScriptableObject {
	public static bool IsLoggedIn = false;


	public static bool isJobPending = false;
	public static string lastResult = "";
	public static Texture2D lastResponseTexture = null;
	static string apiQuery = "";
	
	static Facebook.FacebookDelegate myLogger = new Facebook.FacebookDelegate(DefaultCallback);
	static Facebook.FacebookDelegate prevCB = null;
	
	public static bool isFBAvailable = true;	// Use this to check if the game has enabled Facebook or if Facebook is enabled on the platform
	public static bool isFBInit = false; // Check if Facebook features are ready to go (can be used to show button only if facebook is available AND ready)

	public static string currentURL = ""; // Master URL. If we were hosting on parseapp but inside Facebook canvas, this would be the FB's url

	public static void InitializeFacebook(Facebook.InitDelegate userCallback)
	{
		// Another safety check
		if(isFBAvailable == false)
		{
			Debug.Log("Something went wrong, Facebook should be available for use before calling Init()");
			return;
		}
		if(isFBInit)
		{
			Debug.LogWarning("Facebook's Init() has already been called before!");
			return;
		}
		userCallback += OnFBInit;
		FB.Init(userCallback);
	}
	static void OnFBInit()
	{
		isFBInit = true;
		Debug.Log("FB Init() is successful.");
	}

	public static void PromptLogin(string accessesRequired = "", Facebook.FacebookDelegate userCallback = null)
	{
		if(isFBInit == false)
		{
			Debug.Log("Facebook Init() has not been called yet!");
			return;
		}

		if(isJobPending)
		{
			Debug.Log("Still waiting for response from previous Facebook call. Skipping PromptLogin command.");
			return;
		}

		if(FB.IsLoggedIn)
		{
			if(IsLoggedIn)
			{
				Debug.Log("User is already logged in. Log out first if you want to switch user.");
				return;
			}
			else
			{
				Debug.Log("User is already logged in, but permissions not granted at all (i.e. not even basic permissions granted. Requesting for basic permissions via Login again.");
				//todo: request permissions again

			}
		}

		/*if(postCallback == null)
		{
			//postCallback = DefaultCallback;
			Debug.Log("A proper login callback has to be given");
			return;
		}*/

		//****isJobPending = true;
		// Cleanup from previous call
		/*if(prevCB != null)
		{
			myLogger -= prevCB;
			prevCB = null;
		}
		//myLogger += 
		if(postCallback != null)
		{
			//myLogger += new Facebook.FacebookDelegate(postCallback);
			myLogger += postCallback;
			prevCB = postCallback;
		}*/
		if(userCallback != null)
		{
			prevCB = userCallback;
			myLogger += userCallback;
		}
		myLogger += PostCallback;

		if(accessesRequired.Trim().Length == 0)
		{
			// There was previously some bug in 5.0.3 that if it was empty it would not login
			accessesRequired = "basic_info";
		}

		Debug.Log("Attempting login/acquiring " + accessesRequired + " permission.");
		isJobPending = true;
		FB.Login(accessesRequired, myLogger);
	}
	//public delegate void FacebookDelegate (FBResult result);
	public static void GetProfilePicture(Facebook.FacebookDelegate userCallback = null, int width = 128, int height = 128)
	{
		if(isJobPending)
		{
			Debug.Log("Still waiting for response from another Facebook call. Skipping GetProfilePicture command.");
			return;
		}

		apiQuery = "me/picture?width=" + width.ToString() + "&height=" + height.ToString() + "&redirect=false";

		FBAPIQuery(apiQuery, userCallback, Facebook.HttpMethod.GET);
	}
	public static void GetProfileFullName(Facebook.FacebookDelegate userCallback = null)
	{
		if(isJobPending)
		{
			Debug.Log("Still waiting for response from another Facebook call. Skipping GetProfileFullName command.");
			return;
		}
		
		apiQuery = "me?fields=id,name,name_format,first_name,last_name";
		
		FBAPIQuery(apiQuery, userCallback, Facebook.HttpMethod.GET);
	}

	public static void PostScore(int score, Facebook.FacebookDelegate userCallback = null)
	{
		if(isJobPending)
		{
			Debug.Log("Still waiting for response from another Facebook call. Skipping PostScore command.");
			return;
		}

		apiQuery = "";
		
		FBAPIQuery(apiQuery, userCallback, Facebook.HttpMethod.POST);
	}

	public static void LogOut()
	{
		if(isJobPending)
		{
			Debug.Log("Still waiting for response from another Facebook call. Skipping LogOut command.");
			return;
		}
		Debug.Log("todo: logout and remove basic permissions are the same thing for us");
		FB.Logout();
		IsLoggedIn = FB.IsLoggedIn;
		if(IsLoggedIn == true)
		{
			Debug.Log("Shit is happening");
		}
	}

	public static void RemoveAllPermissions(Facebook.FacebookDelegate userCallback = null)
	{
		if(isJobPending)
		{
			Debug.Log("Still waiting for response from another Facebook call. Skipping RemoveAllPermissions command.");
			return;
		}
		Debug.Log("Deleting permissions");
		if(userCallback == null)
		{
			Debug.Log("You must provide a user call back!");
			return;
		}
		apiQuery = "me/permissions";
		
		FBAPIQuery(apiQuery, userCallback, Facebook.HttpMethod.DELETE);
	}

	/**************private**************/
	static void FBAPIQuery(string query, Facebook.FacebookDelegate userCallback, Facebook.HttpMethod httpMethod)
	{		
		//****isJobPending = true;

		/*Facebook.FacebookDelegate myLogger = null;
		myLogger += new Facebook.FacebookDelegate(CompletedCallback);
		if(postCallback != null)
		{
			myLogger += new Facebook.FacebookDelegate(userCallback);
			myLogger += new Facebook.FacebookDelegate(userCallback);
			myLogger += new Facebook.FacebookDelegate(userCallback);
		}*/

		// Remove 
	/*	if(prevCB != null)
		{
			myLogger -= prevCB;
			prevCB = null;
			//System.GC.Collect();
		}
		if(postCallback != null)
		{
			prevCB = postCallback;
			myLogger += postCallback;
		}
		myLogger += PostCallback;*/

		// If user supplies a callback, we need to clean it up in PostCallback
		if(userCallback != null)
		{
			prevCB = userCallback;
			myLogger += userCallback;
		}
		myLogger += PostCallback;

		FB.API(query, httpMethod, myLogger);
	}
	/*static void FBAPIPost(string query, Facebook.FacebookDelegate postCallback)
	{		
		;
		Facebook.FacebookDelegate myLogger = null;
		myLogger += new Facebook.FacebookDelegate(DefaultCallback);
		if(postCallback != null)
		{
			myLogger += new Facebook.FacebookDelegate(postCallback);
		}
		
		FB.API(query, Facebook.HttpMethod.POST, postCallback);
	}*/


	static void DefaultCallback(FBResult result)
	{
	//	Debug.Log("Default Callback called");
		lastResponseTexture = null;
		//string lastResponse = "";
		// Some platforms return the empty string instead of null.
		if(!string.IsNullOrEmpty(result.Error))
		{
			string lastResponse = "Error Response:\n" + result.Error;
			//Debug.Log(lastResponse);
			//ProcessError(result.Error);
		}
		else if(!apiQuery.Contains("/picture"))	// Normal query
		{
			//lastResponseTexture = result.Texture;
			// Received json response
			lastResult = result.Text;
			//lastResponse = "Success Response:\n" + result.Text;
		}
		else // expected picture
		{
			//lastResponseTexture = result.Texture; // issue with web, get json response and then via www instead
			//lastResponse = "Success Picture Response:\n" + result.Text;
			lastResult = result.Text;
			//StartCoroutine("GetProfilePic");
		}
	}

	static void PostCallback(FBResult result)
	{
		//Debug.Log("Performing PostCallback: removing delegates");
		if(prevCB != null)
		{
			myLogger -= prevCB;
			prevCB = null;
		}
		myLogger -= PostCallback;
		
		//	Debug.Log("Cleared isJobPending to false");
		isJobPending = false;		
	}
	/*
	public static void Test()
	{
		Debug.Log("Testing remainder of myLogger");
		apiQuery = "me/picture?width=128&height=128&redirect=false";

		//myLogger += PostCallback;
		//FB.API(apiQuery, Facebook.HttpMethod.GET, myLogger);		
		Facebook.FacebookDelegate userCallback = null;
		FBAPIQuery(apiQuery, userCallback, Facebook.HttpMethod.GET);
	}*/


	public static void ProcessError(string error)
	{		
		var responseObject = Json.Deserialize(error) as Dictionary<string, object>;
		//var dict = Json.Deserialize(result.Text) as Dictionary<string,string>;
//		User.fullname = (string)responseObject["name"];
		object errorObject;
		if (responseObject.TryGetValue ("error", out errorObject)) 
		{
			var errorDetails = (Dictionary<string, string>) errorObject;
			Debug.Log("Error Message:" + (string)errorDetails["message"] + "\nType:" + (string)errorDetails["type"] + "\nCode:" + (string)errorDetails["code"]);
		}
	}
}
