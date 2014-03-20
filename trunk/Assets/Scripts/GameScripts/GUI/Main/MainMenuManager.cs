using UnityEngine;
using System.Collections;

public class MainMenuManager : MonoBehaviour {


	// Singleton
	private static MainMenuManager instance;
	
	// Constuct
	private MainMenuManager() {}
	
	// Instance
	public static MainMenuManager Instance
	{
		get
		{
			if (instance == null)
				instance = GameObject.FindObjectOfType(typeof(MainMenuManager)) as MainMenuManager;
			else
			{
				Debug.Log("No such object!");
			}
			
			return instance;
		}
		
	}
	/************ end of Singleton**************/


	public GameObject m_CreditBtn  = null;
	public GameObject m_OptionsBtn = null;
	public GameObject m_StartBtn   = null;
	public GameObject m_Ttile 	   = null;

	private float m_TweenInTime;
	private Vector3 m_InitalPos; 

	public enum MainMenuState
	{
		Idle,
		ShowMenu,
		Start,
		GoBack,
		Options,
		Credites,
		InGame,

	}
	private MainMenuState m_MMState = MainMenuState.Idle;
	public void SetMainMenuState(MainMenuState _s)
	{
		m_MMState = _s;
	}

	// Use this for initialization
	void Start () {

		m_InitalPos = transform.position;
		m_MMState = (GameManager.isInGame)? MainMenuState.Idle : MainMenuState.ShowMenu;


	}
	
	// Update is called once per frame
	void Update () {
	
		Debug.Log("Current Menu State: " + m_MMState);
	switch(m_MMState)
	{
			
		case MainMenuState.Idle:
		   	 break;
		case MainMenuState.ShowMenu:
			ShowMenuAction();
			 break;

		case MainMenuState.Start:
			 StartAction();
			 break;
		case MainMenuState.Options:
			//transform.FindChild("ButtonOptions").GetComponent<ButtonOptions>().SetOptionsWindow();
			OptionsAction();
			m_MMState = MainMenuState.Idle;
		break;
	
	}
	m_MMState = MainMenuState.Idle;

	}


	// set disable/enable children buttons.
	private void ActiveButtons(bool inputs)
	{
		GameObject go;
		for(int i = 0; i < gameObject.transform.childCount; ++i)
		{
			go = gameObject.transform.GetChild(i).gameObject;

			if(go.collider2D)
				go.collider2D.enabled = inputs;//.GetComponent<Button>().AcceptInputs(inputs);

			go.GetComponent<SpriteRenderer>().enabled = inputs;
		
		}
	}


	private void ShowMenuAction()
	{
		ActiveButtons(true);
		transform.position = m_InitalPos;
		if(m_StartBtn && m_OptionsBtn && m_CreditBtn)
		{
			LeanTween.moveLocalY(this.gameObject, -1.2f, 0.5f)
					 .setEase( LeanTweenType.easeOutQuad);

		}
	}


	private void StartAction()
	{
		GameManager.GoToNextLevel("Level0" );// + PlayerPrefs.GetInt("LastPlayedLevel").ToString());
		GameManager.isInGame = true;
	}

	private void OptionsAction()
	{
		ActiveButtons(false);

		GameObject _opWin = GameObject.Find("OptoinsWindow");
		if(_opWin)
		{
			LeanTween.moveY(_opWin, 1f, 0.5f)
				     .setEase( LeanTweenType.easeInOutExpo);

		}
	}



}






