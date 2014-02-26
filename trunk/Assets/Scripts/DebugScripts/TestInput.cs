using UnityEngine;
using System.Collections;

public class TestInput : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		/*if(InputManager.GetIsInputDown())
		{
			InputManager.GetCurrentOffset();
		}
		if(InputManager.GetIsInputReleased())
		{
		}*/
		//Debug.Log(InputManager.GetCurrentOffset());
		InputManager.PrintFingersDetected();
		//InputManager.GetCurrentPosition();
	}
}
