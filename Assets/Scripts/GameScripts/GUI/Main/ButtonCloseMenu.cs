using UnityEngine;
using System.Collections;

public class ButtonCloseMenu : MonoBehaviour {

	public bool TapAnywhereToClose = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(TapAnywhereToClose && InputManager.GetIsInputDown())
		{
			OnMouseUpAsButton();
		}
		// Due to Unity bug(?) I have to workaround the problem manually
		if(InputManager.GetIsInputDown() && collider2D.OverlapPoint(Camera.main.ScreenToWorldPoint(InputManager.GetCurrentPositionScreenSpace())))
		{
			OnMouseUpAsButton();
		}
	}
	
	void OnMouseUpAsButton()
	{
		// Set my parent game object to disabled
		gameObject.transform.parent.gameObject.SetActive(false);

		Debug.Log("not firing FK");
	}
}
