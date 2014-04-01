using UnityEngine;
using System.Collections;

class ButtonResume : Button {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	new void Update () {
		base.Update();
		#if UNITYSTANDALONE_WIN || UNITY_EDITOR
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			//EnableAllButtonsInputs(false);
			//gameObject.transform.parent.GetComponent<PauseButtonsManager>().SetAllChildButtonsInput(false);
			ButtonAction();
		}
		#endif
	}

	internal override void ButtonAction()
	{
		// Get PauseMenuManager to reset it
		/*
		// Resume the game
		GameManager.isPaused = false;
		GameObject go = GameObject.Find("PauseButton");
		if(go)
		{
			go.GetComponent<PauseButton>().Reset();
		}
		Time.timeScale = 1f;
*/
		EnableAllPauseButtonsInputs(false, false);
		transform.parent.gameObject.SetActive(false);
	}
}
