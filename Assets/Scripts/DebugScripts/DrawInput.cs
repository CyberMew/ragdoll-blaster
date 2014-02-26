using UnityEngine;
using System.Collections;

public class DrawInput : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
		if(InputManager.GetIsInputPressed())
		{
			Debug.Log("Drawing line from " + (InputManager.GetCurrentPosition() - InputManager.GetCurrentDragOffset()).ToString() + " to " + InputManager.GetCurrentPosition().ToString());
			Drawing.DrawLine(InputManager.GetCurrentPosition() - InputManager.GetCurrentDragOffset(), InputManager.GetCurrentPosition());
		}
	}
}
