using UnityEngine;
using System.Collections;

public class PauseButtonsManager : MonoBehaviour {



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetAllChildButtonsInput(bool inputs)
	{
		GameObject go;
		for(int i = 0; i < gameObject.transform.childCount; ++i)
		{
			go = gameObject.transform.GetChild(i).gameObject;

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
}
