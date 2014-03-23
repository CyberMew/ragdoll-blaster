using UnityEngine;
using System.Collections;
//Notice that we're using the Facebook implementation of MiniJSON:
using Facebook.MiniJSON;
using System.Collections.Generic;

public class FBManager : MonoBehaviour {

	static int ha = 0;
	object aa ;
	static bool facebookAvailable = true;

	static string browser = "";

	void Awake()
	{
	}

	// Use this for initialization
	void Start () {


		FB.Init(OnInitComplete);

		// Play nice with the web player, if this is a web player build
		if(Application.isWebPlayer)
		{
			//Security.PrefetchSocketPolicy(EsIp, EsPort); // Hangs the stand-alone player
			if(true/*due to due, if we are in netscape, disable facebook*/)
			{
				facebookAvailable = false;
			}
		};
	}

	// Logs:
	// "got profile pic" if the call was successful
	// "no profile pic" if there was en error (Check response.Error)
	void LogCallback(FBResult response) {
		//Debug.Log(response.Texture != null ? "got profile pic" : "no profile pic");
		Debug.Log("LogCallback1");
		if(response.Texture != null)
		{
			Debug.Log("LogCallback2");
			lastResponseTexture = response.Texture;
		}
		Debug.Log("LogCallback3");
		//Callback (response);
		Debug.Log("LogCallback4");
	}

	// todo ochange logcalback to this callback
	void Callback(FBResult result)
	{
		Debug.Log("Callback called");
		lastResponseTexture = null;
		// Some platforms return the empty string instead of null.
		if (!string.IsNullOrEmpty(result.Error))
			lastResponse = "Error Response:\n" + result.Error;
		else if (!apiQuery.Contains("/picture"))
		{
			lastResponseTexture = result.Texture;
			lastResponse = "Success Response:\n" + result.Text;
		}
		else
		{
			lastResponseTexture = result.Texture;
			lastResponse = "Success Picture Response:\n" + result.Text;
			jsonResult = result.Text;
			StartCoroutine("GetProfilePic");
		}
		Debug.Log(lastResponse+"before"+result.Text);
		//LogCallback(result);
		Debug.Log(lastResponse+"after"+result.Text);
		//https://graph.facebook.com/me/picture?type=large&access_token=CAAF8ZCA9fvhwBAAknmVAmWmvjsjnSk2yW36NOPg3SdQ4HJaiJeJSm24PlElE5b4BJudY8dnEv9ZAUpAdarxVCregV5X9FIaFvbjhaBcABDAysCsCNo1NlaDDG291toDeJRaVY62kGT1RNBZB4oM3tTVcqt0QRCk0DK4w6sAk7ZBjyEnrnKnbDmPsGyQcqQcZD
	}

	string jsonResult;
	public string apiQuery = "";
	Texture2D lastResponseTexture = null;

	IEnumerator GetProfilePic ()
	{
		// one way to get the current user's profile picture into Unity
		if (FB.IsLoggedIn) {
			Debug.Log("retrieveing profile pic");
			//string url = "https" + "://graph.facebook.com/" + FB.UserId + "/picture?type=large";
			//url += "&access_token=" + FB.AccessToken;
			//string url = "https://graph.facebook.com/545657492/picture?type=" +
			//	"large&access_token=CAAF8ZCA9fvhwBAK0eJ6kiUA1WWNWfWiSEI7q3rs1Soj6MMkbXZCm37MFyTT2OBCRzU2ZAiKK8m8YL6ZAIVmgoWUy1VZAV8231eODMV0ivvOjj9ZAhb5gGJ41UligKauZCNAnvRStuiYUeVXK36xOUBZBnwMuQSAHa3xJkZBof5kOa4e6j3V61EkLnZBIHepuYnfgEZD";
			var dict = Json.Deserialize(jsonResult) as Dictionary<string,object>;
			Dictionary<string,object> dataDict = dict["data"] as Dictionary<string,object>;
			WWW www = new WWW((string) dataDict["url"]);
			//Texture2D picTexture = new Texture2D(width, height, TextureFormat.DXT1, false);
			yield return www;

			www.LoadImageIntoTexture(lastResponseTexture);

			//WWW www = new WWW (url);
			//yield return www;
			Debug.Log ("..success" + lastResponseTexture.width + " " + lastResponseTexture.height);
			//lastResponseTexture = www.texture;
		}
		else
		{
			Debug.Log("SCAM");
		}
	}

	void OnGUI()
	{
		
		if (lastResponseTexture != null)
		{
			float texHeight = 200;
			if (Screen.height - lastResponseTexture.height < texHeight)
			{
				texHeight = Screen.height - lastResponseTexture.height;
			}
			GUI.Label(new Rect(Screen.width * 0.5f - lastResponseTexture.width * 0.5f, (Screen.height - lastResponseTexture.height) * 0.5f, lastResponseTexture.width, lastResponseTexture.height), lastResponseTexture);
		}
		GUI.TextField(new Rect(5, 0, 500, 50), FB.AccessToken);
	}

	void GetProfilePicture ()
	{
		//FB.API ("me?fields=picture.type(small)", Facebook.HttpMethod.GET, LogCallback);
		//StartCoroutine("GetProfilePic");
		Debug.Log("LogCallback-1");
		//FB.API ("/me/picture?type=large" + "&redirect=false", Facebook.HttpMethod.GET, LogCallback);
		//FB.API ("/me/picture?width=50&height=50", Facebook.HttpMethod.GET, Callback);
		apiQuery = "1259800271/picture?width=128&height=128&redirect=false";
		FB.API (apiQuery, Facebook.HttpMethod.GET, Callback);
		//FB.API ("/me/picture", Facebook.HttpMethod.GET, Callback);

		Debug.Log("LogCallback0");
		++ha;
		Debug.Log (ha);
	}

	private void OnInitComplete()
	{
		Debug.Log("OK");
		Debug.Log("FB.Init completed: Is user logged in? " + FB.IsLoggedIn);
		//isInit = true;
		if(FB.IsLoggedIn) {
			// Get data from Facebook to personalize the player's experience
			//StartCoroutine(NewMethod);
			GetProfilePicture ();
		} else {
			// Prompt the user to log in, or offer a "guest" experience
			Debug.Log("Prompting user to login");
			FB.Login("", LoginCallback);
		}
	}

	// Update is called once per frame
	void Update () {
		if(FB.IsLoggedIn) {
			// Get data from Facebook to personalize the player's experience
		} else {
			// Prompt the user to log in, or offer a "guest" experience
		}
	}


	/*void AuthCallback(FBResult result) {
		if(FB.IsLoggedIn) {
			Debug.Log(FB.UserId);
		} else {
			Debug.Log("User cancelled login");
		}
	}*/

	private string lastResponse = "";
	void LoginCallback(FBResult result)
	{
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

			GetProfilePicture();
		}
		Debug.Log(lastResponse);
		Debug.Log(FB.AccessToken);

		FB.API("/me/permissions", Facebook.HttpMethod.GET, delegate (FBResult response) {
			// inspect the response and adapt your UI as appropriate
			// check response.Text and response.Error
			Debug.Log(response.Text);
		});

	}
}
