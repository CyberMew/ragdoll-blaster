using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Facebook.MiniJSON;
using System;

public class ScoreScreen : MonoBehaviour {

	bool hasPublishPermissions;
	bool isFBLoading;

	void Awake()
	{
		//FBUtils.InitializeFacebook(GetPermissions);
	}

	// Use this for initialization
	void Start () {
		isFBLoading = false;
		hasPublishPermissions = false;

		// Check if permissions for user_games_activity and friends_games_activity for scores, as well as extended permission, publish_actions, to post scores
		//VerifyPermissionsAndGetScores();

		
		time = 0f;
		offset = 0f;
		lastDeltaPosY = 0f;
	}

	void VerifyPermissionsAndGetScores()
	{
		// Checking for permissions
		isFBLoading = true;
		FB.API("me/permissions", Facebook.HttpMethod.GET, /*PermissionsCallback*/ delegate (FBResult response) {
			// inspect the response and adapt your UI as appropriate
			// check response.Text and response.Error
			isFBLoading = false;
			if(!string.IsNullOrEmpty(response.Error))
			{
				FbDebug.Error(response.Error);
			}
			else
			{

				// if all 3 permissions are present
				// Check if we have the publish_actions permissions
				var responseObject = Json.Deserialize(response.Text) as Dictionary<string, object>;
				//var dataDict = data["data"] as Dictionary<string,object>;
				object value;
				if(responseObject.TryGetValue("data", out value)) 
				{
					var permissions = new List<object>();
					permissions = (List<object>) /*((Dictionary<string, object>)*/ value;
					if(permissions.Count > 0)
					{
						var permDict = ((Dictionary<string,object>)(permissions[0]));
						object result;
						// The string might not be present in the first place
						if(permDict.TryGetValue("publish_actions", out result)) 
						{
							// Logged in with neccessary permissions
							if(Convert.ToInt32(result) == 1)
							{
								hasPublishPermissions = true;
								Debug.Log("Permissions 'publish_actions' is validated!");
								
								// Post Scores
								//PostScore(); // do not post scores until we confirm user score is the BETTER ONE!!
							}
						}
					}
				}

				// Get scores since we are able to do it no matter what
				GetLatestScores();
			}
		});
	}

	void PostScore()
	{
		isFBLoading = true;
		//FB.API("me/scores?score=" + GameManager.totalShots, Facebook.HttpMethod.POST);
		FB.API("me/scores?score=" + GameManager.totalShots, Facebook.HttpMethod.POST);
		Debug.Log("Submitting player \"better\" scores to Facebook!");
	}

	/*void PermissionsCallback(FBResult result)
	{
		isFBLoading = false;
		if(!string.IsNullOrEmpty(result.Error))
		{
			string lastResponse = "Error Response:\n" + result.Error;
			Debug.Log(lastResponse);
			return;
		}

		// todo:Check
	}*/

	//private float sliderValue = 1.0f;
	//private float maxSliderValue = 10.0f;

	Vector2 scrollPosition = Vector2.zero;

	void GetPermissions()
	{
		isFBLoading = true;
		//todo: permissions might not be required if its our own app, might be able to skip friends_games_activity step!!
		// Get additional permissions for friends_games_activity for apps' scores, as well as publish_actions to post scores
		FB.Login("publish_actions", LoginCallback);
	}
	void LoginCallback(FBResult result)
	{
		isFBLoading = false;
		if(!string.IsNullOrEmpty(result.Error))
		{
			string lastResponse = "Error Response:\n" + result.Error;
			Debug.Log(lastResponse);
			return;
		}

		// Has to re-verify permissions since we don't know if user cancelled login dialogue
		VerifyPermissionsAndGetScores();
	}

