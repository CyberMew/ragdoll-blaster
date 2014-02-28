using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour {

	public GameObject cannonBarrel;

	// Use this for initialization
	void Start () {
		//Time.timeScale = 0f;
		//cannonBarrel.GetComponent<CannonFire>().ReadyToFire(false);
		GameManager.isTutorialOn = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(InputManager.GetIsInputDown())
		{
			Destroy(this.gameObject);
			//cannonBarrel.GetComponent<CannonFire>().ReadyToFire(true);
		}
	}

	void OnDestroy()
	{
		GameManager.isTutorialOn = false;
	}

	void OnGUI()
	{
		GUI.Label(new Rect(Screen.width * 0.5f, Screen.height * 0.5f,100,100), "Click to dismiss Tutorial");
	}
}
