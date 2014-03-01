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

	static GameManager()
	{
		currLevel = 0;
		if(PlayerPrefs.HasKey("LastPlayedLevel"))
		{
			currLevel = PlayerPrefs.GetInt("LastPlayedLevel");
		}
		isPaused = false;
		isTutorialOn = false;
		isUIBusy = false;
		
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