	// Helper functions
	void GetLatestScores()
	{
		isFBLoading = true;
		// Get the scores for the 20 guys
		FB.API("app/scores?fields=score,user.limit(50)", Facebook.HttpMethod.GET, ScoresCallback);
	}
	private static Dictionary<string, Texture2D>  friendImages    = new Dictionary<string, Texture2D>();
	List<object> scoresList;
	void ScoresCallback(FBResult result)
	{
		isFBLoading = false;
		if(!string.IsNullOrEmpty(result.Error))
		{
			string lastResponse = "Error Response:\n" + result.Error;
			Debug.Log(lastResponse);
			return;
		}

		// Handle scores - problem: user latest scores might not be in yet (have to handle that locally)
		
		FbDebug.Log("ScoresCallback");
		if (result.Error != null)
		{
			FbDebug.Error(result.Error);
			return;
		}
		
		scoresList = new List<object>();

		List<object> scoreListTemp = new List<object>();
		var responseObject = Json.Deserialize(result.Text) as Dictionary<string, object>;
		object scoresTmp;
		if(responseObject.TryGetValue ("data", out scoresTmp)) 
		{
			scoreListTemp = (List<object>) scoresTmp;
		}
		else
		{
			Debug.LogWarning("Problem getting user scores!");
		}
		
		foreach(object score in scoreListTemp) 
		{
			// entry holds "score" and "user" information
			var entry = (Dictionary<string,object>) score;

			// Skip players with 0 scores by default
			if(Convert.ToInt32(entry["score"]) <= 0)
			{
				// Impossible to have 0 in our game - probably never played the game before
				//continue;
			}

			// user holds "name" and "id"
			var user = (Dictionary<string,object>) entry["user"];
			
			string userId = (string)user["id"];
			Debug.LogWarning((string)user["name"] + ": " + Convert.ToInt32(entry["score"]));
			
			// This entry is the current player
			if(string.Equals(userId,FB.UserId))
			{
				int playerHighScore = Convert.ToInt32(entry["score"]);
				Debug.Log("Local player's score on server is " + playerHighScore);
				if(playerHighScore > GameManager.totalShots && GameManager.totalShots != 0)
				{
					Debug.Log("Locally overriding with just acquired score: " + GameManager.totalShots);
					playerHighScore = GameManager.totalShots;
					entry["score"] = playerHighScore.ToString();
					
					// Post Scores since the one on FB is lousier
					PostScore();
				}
				else if(GameManager.totalShots != 0)
				{
					GameManager.totalShots = playerHighScore;
				}
			}

			// Store all the user's information
			scoresList.Add(entry);
			Debug.Log("Storing user information locally");

			// Getting images (except for the logged in user!)
			if(!friendImages.ContainsKey(userId) && userId != FB.UserId)
			{
				Debug.Log("Attempting to retrieve user photo");
				// We don't have this players image yet, request it now
				FB.API(userId + "/picture?width=128&height=128&redirect=false", Facebook.HttpMethod.GET, pictureResult =>
				       {
							if (pictureResult.Error != null)
							{
								Debug.Log("tio error while downloading the user picture!");
								FbDebug.Error(pictureResult.Error);
							}
							else
							{
								Debug.Log("Saving friend's photos.." + pictureResult.Text);
								// Initiate photo saving (code from FacebookButton.cs)
								var dict = Json.Deserialize(pictureResult.Text) as Dictionary<string,object>;
								Dictionary<string,object> dataDict = dict["data"] as Dictionary<string,object>;
								string url = ((string) dataDict["url"]).Trim();
								if(url.EndsWith(".png") || url.EndsWith(".jpg"))
								{
									StartCoroutine("DownloadProfilePicture", new string[]{url, userId});
								}
								else
								{
									Debug.Log("Picture download requires .png or .jpg file!");
								}
						bool isPictureSet = Convert.ToBoolean(dataDict["is_silhouette"]);
						if(isPictureSet)
						{
							//http://stackoverflow.com/questions/5555199/how-to-determine-if-a-facebook-user-has-uploaded-a-profile-picture-or-its-defaul
							Debug.Log("We might want to cache this male/female photo");
						}

								//friendImages.Add(userId, pictureResult.Texture);
							}
						}
				);
			}
		}//GUIStyle.CalcHeight

		
		// Now sort the entries based on score (lowest = top ranking)
		scoresList.Sort(delegate(object firstObj,
		                     object secondObj)
		            {
			// return 1 if firstObj is greater, 0 if equal, -1 if secondObj is greater
			//myList.Sort((t1, t2) => t1.ID.CompareTo(t2.ID));
			return (Convert.ToInt32(((Dictionary<string,object>)firstObj)["score"])).CompareTo(Convert.ToInt32(((Dictionary<string,object>)secondObj)["score"]));
					}
		);

		/*scoresList.ForEach( item => {
			Debug.Log(	(string)	((Dictionary<string,object>)item)	["score"]	);
			}
		);*/
	}
	
