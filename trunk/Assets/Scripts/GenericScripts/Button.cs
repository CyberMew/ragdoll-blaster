using UnityEngine;
using System.Collections;

abstract class Button : MonoBehaviour {
	
	Sprite DefaultSprite;
	public Sprite HoverSprite;
	public Sprite PushedSprite;

	public GameObject ButtonMenuLogic;
		
	public AudioClip hover;
	public AudioClip click;
	public AudioClip release;

	protected bool isClickedBefore = false;
	protected bool acceptInputs = false;

	private AudioSource audioPlayer;

	void Awake()
	{
		audioPlayer = Camera.main.audio;
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	/*
	public void AcceptInputs(bool inputs)
	{
		acceptInputs = inputs;
		Debug.Log("Setting input of " + gameObject.name + " to " + inputs.ToString());
	}*/

	
	// OnHover
	void OnMouseEnter()
	{
		//if(acceptInputs)
		{
			//GetComponent<SpriteRenderer>().sprite = HoverSprite;
		}
		audioPlayer.PlayOneShot(hover);
	}	
	
	// OnLeaving
	void OnMouseExit()
	{
		//if(acceptInputs)
		{
			//GetComponent<SpriteRenderer>().sprite = DefaultSprite;
		}
	}
	
	// Being pushed
	void OnMouseDown()
	{
		//if(acceptInputs == false)
		{
			//GetComponent<SpriteRenderer>().sprite = PushedSprite;
			isClickedBefore = true;
		}
		audioPlayer.PlayOneShot(click);
	}
	
	// Clicked and released
	void OnMouseUpAsButton()
	{
		if(isClickedBefore)
		{
			isClickedBefore = false;
			
			audioPlayer.PlayOneShot(release);
			Debug.Log("Disabling all buttons inputs");

			// MainMenu scene is also using this base class - do a check
			/*if(GameManager.isInGame)
			{
				// Get the pause manager script to disable all buttons
				EnableAllButtonsInputs(false); // This should be called in button resume or something
			}*/

			ButtonAction();
		}
	}

	public void EnableAllPauseButtonsInputs(bool inputs)
	{
		gameObject.transform.parent.GetComponent<PauseButtonsManager>().SetAllChildButtonsInput(inputs);
	}

	// Force them to implement behaviour of the button behavior when clicked
	internal abstract void ButtonAction();
	
}
