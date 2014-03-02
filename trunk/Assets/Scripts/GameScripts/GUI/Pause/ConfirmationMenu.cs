using UnityEngine;
using System.Collections;

public class ConfirmationMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDisable()
	{
		// Receive message about current menu being disabled, so we are closed and have to show the pause menu buttons
		gameObject.transform.parent.gameObject.GetComponent<ButtonConfirmation>().EnableAllPauseButtonsInputs(true);
	}
}
