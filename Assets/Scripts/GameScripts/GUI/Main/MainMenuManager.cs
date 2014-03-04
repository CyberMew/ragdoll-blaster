using UnityEngine;
using System.Collections;

public class MainMenuManager : MonoBehaviour {


	// Singleton
	private static MainMenuManager instance;
	
	// Constuct
	private MainMenuManager() {}
	
	// Instance
	public static MainMenuManager Instance
	{
		get
		{
			if (instance == null)
				instance = GameObject.FindObjectOfType(typeof(MainMenuManager)) as MainMenuManager;
			else
			{
				Debug.Log("No such object!");
			}
			
			return instance;
		}
		
	}
	/************ end of Singleton**************/







	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//disable/ enable specific button.
	public void SetButton(string _childName, bool _input)
	{
		GameObject btn = GameObject.Find(_childName);

		if(btn)
		{
			btn.collider2D.enabled = _input;//.GetComponent<Button>().AcceptInputs(inputs);
			btn.GetComponent<SpriteRenderer>().enabled = _input;
			// todo: remove this text bullshit
			btn.guiText.enabled = _input;
		}
	}


	// set disable/enable children buttons.
	public void ActiveButtons(bool inputs)
	{
		GameObject go;
		for(int i = 0; i < gameObject.transform.childCount; ++i)
		{
			go = gameObject.transform.GetChild(i).gameObject;
			
			go.collider2D.enabled = inputs;//.GetComponent<Button>().AcceptInputs(inputs);
			go.GetComponent<SpriteRenderer>().enabled = inputs;
			// todo: remove this text bullshit
			go.guiText.enabled = inputs;
		}
	}


}






