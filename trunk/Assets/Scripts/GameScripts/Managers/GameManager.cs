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
		isPaused = false;
		isTutorialOn = false;
		isUIBusy = false;
		
		Screen.autorotateToLandscapeLeft = true;
		Screen.autorotateToLandscapeRight = true;
		Screen.orientation = ScreenOrientation.AutoRotation;
		//todo: remove me - just a test
		Handheld.StartActivityIndicator();
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
		#if UNITY_IPHONE
		Handheld.SetActivityIndicatorStyle(iOSActivityIndicatorStyle.Gray);
		#elif UNITY_ANDROID
		Handheld.SetActivityIndicatorStyle(AndroidActivityIndicatorStyle.Small);
		#endif
		Handheld.StartActivityIndicator();
		//yield return new WaitForSeconds(0);
		Application.LoadLevel(levelName);
	}
}
