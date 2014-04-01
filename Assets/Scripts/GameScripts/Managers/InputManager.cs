using UnityEngine;
using System.Collections;

// We are assuming single based touch, like a mouse input!
public static class InputManager {

	static InputManager()
	{
#if UNITY_ANDROID || UNITY_IPHONE
		Input.multiTouchEnabled = false;
		Debug.Log("Android Input detected. Setting to Single-based Touch.");
#endif

#if UNITY_STANDALONE
		Debug.Log("Input - Mouse");
#endif
	}
	
	#if UNITY_ANDROID || UNITY_IPHONE
	static Vector2 lastKnowDeltaPosition;
	public static Vector2 GetTouchDelta()
	{
		return lastKnowDeltaPosition;
	}
	public static void SetTouchDelta(Vector2 pos)
	{
		lastKnowDeltaPosition = pos;
	}
#endif

	// Check if finger touched the device
	static public bool GetIsInputDown()
	{
#if UNITY_ANDROID || UNITY_IPHONE
		if(Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);

			if(touch.phase == TouchPhase.Began)
			{
				Debug.Log("Input triggered - Touch");
				SetOffset(Vector2.zero);
				return true;
			}
		}
		return false;
#elif UNITY_STANDALONE || UNITY_EDITOR || UNITY_WEBPLAYER
		if(Input.GetMouseButtonDown(0))
		{
			//Debug.Log("Input down - Mouse");
			InputManager.SetOffset(Vector2.zero);
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
		#if UNITY_ANDROID || UNITY_IPHONE
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
		#elif UNITY_STANDALONE || UNITY_EDITOR || UNITY_WEBPLAYER
		if(Input.GetMouseButton(0))
		{
//			Debug.Log("Input down - Mouse");
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
		#if UNITY_ANDROID || UNITY_IPHONE
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
		#elif UNITY_STANDALONE || UNITY_EDITOR || UNITY_WEBPLAYER
		if(Input.GetMouseButtonUp(0))
		{
//			Debug.Log("Input release - Mouse");
			return true;
		}
		return false;
		#else
		Debug.LogError("Something is wrong! Platform on " + Application.platform.ToString());
		return false;
		#endif
	}

	static private Vector2 pinchOffset;
	
	// Check if we detect pinching
	static public bool GetIsPinchInwards()
	{
		bool result = false;
		#if UNITY_ANDROID || UNITY_IPHONE
		if(Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			
			if(touch.phase == TouchPhase.Ended)
			{
				Debug.Log("Input release - Touch");
				result = true;
			}
		}
		#elif UNITY_STANDALONE || UNITY_EDITOR || UNITY_WEBPLAYER
		if(Input.GetMouseButtonUp(0))
		{
			Debug.Log("Input release - Mouse");
			result = true;
		}
		#else
		Debug.LogError("Something is wrong! Platform on " + Application.platform.ToString());
		#endif
		return result;
	}

	// Screen space/Pixel coordinates (bottom left is Origin, upper right is Screen)
	static public Vector2 GetCurrentPositionScreenSpace()
	{
		#if (UNITY_ANDROID || UNITY_IPHONE)
		if(Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			//;			Debug.Log("RAW:" + touch.rawPosition);
	//		Debug.Log("NON:" + touch.position);
			return touch.position;
		}
		return new Vector2(-1337,-1337);
		#elif UNITY_STANDALONE || UNITY_EDITOR || UNITY_WEBPLAYER
		// Origin is lower right corner in pixels
//		Debug.Log("MOUSE:" + Input.mousePosition);
		return Input.mousePosition;
		#else
		Debug.LogWarning("Something is wrong! Platform on " + Application.platform.ToString());
		return Input.mousePosition;
		#endif		
	}

	static private Vector2 dragOffset;

	static public void SetOffset(Vector2 dir)
	{
		dragOffset = dir;
	}

	// Track the direction vector from start to end point
	static public Vector2 GetCurrentDragOffset()
	{		
		#if UNITY_ANDROID || UNITY_IPHONE
		/*if(Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			Debug.Log("offsetdir:" + touch.deltaPosition);
			return touch.deltaPosition;
		}
		return new Vector2(-1,-1);*/
		return dragOffset;
		#elif UNITY_STANDALONE || UNITY_EDITOR || UNITY_WEBPLAYER
//		Debug.Log("MOUSEoffset:" + dragOffset);
	//	return Input.mousePosition;
		return dragOffset;
		#else
		Debug.LogError("Something is wrong! Platform on " + Application.platform.ToString());
		return new Vector2(-1,-1);
		#endif
	}

	static private bool isMoving;
	public static void SetIsMoving(bool move)
	{
		isMoving = move;
	}

	public static bool GetIsInputMoving()
	{
		return isMoving;
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
