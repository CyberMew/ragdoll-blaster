using UnityEngine;
using System.Collections;

public static class GameManager {

	public const int width = 1280;
	public const int height = 720;
	public static int totalLevels = 3;
	
	public static int currLevel;
	public static bool isPaused;
	public static bool isTutorialOn;
	public static bool isUIBusy;
	public static bool isInGame;

	public static int totalShots;
	public static int tempShots;

	public static bool isGameWon;

	public static int graphicQuality;
	static GameManager()
	{
		currLevel = 0;  //-1 point to main menu level. 
		totalShots = 0;
		tempShots = 0;
		
		if(PlayerPrefs.HasKey("LastPlayedLevel"))
		{
			#if !UNITY_EDITOR
			// todo: restore this back!
		//	currLevel = PlayerPrefs.GetInt("LastPlayedLevel");
		//	totalShots = PlayerPrefs.GetInt("TotalShots");
			#endif
		}
		isPaused = false;
		isTutorialOn = false;
		isUIBusy = false;
		isInGame = false;
		isGameWon = false;

		// -1 for main menu scene
		totalLevels = Application.levelCount - 1;
		graphicQuality = 1;
		
		Screen.autorotateToLandscapeLeft = true;
		Screen.autorotateToLandscapeRight = true;
		Screen.autorotateToPortrait = false;
		Screen.autorotateToPortraitUpsideDown = false;
		Screen.orientation = ScreenOrientation.AutoRotation;
		
		#if UNITY_IPHONE
		Handheld.SetActivityIndicatorStyle(iOSActivityIndicatorStyle.Gray);
		#elif UNITY_ANDROID
		Handheld.SetActivityIndicatorStyle(AndroidActivityIndicatorStyle.Small);
		#endif

		//InitializeFacebook();
	}

