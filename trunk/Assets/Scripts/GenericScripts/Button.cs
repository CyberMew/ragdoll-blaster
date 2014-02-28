using UnityEngine;
using System.Collections;

abstract class Button : MonoBehaviour {
	
	Sprite DefaultSprite;
	public Sprite HoverSprite;
	public Sprite PushedSprite;
	public GameObject ButtonMenuLogic;

	protected bool isClickedBefore = false;
	protected bool acceptInputs = false;

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
	}
	
	// Clicked and released
	void OnMouseUpAsButton()
	{
		if(isClickedBefore)
		{
			isClickedBefore = false;
			
			Debug.Log("Disabling all buttons inputs");
			//gameObject.transform.parent.GetComponent<PauseButtonsManager>().SetAllChildButtonsInput(false);
			EnableAllButtonsInputs(false);

			ButtonAction();
		}
	}

	public void EnableAllButtonsInputs(bool inputs)
	{
		gameObject.transform.parent.GetComponent<PauseButtonsManager>().SetAllChildButtonsInput(inputs);
	}

	internal abstract void ButtonAction();


	///{
		//Debug.LogWarning("ButtonAction for button " + gameObject.name + " is undefined.");
	//}
}
