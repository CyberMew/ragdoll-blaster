using UnityEngine;
using System.Collections;

public class PauseButton : MonoBehaviour {

	public GameObject PauseMenuObject;
	Sprite DefaultSprite;
	public Sprite HoverSprite;
	public Sprite PushedSprite;

	private bool isClickedBefore = false;
	
	public AudioClip hover;
	public AudioClip click;
	public AudioClip release;
	private AudioSource audioPlayer;

	// Use this for initialization
	void Start () {
		DefaultSprite = GetComponent<SpriteRenderer>().sprite;
		audioPlayer = Camera.main.audio;
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

	void OnApplicationPause()
	{
#if !UNITY_EDITOR
		OnMouseUpAsButton();
#endif
	}

	// OnHover
	void OnMouseEnter()
	{
		if(GameManager.IsGamePaused() == false)
		{
			GetComponent<SpriteRenderer>().sprite = HoverSprite;
			audioPlayer.PlayOneShot(hover);
		}
		//GameManager.isUIBusy = true;
	}	

	// OnLeaving
	void OnMouseExit()
	{
		if(GameManager.IsGamePaused() == false)
		{
			GetComponent<SpriteRenderer>().sprite = DefaultSprite;
		}
		//GameManager.isUIBusy = false;
	}

	// Being pushed
	void OnMouseDown()
	{
		if(GameManager.IsGamePaused() == false)
		{
			GetComponent<SpriteRenderer>().sprite = PushedSprite;
			isClickedBefore = true;
			audioPlayer.PlayOneShot(click);
		}
		Debug.Log("test");
		GameManager.isUIBusy = true;
	}

	// Clicked and released
	void OnMouseUpAsButton()
	{
		if(GameManager.IsGamePaused() == false && isClickedBefore)
		{
			isClickedBefore = false;
			GameManager.isPaused = true;
			Time.timeScale = 0f;
			// Bring up the pause menu object
			//PauseMenuObject.GetComponent<PauseButtonsManager>().SetAllChildButtonsInput(true);
			PauseMenuObject.SetActive(true);
			audioPlayer.PlayOneShot(release);
		}
		GameManager.isUIBusy = true;
	}

	public void Reset()
	{
		GetComponent<SpriteRenderer>().sprite = DefaultSprite;
		// todo: Check if the mouse is still hovering over the sprite. If it is, set it to hoversprite because the OnMouseOver will not activate
		//GetComponent<SpriteRenderer>().sprite = HoverSprite;
		GameManager.isUIBusy = false;
	}
}
