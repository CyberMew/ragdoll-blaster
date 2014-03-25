﻿using UnityEngine;
using System.Collections;

abstract class Button : MonoBehaviour {

	protected Sprite DefaultSprite;
	public Sprite HoverSprite = null;
	public Sprite PushedSprite = null;

	public GameObject MenuToOpen = null;
		
	public AudioClip hover;
	public AudioClip click;
	public AudioClip release;

	protected bool isClickedBefore = false;
	//protected bool acceptInputs = false;

	private AudioSource audioPlayer;

	void Awake()
	{
		DefaultSprite = GetComponent<SpriteRenderer>().sprite;
		audioPlayer = Camera.main.audio;
		if(audioPlayer == null)
		{
			audioPlayer = Camera.main.gameObject.AddComponent<AudioSource>();
			Debug.Log("Camera does not have audio source, adding one now.");
		}
		if(hover == null)
		{
			Debug.Log("(" + gameObject.name + ") hover not supplied: loading defaults");
			hover = Resources.Load("/Audio/rollover1") as AudioClip;
		}
		if(click == null)
		{
			Debug.Log("(" + gameObject.name + ") click not supplied: loading defaults");
			click = Resources.Load("/Audio/click1") as AudioClip;
		}
		if(release == null)
		{
			Debug.Log("(" + gameObject.name + ") release not supplied: loading defaults");
			release = Resources.Load("/Audio/mouserelease1") as AudioClip;
		}
		if(HoverSprite == null)
		{
			HoverSprite = DefaultSprite;
		}
		if(PushedSprite == null)
		{
			PushedSprite = DefaultSprite;
		}
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
			GetComponent<SpriteRenderer>().sprite = HoverSprite;
		}
		audioPlayer.PlayOneShot(hover);

	}	
	
	// OnLeaving
	void OnMouseExit()
	{
		//if(acceptInputs)
		{
			GetComponent<SpriteRenderer>().sprite = DefaultSprite;
		}
	}
	
	// Being pushed
	void OnMouseDown()
	{
		//if(acceptInputs == false)
		{
			GetComponent<SpriteRenderer>().sprite = PushedSprite;
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

	public void EnableAllPauseButtonsInputs(bool inputs, bool isVisible)
	{
		gameObject.transform.parent.GetComponent<PauseButtonsManager>().SetAllChildButtonsInput(inputs, isVisible);
	}

	// Force them to implement behaviour of the button behavior when clicked
	internal abstract void ButtonAction();



}
