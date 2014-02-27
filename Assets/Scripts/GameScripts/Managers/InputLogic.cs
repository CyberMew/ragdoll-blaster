using UnityEngine;
using System.Collections;

public class InputLogic : MonoBehaviour {
	Vector2 startPos;
	Vector2 direction;
	bool directionChosen;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this.gameObject);
		direction = Vector2.zero;
	}

	// Update is called once per frame
	void Update () {
		GetDistance();
		GetPinch();
	}

	void GetPinch ()
	{
		#if UNITY_ANDROID || UNITY_IPHONE

		#elif UNITYSTANDALONE_WIN || UNITY_EDITOR

		#else
		Debug.LogError("Something is wrong! Platform on " + Application.platform.ToString());
		#endif
	}

	void GetDistance()
	{
		#if UNITY_ANDROID || UNITY_IPHONE
		// Track a single touch as a direction control.
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			
			// Handle finger movements based on touch phase.
			switch (touch.phase) {
				// Record initial touch position.
			case TouchPhase.Began:
				startPos = touch.position;
				directionChosen = false;
				Debug.Log("InputLogic - Touched begin");
				break;
				
				// Determine direction by comparing the current touch
				// position with the initial one.
			case TouchPhase.Moved:
				direction = touch.position - startPos;
				Debug.Log("InputLogic - Touched Moved");
				break;
				
				// Report that a direction has been chosen when the finger is lifted.
			case TouchPhase.Ended:
				directionChosen = true;
				Debug.Log("InputLogic - Touched Ended");
				break;
			}
		}
		
		
		//todo: rewrite above code to reuse from inputmanager!
		
		#elif UNITYSTANDALONE_WIN || UNITY_EDITOR
		// no touch detected, switching to mouse instead
		if(InputManager.GetIsInputDown())
		{
			startPos = InputManager.GetCurrentPositionScreenSpace();
			Debug.Log("Triggered");
		}
		if(InputManager.GetIsInputPressed())
		{
			direction = InputManager.GetCurrentPositionScreenSpace() - startPos;
			Debug.Log("Pressed");
		}
		if(InputManager.GetIsInputReleased())
		{
			directionChosen = true;
		}
		#else
		Debug.LogError("Something is wrong! Platform on " + Application.platform.ToString());
		#endif
		InputManager.SetOffset(direction);
		
		if (directionChosen) {
			// Something that uses the chosen direction...
		}
	}
}
