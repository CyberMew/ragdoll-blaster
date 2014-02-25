﻿using UnityEngine;
using System.Collections;

public static class GameManager {
	
	public const int totalLevels = 3;
	
	public static int currLevel;
	
	static GameManager()
	{
		currLevel = 0;
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
			Application.LoadLevel("level" + currLevel.ToString());
		}
	}
}
