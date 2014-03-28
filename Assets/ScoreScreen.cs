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
								PostScore();
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
		FB.API("me/scores?score=1", Facebook.HttpMethod.POST);
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
	
	// Update is called once per frame
	void Update () {
	
	}
	
	private float sliderValue = 1.0f;
	private float maxSliderValue = 10.0f;

	Vector2 scrollPosition = new Vector2(50,50);

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
		FB.API("app/scores?fields=score,user.limit(20)", Facebook.HttpMethod.GET, ScoresCallback);
	}
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
				continue;
			}

			// user holds "name" and "id"
			var user = (Dictionary<string,object>) entry["user"];
			
			string userId = (string)user["id"];
			Debug.Log((string)user["name"] + ": " + Convert.ToInt32(entry["score"]));
			
			// This entry is the current player
			if(string.Equals(userId,FB.UserId))
			{
				int playerHighScore = Convert.ToInt32(entry["score"]);
				Debug.Log("Local player's score on server is " + playerHighScore);
				if(playerHighScore > GameManager.totalShots)
				{
					Debug.Log("Locally overriding with just acquired score: " + GameManager.totalShots);
					playerHighScore = GameManager.totalShots;
				}
				
				entry["score"] = playerHighScore.ToString();
				//GameStateManager.HighScore = playerHighScore;
			}

			// Store all the user's information
			scoresList.Add(entry);

			// Getting images
			//private static Dictionary<string, Texture>  friendImages    = new Dictionary<string, Texture>();
			/*if(!friendImages.ContainsKey(userId))
			{
				// We don't have this players image yet, request it now
				FB.API(Util.GetPictureURL(userId, 128, 128), Facebook.HttpMethod.GET, pictureResult =>
				       {
							if (pictureResult.Error != null)
							{
								FbDebug.Error(pictureResult.Error);
							}
							else
							{
								friendImages.Add(userId, pictureResult.Texture);
							}
						}
				);
			}*/
		}
		
		// Now sort the entries based on score (lowest = top ranking)
		scoresList.Sort(delegate(object firstObj,
		                     object secondObj)
		            {
			// return 1 if firstObj is greater, 0 if equal, -1 if secondObj is greater
			//myList.Sort((t1, t2) => t1.ID.CompareTo(t2.ID));
			return (Convert.ToInt32(((Dictionary<string,object>)firstObj)["score"])).CompareTo(Convert.ToInt32(((Dictionary<string,object>)secondObj)["score"]));
					}
		);

		scoresList.ForEach( item => {
			Debug.Log(	(string)	((Dictionary<string,object>)item)	["score"]	);
			}
		);
	}

	void DisplayScores()
	{

	}

	void OnGUI()
	{
		float areaWidth = 500f * Screen.width / GameManager.width;
		float areaHeight = 400f * Screen.height / GameManager.height;
		// Wrap everything in the designated GUI Area
		GUILayout.BeginArea(new Rect((Screen.width - areaWidth) * 0.5f, (Screen.height - areaHeight) * 0.5f, areaWidth, areaHeight));

		GUILayout.Box("test", GUILayout.ExpandHeight(true));
		//GUI.Box(GUILayoutUtility.GetLastRect(), "TEST");

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
			if(GUILayout.Button("Post my scores and compare my scores against friends!"))
			{
				// FB cmd to request and get latest scores after
				GetPermissions();
				// delegate to post scores and get scores
			}
		}
		else
		{
			// Post scores
			// Display scores
			DisplayScores();
		}

		// Begin the singular Horizontal Group
		GUILayout.BeginHorizontal();

		// Arrange two more Controls vertically beside the Button
		GUILayout.BeginVertical();

		// End the Groups and Area
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
		
		
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
		GUILayout.EndHorizontal();
	}
}
