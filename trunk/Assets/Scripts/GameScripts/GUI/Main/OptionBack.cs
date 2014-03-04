using UnityEngine;
using System.Collections;

class OptionBack : Button {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	internal override void ButtonAction()
	{
		this.transform.parent.gameObject.SetActive(false);
		MainMenuManager.Instance.ActiveButtons(true);
	}
}