	// Download from the web
	IEnumerator DownloadProfilePicture(string[] userIdAndURL/*string url, string userId*/)
	{
		string url = (string)userIdAndURL[0];
		string userId = (string)userIdAndURL[1];
		Debug.Log(url + " " + userId);
		// Start a download of the given URL
		WWW www = new WWW(url);
		// Wait until the download is done
		yield return www;
		
		Texture2D profilePicture = new Texture2D(1,1);
		
		www.LoadImageIntoTexture(profilePicture);
		friendImages.Add(userId, profilePicture);
		//profilePicture = www.texture;	// Creates new texture in memory each time
		www.Dispose();
		
		Debug.Log ("Picture download successfully: " + profilePicture.width + "x" + profilePicture.height);
		// Some post action to notify picture download is a success.
	}

	static float scrollVelocity = 0f;
	float time;
	const float maxTime = 3f;
	Vector2 beginPos = Vector2.zero;
	float offset;
	float lastDeltaPosY;
	// Update is called once per frame
	void Update ()
	{
		// For quick testing purposes
		/*if(Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);

			if(touch.phase == TouchPhase.Moved)
			{
				Vector2 tmp = InputManager.GetCurrentPositionScreenSpace();
				tmp.y = Screen.height - tmp.y; // Convert bottom origin to top origin to match the scrollViewRect coordinate system

				// Move only when inside the scroll rect area
				if(scrollViewRect.Contains(tmp))
				{
					scrollPosition.y = offset + InputManager.GetCurrentDragOffset().y;
					lastDeltaPosY = touch.deltaPosition.y;
				}
			}
			else if(touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
			{
				offset = scrollPosition.y;

				// Was it a tap, or a drag-release?
				// Calculate if there is any inertia
				//if (Mathf.Abs(touch.deltaPosition.y) >= 10)
				{
					float distPerTime = lastDeltaPosY / touch.deltaTime;
					if (float.IsNaN(distPerTime) || float.IsInfinity(distPerTime))
					{
						distPerTime = 0.0f;
					}
					// No choice have to use it's build in frame based deltaPos to check
					scrollVelocity = (int)(distPerTime);
				}
					//timeTouchPhaseEnded = Time.time;
				scrollVelocity = lastDeltaPosY;
				time = 0f;
			}
		}*/
		if(InputManager.GetIsInputDown())
		{
			// Stop the inertia
			time = maxTime;
			scrollVelocity = 0f;
		}
		else if(InputManager.GetIsInputMoving())
		{
			Vector2 tmp = InputManager.GetCurrentPositionScreenSpace();
			tmp.y = Screen.height - tmp.y; // Convert bottom origin to top origin to match the scrollViewRect coordinate system
			
			// Move only when inside the scroll rect area
			if(scrollViewRect.Contains(tmp))
			{
				scrollPosition.y = offset + InputManager.GetCurrentDragOffset().y;

				lastDeltaPosY = InputManager.GetTouchDelta().y;
			}
		}
		else if(InputManager.GetIsInputReleased())
		{
			offset = scrollPosition.y;
			scrollVelocity = lastDeltaPosY;
			time = 0f;
		}


		// Continue scrolling (controlled via timer)
		if(time < maxTime)
		{
			scrollVelocity = Mathf.Lerp(scrollVelocity, 0, time / maxTime);
			time += Time.deltaTime;

			scrollPosition.y += scrollVelocity;
			offset = scrollPosition.y;
		}

	}

	public GUISkin ScoreSkin;
	Rect scrollViewRect;

