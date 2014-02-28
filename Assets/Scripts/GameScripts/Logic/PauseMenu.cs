using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if(GameManager.isPaused && (InputManager.GetIsInputDown() || Input.GetKeyDown(KeyCode.A)))
		{
			//this.gameObject.SetActive(false);
			//cannonBarrel.GetComponent<CannonFire>().ReadyToFire(true);
			GameManager.isPaused = false;
			GameObject go = GameObject.Find("PauseButton");
			if(go)
			{
				go.GetComponent<PauseButton>().Reset();
			}
			Time.timeScale = 1f;
		}
	}
	
	void OnGUI()
	{
		if(GameManager.isPaused)
		{
			GUI.Label(new Rect(Screen.width * 0.5f, Screen.height * 0.5f,100,100), "Click to dismiss PAUSE MENU");
		}
	}

	void OnDisable()
	{
	}
}
