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
			Debug.Log("Drawing line from " + (InputManager.GetCurrentPositionScreenSpace() - InputManager.GetCurrentDragOffset()).ToString() + " to " + InputManager.GetCurrentPositionScreenSpace().ToString());
			Drawing.DrawLine(InputManager.GetCurrentPositionScreenSpace() - InputManager.GetCurrentDragOffset(), InputManager.GetCurrentPositionScreenSpace());
		}
	}
}
