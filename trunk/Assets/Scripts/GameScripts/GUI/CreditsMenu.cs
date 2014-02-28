using UnityEngine;
using System.Collections;

public class CreditsMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
		GUI.Label(new Rect(Screen.width * 0.5f, Screen.height * 0.5f,100,100), "This is a demo project done by XXXXXXXXXXXXXXXXXXXX");
	}

	// Someone disabled/close my credits logic/menu
	void OnDisable()
	{
		// Report to my parent button, and they should them to renable the pause menus
		gameObject.transform.parent.gameObject.GetComponent<ButtonCredits>().EnableAllButtonsInputs(true);		
	}
}
