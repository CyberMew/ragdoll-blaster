using UnityEngine;
using System.Collections;

public class HowToPlayMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
/* todo: Remove me!
	void OnGUI()
	{
		GUI.Label(new Rect(Screen.width * 0.5f, 200f,100,100), "Some completed how to play code here.");
	}
*/	
	// Someone disabled/close my HowToPlay logic/menu
	void OnDisable()
	{
		// Report to my parent button, and they should them to renable the pause menus
		gameObject.transform.parent.gameObject.GetComponent<ButtonHowToPlay>().EnableAllPauseButtonsInputs(true, true);		
	}
}
