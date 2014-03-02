using UnityEngine;
using System.Collections;

class ButtonResume : Button {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {		
		#if UNITYSTANDALONE_WIN || UNITY_EDITOR
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			gameObject.transform.parent.GetComponent<PauseButtonsManager>().SetAllChildButtonsInput(false);
			ButtonAction();
		}
		#endif
	}

	internal override void ButtonAction()
	{
		// Resume the game
		GameManager.isPaused = false;
		GameObject go = GameObject.Find("PauseButton");
		if(go)
		{
			go.GetComponent<PauseButton>().Reset();
		}
		Time.timeScale = 1f;
	}
}
