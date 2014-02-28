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
	}
	
	static public void GoToNextLevel()
	{
		if(currLevel == totalLevels)
		{
			Application.LoadLevel("Credits");
		}
		else
		{
			++currLevel;
			Debug.Log("Loading next level: " + "Level" + currLevel.ToString());
			Application.LoadLevel("Level" + currLevel.ToString());
		}
	}

	public static bool IsGamePaused ()
	{
		return isPaused || isTutorialOn;
	}
}
