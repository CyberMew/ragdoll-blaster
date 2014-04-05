using UnityEngine;
using System.Collections;

class EnableFacebookButton : Button {

	public GameObject menuToEnable;
	public GameObject menuToDisable;

	// Use this for initialization
	void Start () {
		
		// Make sure it's ok to initialize facebook first
		//Invoke("GetUrlAndInitFB", 0.1f);
		GetUrlAndInitFB();
	}
	
	// Update is called once per frame
	new void Update () {
		base.Update();
	
	}
	
	internal override void ButtonAction()
	{
		menuToEnable.SetActive(true);
		collider2D.enabled = false;
		menuToDisable.SetActive(false);
	}

	public void Reset()
	{
		if(this != null)
		{
		menuToEnable.SetActive(false);
		collider2D.enabled = true;
		menuToDisable.SetActive(true);
		}
	}
	
	void GetUrlAndInitFB()
	{
		/*Application.ExternalEval(
			"var u = UnityObject2.instances[0].getUnity();" +
			"if(u == null) { console.log('Unity is not yet loaded'); } " +
			"console.log('testing console log anyway'); " +
			"u.SendMessage('" + gameObject.name + "', 'InitializeForFacebook', document.URL);"
			);*/
		//	Application.ExternalEval(
		//	"UnityObject2.instances[0].getUnity().SendMessage('" + gameObject.name + "', 'InitializeForFacebook', window.top.location.href);"
		//	);
		#if UNITY_WEBPLAYER && !UNITY_EDITOR
		Application.ExternalEval("UnityObject2.instances[0].getUnity().SendMessage(\"" + name + "\", \"InitializeForFacebook\", document.URL);");
		#else
		InitializeForFacebook("");
		#endif
	}
	
	void InitializeForFacebook(string url)
	{
		Debug.Log("Finally gotten the url, can proceed to init: "  + url);
		
		// Processing for the url
		//todo: verify if we are in facebook!
		FBUtils.currentURL = url;

		
		#if UNITY_WEBPLAYER && !UNITY_EDITOR
		//"UnityObject2.instances[0].getUnity().SendMessage('FacebookCallbackGO', 'InitializeForFacebook', window.location.href);"
		string injection =
			"var headerElement = document.createElement('div');" +
				"headerElement.textContent = ('This game was made with Android in mind. Pardon us if the art does not look nice or fun - we just want to check out Unity 4.3 new 2D features.<br /> Check out the Android version on Google Play here:');" +
				"var body = document.getElementsByTagName(\"body\")[0];" +
				"var insertionPoint = body.children[0]; " +
				"body.insertBefore(headerElement, insertionPoint);";
		Application.ExternalEval(injection);
		//Application.ExternalEval("alert(navigator.appName);");
		
		// Execute javascript in iframe to keep the player centred		
		string javaScript = @"
	            window.onresize = function() {

	              var unity = UnityObject2.instances[0].getUnity();
	              var unityDiv = document.getElementById(""unityPlayerEmbed"");

	              var width =  window.innerWidth;
	              var height = window.innerHeight;

	              var appWidth = " + GameManager.width + @";
	              var appHeight = " + GameManager.height + @";

	              unity.style.width = appWidth + ""px"";
	              unity.style.height = appHeight + ""px"";

	              unityDiv.style.marginLeft = (width - appWidth)/2 + ""px"";
	              unityDiv.style.marginTop = (height - appHeight)/2 + ""px"";
	              unityDiv.style.marginRight = (width - appWidth)/2 + ""px"";
	              unityDiv.style.marginBottom = (height - appHeight)/2 + ""px"";
	            }
	            window.onresize(); // force it to resize now";
		
		Application.ExternalCall(javaScript);	
		// Alternatively we could call this in WEBPLAYER only
		//FB.Canvas.SetResolution(GameManager.width, GameManager.height, false, 0, FBScreen.CenterVertical(), FBScreen.CenterHorizontal());
		
		// Get browser name (http://answers.unity3d.com/questions/26550/browser-version-inside-unity3d.html)
		javaScript = @"
			    window.onresize = function() {
				{
					console.log(navigator.appName);

				}
				window.onresize(); // not suppose to do this!;";
		//Application.ExternalCall(javaScript);
		
		// Do some checks for web version for deciding whether or not to enable Facebook
		if(Application.absoluteURL.Equals("https://ragclone.parseapp.com/ragclone.unity3d") == false)
		{
			FBUtils.isFBAvailable = false;
			Debug.Log("This game file could be running locally! Please run it on Facebook instead! Disabling Facebook features.");
			// Facebook is only available 
		}
		if(FBUtils.currentURL.Contains("https://ragclone.parseapp.com"))
		{
			FBUtils.isFBAvailable = false;
			Debug.Log("This game file could be running directly on where it is hosted! Please run it on Facebook instead! Disabling Facebook features.");
		}
		
		//string name = "FacebookURL";
		//Application.ExternalEval("u.getUnity().SendMessage(<string name of your GameObject>, <string name of your method in the GameObject>, location.href);");
		//		Application.ExternalEval("UnityObject2.instances[0].getUnity().SendMessage(\"" + name + "\", \"ReceiveURL\", document.URL);");
		//	string url = "UnityObject2.instances[0].getUnity().SendMessage(\"" + name + "\", \"ReceiveURL\", document.URL);";
		//Debug.Log("Calling:" + url);
		
		//FB.AppId;
		#endif

		if(FBUtils.isFBAvailable)
		{
			GameManager.InitializeFacebook();
		}
		else
		{
			gameObject.SetActive(false);
		}
	}

}
