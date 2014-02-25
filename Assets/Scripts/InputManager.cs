﻿using UnityEngine;
using System.Collections;

// We are assuming single based touch, like a mouse input!
public static class InputManager {

	static InputManager()
	{
#if UNITY_ANDROID
		Input.multiTouchEnabled = false;
		Debug.Log("Android Input detected. Setting to Single-based Touch.");
#endif

#if UNITYSTANDALONE_WIN
		Debug.Log("Input - Mouse");
#endif
	}

	// Check if finger touched the device
	static public bool GetIsInputDown()
	{
#if UNITY_ANDROID
		if(Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);

			if(touch.phase == TouchPhase.Began)
			{
				Debug.Log("Input triggered - Touch");
				return true;
			}
		}
		return false;
#elif UNITYSTANDALONE_WIN || UNITY_EDITOR
		if(Input.GetMouseButtonDown(0))
		{
			Debug.Log("Input down - Mouse");
			return true;
		}
		return false;
#else
		Debug.LogError("Something is wrong! Platform on " + Application.platform.ToString());
		return false;
#endif
	}
	
	// Check if finger is pressing on the device
	static public bool GetIsInputPressed()
	{
		#if UNITY_ANDROID
		if(Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			
			if(touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
			{
				Debug.Log("Input pressed - Touch");
				return true;
			}
		}
		return false;
		#elif UNITYSTANDALONE_WIN || UNITY_EDITOR
		if(Input.GetMouseButton(0))
		{
			Debug.Log("Input down - Mouse");
			return true;
		}
		return false;
		#else
		Debug.LogError("Something is wrong! Platform on " + Application.platform.ToString());
		return false;
		#endif
	}
	// Check if finger is lifted from device
	static public bool GetIsInputReleased()
	{
		#if UNITY_ANDROID
		if(Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			
			if(touch.phase == TouchPhase.Ended)
			{
				Debug.Log("Input release - Touch");
				return true;
			}
		}
		return false;	// Cancelled phase not counted (face close to phone or call coming in).
		#elif UNITYSTANDALONE_WIN || UNITY_EDITOR
		if(Input.GetMouseButtonUp(0))
		{
			Debug.Log("Input release - Mouse");
			return true;
		}
		return false;
		#else
		Debug.LogError("Something is wrong! Platform on " + Application.platform.ToString());
		return false;
		#endif
	}
	
	static public Vector2 GetCurrentPosition()
	{
		#if UNITY_ANDROID
		if(Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			Debug.Log("RAW:" + touch.rawPosition);
			Debug.Log("NON:" + touch.position);
			return touch.position;
		}
		return new Vector2(-1,-1);
		#elif UNITYSTANDALONE_WIN || UNITY_EDITOR
//		Debug.Log("MOUSE:" + Input.mousePosition);
		return Input.mousePosition;
		#else
		Debug.LogError("Something is wrong! Platform on " + Application.platform.ToString());
		return new Vector2(-1,-1);
		#endif		
	}

	static public Vector2 offset;
	// Track the direction vector from start to end point
	static public Vector2 GetCurrentOffset()
	{		
		#if UNITY_ANDROID
		/*if(Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			Debug.Log("offsetdir:" + touch.deltaPosition);
			return touch.deltaPosition;
		}
		return new Vector2(-1,-1);*/
		return offset;
		#elif UNITYSTANDALONE_WIN || UNITY_EDITOR
		Debug.Log("MOUSEoffset:" + offset);
	//	return Input.mousePosition;
		return offset;
		#else
		Debug.LogError("Something is wrong! Platform on " + Application.platform.ToString());
		return new Vector2(-1,-1);
		#endif
	}


	static public void PrintFingersDetected()
	{
		int fingerCount = 0;
		foreach (Touch touch in Input.touches) {
			if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
				fingerCount++;
			
		}
		if (fingerCount > 0)
		{
			Debug.Log("User has " + fingerCount + " finger(s) touching the screen");
			//Debug.Log("Input.touchCount:" + Input.touchCount);
			if(fingerCount != Input.touchCount)
			{
				Debug.LogError("Something is seriously going wrong here!");
			}
		}
		else if(Input.GetMouseButtonDown(0))
		{
			Debug.Log("User has 1 finger (mouse down) touching the screen");
		}


	}
}