	void DisplayScores()
	{
		//float areaWidth = 500f * Screen.width / GameManager.width;
		//float areaHeight = 700f * Screen.height / GameManager.height;

		
		#if (UNITY_IOS || UNITY_ANDROID || UNITY_WP8) && !UNITY_EDITOR
		// Remove scrollbars textures
		scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false, GUIStyle.none, GUIStyle.none);//, GUILayout.Width(areaWidth), GUILayout.Height(areaHeight * 0.7f));
		#else
		scrollPosition = GUILayout.BeginScrollView(scrollPosition);//, GUILayout.Width(areaWidth), GUILayout.Height(areaHeight * 0.7f));
		#endif

		GUILayout.BeginVertical();
		int count = 1;
		/*for(int i = 0; i < scoresList.Count; ++i)
		{
			var obj = scoresList[i];
		}*/
		foreach(object item in scoresList)
		{
			// Display rank, photo, id, score
			GUILayout.BeginHorizontal();
			GUILayout.Label(count++.ToString() + ".", GUILayout.MaxWidth(35f));



			// entry holds "score" and "user" information
			var entry = (Dictionary<string,object>) item;
			// user holds "name" and user's "id"
			var user = (Dictionary<string,object>) entry["user"];

			Texture2D texture;
			if(friendImages.TryGetValue(((string)user["id"]), out texture))
			{
				GUILayout.Label(texture, GUILayout.Width(128f), GUILayout.Height(128f));
			}
			else if((string)user["id"] == FB.UserId)
			{
				GUILayout.Label(User.profilePicture, GUILayout.Width(128f), GUILayout.Height(128f));
			}
			else
			{
				GUILayout.Label("Downloading...", GUILayout.MinWidth(128f));
			}

			GUILayout.Label(((string)user["name"]));
			//Debug.Log(((string)user["name"]));
			GUILayout.Label(Convert.ToInt32(entry["score"]).ToString(), GUILayout.MaxWidth(30f));

			GUILayout.EndHorizontal();
		}
		/*while(count < 51)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label(count++.ToString() + ".", GUILayout.MaxWidth(35f));
			//GUILayout.Label(new Texture2D(65,65));
			GUILayout.Label("PIC", GUILayout.Width(128), GUILayout.Height(128));
			GUILayout.Label("FILLER NAME");
			GUILayout.Label("212", GUILayout.MaxWidth(35f));
			GUILayout.EndHorizontal();
		}*/
		GUILayout.EndVertical();
		GUILayout.EndScrollView();
		
