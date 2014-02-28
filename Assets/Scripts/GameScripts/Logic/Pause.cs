using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour {

	public GameObject PauseMenuObject;
	Sprite DefaultSprite;
	public Sprite HoverSprite;
	public Sprite PushedSprite;

	// Use this for initialization
	void Start () {
		DefaultSprite = GetComponent<SpriteRenderer>().sprite;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
		// Skip draw of pause menu buttons if isTutorialOn is active
		if(GameManager.isTutorialOn == false)
		{
			return;
		}

		// Game is paused, continue drawing menus

	}

	// OnHover
	void OnMouseEnter()
	{
		if(GameManager.isTutorialOn == false)
		{
			//GetComponent<SpriteRenderer>().sprite = HoverSprite;
		}
	}	

	// OnLeaving
	void OnMouseExit()
	{
		if(GameManager.isTutorialOn == false)
		{
			//GetComponent<SpriteRenderer>().sprite = DefaultSprite;
		}
	}

	// Being pushed
	void OnMouseDown()
	{
		if(GameManager.isTutorialOn == false)
		{
			//GetComponent<SpriteRenderer>().sprite = PushedSprite;
		}
	}

	// Clicked and released
	void OnMouseUpAsButton()
	{
		if(GameManager.isTutorialOn == false)
		{
			GameManager.isPaused = true;
		}
	}
}
