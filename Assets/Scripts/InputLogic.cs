﻿using UnityEngine;
using System.Collections;

public class InputLogic : MonoBehaviour {
	Vector2 startPos;
	Vector2 direction;
	bool directionChosen;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this.gameObject);
	}

	// Update is called once per frame
	void Update () {
		direction = Vector2.zero;
#if UNITY_ANDROID
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
				break;
				
				// Determine direction by comparing the current touch
				// position with the initial one.
			case TouchPhase.Moved:
				direction = touch.position - startPos;
				break;
				
				// Report that a direction has been chosen when the finger is lifted.
			case TouchPhase.Ended:
				directionChosen = true;
				break;
			}
		}


		 //todo: rewrite above code to reuse from inputmanager!

#elif UNITYSTANDALONE_WIN || UNITY_EDITOR
		// no touch detected, switching to mouse instead
		if(InputManager.GetIsInputDown())
		{
			startPos = InputManager.GetCurrentPosition();
			Debug.Log("Triggered");
		}
		if(InputManager.GetIsInputPressed())
		{
			direction = InputManager.GetCurrentPosition() - startPos;
			Debug.Log("Pressed");
		}
		if(InputManager.GetIsInputReleased())
		{
			directionChosen = true;
		}
		#else
		Debug.LogError("Something is wrong! Platform on " + Application.platform.ToString());
#endif
		InputManager.offset = direction;

		if (directionChosen) {
			// Something that uses the chosen direction...
		}

	}
}
