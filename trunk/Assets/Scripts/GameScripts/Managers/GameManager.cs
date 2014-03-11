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
			currLevel = PlayerPrefs.GetInt("LastPlayedLevel");
			totalShots = PlayerPrefs.GetInt("TotalShots");
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
