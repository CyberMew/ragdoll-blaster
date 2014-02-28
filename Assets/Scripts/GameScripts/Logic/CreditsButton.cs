using UnityEngine;
using System.Collections;

public class CreditsButton : MonoBehaviour {

	Sprite DefaultSprite;
	public Sprite HoverSprite;
	public Sprite PushedSprite;

	private bool isClickedBefore = false;

	// Use this for initialization
	void Start () {
		//DefaultSprite = GetComponent<SpriteRenderer>().sprite;
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnGUI()
	{
	}

	// OnHover
	void OnMouseEnter()
	{
		//GetComponent<SpriteRenderer>().sprite = HoverSprite;
	}	

	// OnLeaving
	void OnMouseExit()
	{
		//GetComponent<SpriteRenderer>().sprite = DefaultSprite;
	}

	// Being pushed
	void OnMouseDown()
	{
		if(GameManager.IsGamePaused() == false)
		{
			//GetComponent<SpriteRenderer>().sprite = PushedSprite;
			isClickedBefore = true;
		}
	}

	// Clicked and released
	void OnMouseUpAsButton()
	{
		if(GameManager.IsGamePaused() == false && isClickedBefore)
		{
			isClickedBefore = false;


		}
		GameManager.isUIBusy = true;
	}
}