	public static void InitializeFacebook()
	{
		// Do a check to make sure user can use Facebook first!
		//if (Application.isWebPlayer)
		{
		//	if(Application.srcValue != "ragclone.unity3d")
		//	FBUtils.isFBAvailable = false;
			
			//if(string.Compare(Application.absoluteURL, "apps.facebook.com/418897008246300/game.unity3d", true) != 0)
				//isPirated = true;
			
			//if (isPirated)
				//print("Pirated web player");
			
		}
		Debug.Log("absoluteURL:" + Application.absoluteURL);
		Debug.Log("srcValue:" + Application.srcValue);

	#if UNITY_WEBPLAYER
			//"UnityObject2.instances[0].getUnity().SendMessage('FacebookCallbackGO', 'InitializeForFacebook', window.location.href);"
			string injection =
				"var headerElement = document.createElement('div');" +
					"headerElement.textContent = ('Check out our other great games: ...');" +
					"var body = document.getElementsByTagName(\"body\")[0];" +
					"var insertionPoint = body.children[0]; " +
					"body.insertBefore(headerElement, insertionPoint);";
			Application.ExternalEval(injection);
			//Application.ExternalEval("alert(navigator.appName);");

			// Execute javascript in iframe to keep the player centred		
			string javaScript = @"
	            window.onresize = function() {

	              var unity = UnityObject2.instances[0].getUnity();
	              var unityDiv = document.getElementById(""unityPlayerEmbed"");

	              var width =  window.innerWidth;
	              var height = window.innerHeight;

	              var appWidth = " + GameManager.width + @";
	              var appHeight = " + GameManager.height + @";

	              unity.style.width = appWidth + ""px"";
	              unity.style.height = appHeight + ""px"";

	              unityDiv.style.marginLeft = (width - appWidth)/2 + ""px"";
	              unityDiv.style.marginTop = (height - appHeight)/2 + ""px"";
	              unityDiv.style.marginRight = (width - appWidth)/2 + ""px"";
	              unityDiv.style.marginBottom = (height - appHeight)/2 + ""px"";
	            }
	            window.onresize(); // force it to resize now";
			
			Application.ExternalCall(javaScript);	
			// Alternatively we could call this in WEBPLAYER only
			//FB.Canvas.SetResolution(GameManager.width, GameManager.height, false, 0, FBScreen.CenterVertical(), FBScreen.CenterHorizontal());
			
			// Get browser name (http://answers.unity3d.com/questions/26550/browser-version-inside-unity3d.html)
			javaScript = @"
			    window.onresize = function() {
				{
					console.log(navigator.appName);

				}
				window.onresize(); // not suppose to do this!;";
			//Application.ExternalCall(javaScript);

		// Do some checks for web version for deciding whether or not to enable Facebook
		if(Application.absoluteURL.Equals("https://ragclone.parseapp.com/ragclone.unity3d") == false)
		{
			FBUtils.isFBAvailable = false;
			Debug.Log("This game file could be running locally! Please run it on Facebook instead! Disabling Facebook features.");
			// Facebook is only available 
		}
		if(FBUtils.currentURL.Contains("https://ragclone.parseapp.com"))
		{
			FBUtils.isFBAvailable = false;
			Debug.Log("This game file could be running directly on where it is hosted! Please run it on Facebook instead! Disabling Facebook features.");
		}

		//string name = "FacebookURL";
		//Application.ExternalEval("u.getUnity().SendMessage(<string name of your GameObject>, <string name of your method in the GameObject>, location.href);");
//		Application.ExternalEval("UnityObject2.instances[0].getUnity().SendMessage(\"" + name + "\", \"ReceiveURL\", document.URL);");
	//	string url = "UnityObject2.instances[0].getUnity().SendMessage(\"" + name + "\", \"ReceiveURL\", document.URL);";
		//Debug.Log("Calling:" + url);

		//FB.AppId;
	#endif
		//Application.ExternalCall(
		//	"UnityObject2.instances[0].getUnity().SendMessage('FacebookCallbackGO', 'InitializeForFacebook', window.location.href);"
		//	);
		// Get URL after 3 secs
		//	Invoke(GetWebURL, 3f);
		if(FBUtils.isFBAvailable)
		{
			Debug.Log("Facebook access is available. Init-ing now.");
			FBUtils.InitializeFacebook(OnFacebookInitComplete);
		}
	}

	/*static void GetWebURL()
	{
		Application.ExternalEval(
			"UnityObject2.instances[0].getUnity().SendMessage('FacebookCallbackGO', 'InitializeForFacebook', window.location.href);"
			);

	}

	static void ReceiveURL(string url) {
		// this will include the full URL, including url parameters etc.
		Debug.Log("Master URL:" + url);
		// Check if it contains app id or app namespace
	}*/

	static void OnFacebookInitComplete()
	{
		// be happy
		Debug.Log("GameManager has init facebook successfully.");
	}
	
	static public void GoToNextLevel(string nextLevelOverride = "")
	{
		if(isGameWon)
		{
			++currLevel;
			
			// Save the shots only when player has completed the level
			totalShots += tempShots;
		}
		// Reset the tempshots no matter what as long as we switch levels
		tempShots = 0;

		if(currLevel == totalLevels)
		{
			//Application.LoadLevel("Credits");
			Load("Credits");
			
			Debug.Log ("Loading credits screen - if it didn't work, give Target script a specific level name");
		}
		/*else if(nextLevelOverride == "Credits")
		{
			Load("Credits");
		}*/
		else
		{
			// todo: shift this line of code to the place where we actually set it (probably in options), when Unity fix their cache bug
			QualitySettings.SetQualityLevel(graphicQuality, false);
			PlayerPrefs.SetInt("LastPlayedLevel", currLevel);
			PlayerPrefs.SetInt("TotalShots", totalShots);
			PlayerPrefs.Save();
			Debug.Log("Loading next level: " + "Level" + currLevel.ToString());
			if(nextLevelOverride.Length == 0)
			{
				Load("Level" + currLevel.ToString());
			}
			else
			{
				Load(nextLevelOverride);
			}
		}
	}

	/*
	static public void OverWriteGoToNextLevel(string sceneName)
	{
		if(isGameWon)
		{
			++currLevel;
			
			// Save the shots only when player has completed the level
			totalShots += tempShots;
		}
		// Reset the tempshots no matter what as long as we switch levels
		tempShots = 0;
		
		if(currLevel == totalLevels)
		{
			//Application.LoadLevel("Credits");
			//Load("Credits");
			
			Debug.Log ("todo: change to mainmenu credits screen");
		}
		else
		{
			// todo: shift this line of code to the place where we actually set it (probably in options), when Unity fix their cache bug
			QualitySettings.SetQualityLevel(graphicQuality, false);
			PlayerPrefs.SetInt("LastPlayedLevel", currLevel);
			PlayerPrefs.SetInt("TotalShots", totalShots);
			PlayerPrefs.Save();
			Debug.Log("Loading next level: " + "Level" + currLevel.ToString());
			Load(sceneName);
		}
	}*/

	static public void LoadLevel(string levelname)
	{
		Application.LoadLevel(levelname);
	}

	public static bool IsGamePaused ()
	{
		return isPaused || isTutorialOn;
	}

	static void Load(string levelName)
	{
		#if UNITY_IPHONE || UNITY_ANDROID
		//Handheld.StartActivityIndicator();
		#endif

		// In case this is called multiple times before the next level is ready to be loaded
		if(Application.isLoadingLevel == false)
		{
			isGameWon = false;
		}
		Application.LoadLevel(levelName);
	}
}
