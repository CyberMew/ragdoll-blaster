using UnityEngine;
using System.Collections;

public class PauseButtonsManager : MonoBehaviour {

	public GameObject[] listOfAffectedButtons;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// This is for direct child enable/disable
	public void SetAllChildButtonsInput(bool inputs)
	{
		GameObject go;
		for(int i = 0; i < listOfAffectedButtons.Length; ++i)
		{
			go = listOfAffectedButtons[i];

			if(go.collider2D)
			{//todo: renable back becuase the guitext already provides thie collider crap
				//go.collider2D.enabled = inputs;//.GetComponent<Button>().AcceptInputs(inputs);
			}
			if(go.GetComponent<SpriteRenderer>())
			{
				go.GetComponent<SpriteRenderer>().enabled = inputs;
			}
			// todo: remove this text bullshit
			if(go.guiText)
			{
				go.guiText.enabled = inputs;
			}
		}
	}
	void OnEnable()
	{
		SetAllChildButtonsInput(true);
	}

	void OnDisable()
	{
		// If closed by someone like CloseButton - means pause menu is closed.
		
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
