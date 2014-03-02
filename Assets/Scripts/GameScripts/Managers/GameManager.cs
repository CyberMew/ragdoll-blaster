using UnityEngine;
using System.Collections;

public static class GameManager {

	public const int width = 1280;
	public const int height = 720;
	public const int totalLevels = 3;
	
	public static int currLevel;
	public static bool isPaused;
	public static bool isTutorialOn;
	public static bool isUIBusy;

	public static int totalShots;



	public static int graphicQuality;
	static GameManager()
	{

		currLevel = -1;  //-1 point to main menu level. 
		if(PlayerPrefs.HasKey("LastPlayedLevel"))
		{
			// todo: restore this back!
			//currLevel = PlayerPrefs.GetInt("LastPlayedLevel");
		}
		isPaused = false;
		isTutorialOn = false;
		isUIBusy = false;
		totalShots = 0;
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
	
	static public void GoToNextLevel()
	{
		if(currLevel == totalLevels)
		{
			//Application.LoadLevel("Credits");
			Load("Credits");
		}
		else
		{
			++currLevel;

		
			// todo: shift this line of code to the place where we actually set it (probably in options), when Unity fix their cache bug
			QualitySettings.SetQualityLevel(graphicQuality, false);
			PlayerPrefs.SetInt("LastPlayedLevel", currLevel);
			PlayerPrefs.Save();
			Debug.Log("Loading next level: " + "Level" + currLevel.ToString());
			//Application.LoadLevel("Level" + currLevel.ToString());
			Load("Level" + currLevel.ToString());
		}
	}

	public static bool IsGamePaused ()
	{
		return isPaused || isTutorialOn;
	}

	static void Load(string levelName)
	{
		Handheld.StartActivityIndicator();
		Application.LoadLevel(levelName);
	}
}
