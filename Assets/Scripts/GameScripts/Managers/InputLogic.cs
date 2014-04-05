using UnityEngine;
using System.Collections;

public class InputLogic : MonoBehaviour {
	Vector2 startPos;
	Vector2 direction;
	bool directionChosen;

	static bool alreadyExists = false;

	// note: assume this class will always be present, which it should!
	void OnApplicationPause(bool pause)
	{
		// todo: should we check if logged in first?
		if(pause == false && FBUtils.isFBInit/* && FBUtils.IsLoggedIn*/)
		{
			// todo: this should allow us to see if people came in from facebook app's request, what request is it, and also to remove request 1 by 1 instead of in bulk
			FB.GetDeepLink( response => {
				// Some platforms return the empty string instead of null.
				if (!string.IsNullOrEmpty(response.Error))
				{
					FbDebug.Error(response.Error);
				}
				else
				{
					try
					{
						Debug.Log(response.Text);
						var index = (new System.Uri(response.Text)).Query.IndexOf("request_ids");
						if(index != -1)
						{
							// ...have the user interact with the friend who sent the request,
							// perhaps by showing them the gift they were given, taking them
							// to their turn in the game with that friend, etc.
							Debug.Log(index.ToString() + " or " + response.Text);
						}
					}
					catch(System.Exception e)
					{
						Debug.Log(response.Text);
					}
				}
			}
			);
		}
	}

	// Use this for initialization
	void Start () {
		if(alreadyExists == false)
		{
			DontDestroyOnLoad(this.gameObject);
			alreadyExists = true;
		}
		else
		{
			Destroy(this.gameObject);
		}
		direction = Vector2.zero;
	}

	// Update is called once per frame
	void Update () {
		GetDistance();
		GetPinch();
		#if UNITY_ANDROID || UNITY_IPHONE
		GetTouchDelta();
		#endif
		GetIsMoving();
	}

	void GetIsMoving ()
	{
		bool isMoving = false;
		#if UNITY_ANDROID || UNITY_IPHONE
		
		// Track a single touch as a direction control.
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			
			// Handle finger movements based on touch phase.
			switch (touch.phase) {
				// Determine direction by comparing the current touch
				// position with the initial one.
			case TouchPhase.Moved:
				isMoving = true;
				break;
			}
		}
		#elif UNITY_STANDALONE || UNITY_EDITOR || UNITY_WEBPLAYER
		
		float verticalThreshold = 0.01f;
		float horizontalThreshold = 0.01f;
		float horizontal = Input.GetAxis ("Mouse X");
		float vertical = Input.GetAxis ("Mouse Y");
		
		if(vertical > verticalThreshold)
		{
			// Code for mouse moving right
			isMoving = true;
		}
		else if (vertical < -verticalThreshold)
		{
			// Code for mouse moving left
			isMoving = true;
		}
		else
		{
			// Code for mouse standing still
		}
		
		if (horizontal > horizontalThreshold)
		{
			// Code for mouse moving forward
			isMoving = true;
		}
		else if (horizontal < -horizontalThreshold)
		{
			// Code for mouse moving backward
			isMoving = true;
		}
		else
		{
			// Code for mouse standing still
		}
		#else
		Debug.LogError("Something is wrong! Platform on " + Application.platform.ToString() + ". Setting isMoving to FALSE");
		#endif

		// Update InputManager
		InputManager.SetIsMoving(isMoving);
	}

	
	#if UNITY_ANDROID || UNITY_IPHONE
	void GetTouchDelta()
	{
		// Track a single touch as a direction control.
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			
			// Handle finger movements based on touch phase.
			switch (touch.phase) {
				// Determine direction by comparing the current touch
				// position with the initial one.
			case TouchPhase.Moved:
				InputManager.SetTouchDelta(touch.deltaPosition);
				break;

			}
		}
	}
	#endif

	void GetPinch ()
	{
		#if UNITY_ANDROID || UNITY_IPHONE

		#elif UNITYSTANDALONE_WIN || UNITY_EDITOR || UNITY_WEBPLAYER

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
				direction = Vector2.zero;
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
		
		#elif UNITYSTANDALONE_WIN || UNITY_EDITOR || UNITY_WEBPLAYER
		// no touch detected, switching to mouse instead
		if(InputManager.GetIsInputDown())
		{
			startPos = InputManager.GetCurrentPositionScreenSpace();
	//		Debug.Log("Triggered");
		}
		if(InputManager.GetIsInputPressed())
		{
			direction = InputManager.GetCurrentPositionScreenSpace() - startPos;
//			Debug.Log("Pressed");
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
