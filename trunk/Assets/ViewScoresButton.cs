using UnityEngine;
using System.Collections;

class ViewScoresButton : Button {

	private ScoreScreen scoreMenu;

	// Use this for initialization
	void Start () {
		scoreMenu = GetComponent<ScoreScreen>() as ScoreScreen;
	}
	
	// Update is called once per frame
	new void Update () {
		base.Update();
	}

	
	internal override void ButtonAction()
	{
		scoreMenu.enabled = true;
	}

}