		if(Event.current.type == EventType.Repaint /*&& 
		   scrollArea.Contains(Event.current.mousePosition)*/)
		{
			// Get the rect of the scrollview relative to the area
			Rect scrollArea = GUILayoutUtility.GetLastRect();
			
			// Convert relative GUI point to the area
			Vector2 topLeftCorner = GUIUtility.GUIToScreenPoint(new Vector2(scrollArea.x,scrollArea.y));

			// Construct a rect in terms of screen space (not UnityGUI space) where Origin is TopLeft
			scrollViewRect = new Rect(topLeftCorner.x, topLeftCorner.y, scrollArea.width, scrollArea.height);

			//Debug.Log(topLeftCorner);// + "\nScreen: " + screenPos + " GUI: " + convertedGUIPos);
			//Debug.Log(((Screen.width - areaWidth) * 0.5f).ToString() + ((Screen.height - areaHeight) * 0.5f).ToString());
			
		}
	}

	static float areaWidth = 700f * Screen.width / GameManager.width;
	static float areaHeight = 600f * Screen.height / GameManager.height;
	
	void OnGUI()
	{
		GUI.skin = ScoreSkin;
		
		GUILayout.BeginArea(new Rect((Screen.width - areaWidth) * 0.5f, (Screen.height - areaHeight) * 0.5f, areaWidth, areaHeight));
		GUILayout.Box("test", GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
		//GUI.Box(GUILayoutUtility.GetLastRect(), "TEST");
		GUILayout.EndArea();

		// Wrap everything in the designated GUI Area
		GUILayout.BeginArea(new Rect((Screen.width - areaWidth) * 0.5f, (Screen.height - areaHeight) * 0.5f, areaWidth, areaHeight));

		// Text for leaderboard
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Label("Leaderboards");
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();

		// Adds some spacing
		GUILayout.Space(20f);

		if(isFBLoading)
		{
			// todo: display some spinning logo while stuff are loading
			//GUILayout.Label("Please wait, loading...");
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label("Please wait, loading...");
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}
		// Check scores against friends, if we haven't authorize yet
		else if(hasPublishPermissions == false)
		{
			if(GUILayout.Button("Post my scores and compare\nmy scores against friends!"))
			{
				// Get permissions, and if successful, get latest scores after
				GetPermissions();
				//hasPublishPermissions = true;
			}
		}
		else
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label("Ranking");		
			GUILayout.Label("Name");
			GUILayout.Label("Shots");
			GUILayout.EndHorizontal();
			
			// Adds some spacing
			GUILayout.Space(20f);

			// Display scores (inside scrollable area)
			DisplayScores();
			
			// Adds some spacing
			GUILayout.Space(20f);
			
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			// Display Share/Challenge button
			if(GUILayout.Button("Share/Challenge", GUILayout.ExpandWidth(false)))
			{
				FB.AppRequest("I just completed RagClone with only " + GameManager.totalShots + " shots :)\nTry and see if you can beat me!",title: "RagClone Challenge!");
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}
		GUILayout.EndArea();
		/*
		
		GUILayout.BeginArea (new Rect (100, 150, Screen.width-200, Screen.height-100));
		GUILayout.Button ("I am a regular Automatic Layout Button");
		GUILayout.Button ("My width has been overridden", GUILayout.Width (95));
		GUILayout.EndArea ();



		// Fixed Layout
		GUI.Button (new Rect (25,25,100,30), "I am a Fixed Layout Button");
		
		// Automatic Layout
		GUILayout.Button ("I am an Automatic Layout Button");

		
		GUILayout.Button ("I am not inside an Area");
		GUILayout.BeginArea (new Rect (Screen.width/2, Screen.height/2, 300, 300));
		GUILayout.Button ("I am completely inside an Area");
		GUILayout.EndArea ();


		// Wrap everything in the designated GUI Area
		GUILayout.BeginArea (new Rect (300,0,200,60));
		
		// Begin the singular Horizontal Group
		GUILayout.BeginHorizontal();
		
		// Place a Button normally
		if (GUILayout.RepeatButton ("Increase max\nSlider Value"))
		{
			maxSliderValue += 3.0f * Time.deltaTime;
		}
		
		// Arrange two more Controls vertically beside the Button
		GUILayout.BeginVertical();
		GUILayout.Box("Slider Value: " + Mathf.Round(sliderValue));
		sliderValue = GUILayout.HorizontalSlider (sliderValue, 0.0f, maxSliderValue);
		
		// End the Groups and Area
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
		GUILayout.EndArea();

		
		GUILayout.BeginArea (new Rect (100, 150, Screen.width-200, Screen.height-100));
		GUILayout.Button ("I am a regular Automatic Layout Button");
		GUILayout.Button ("My width has been overridden", GUILayout.Width (95));
		GUILayout.EndArea ();

		
		GUILayout.BeginHorizontal();
		GUILayout.Button("Short Button", GUILayout.ExpandWidth(false));
		GUILayout.Button("Very very long long long ass Button");
		GUILayout.EndHorizontal();*/

		
		Vector2 screenPos = Event.current.mousePosition;
		GUI.BeginGroup(new Rect(123, 10, 100, 100));
		GUI.Box(new Rect(110,0,100,100), "");
		Vector2 convertedGUIPos = GUIUtility.ScreenToGUIPoint(screenPos);
		GUI.EndGroup();
		Vector2 convertedGUIPos2 = GUIUtility.GUIToScreenPoint(new Vector2(0,0));
		//Debug.Log(convertedGUIPos2 + "\nScreen: " + screenPos + " GUI: " + convertedGUIPos);
	}
}
