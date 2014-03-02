using UnityEngine;
using System.Collections;

 class HighButton : Button {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	internal override void ButtonAction()
	{
		GameManager.graphicQuality = 5;

	}

}
