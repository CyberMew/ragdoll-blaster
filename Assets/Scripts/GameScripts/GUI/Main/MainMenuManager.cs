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


	public enum MainMenuState
	{
		Idle,
		GoBack,
		Options,
		Credites,
		InGame,

	}
	private MainMenuState m_MMState = MainMenuState.Idle;
	public MainMenuState MM_STATES
	{
		get{ return m_MMState;}
		set{ m_MMState = value;}
	}

	// Use this for initialization
	void Start () {
		m_MMState = MainMenuState.Idle;


		m_TweenInTime = 0.3f;

		if(m_StartBtn && m_OptionsBtn && m_CreditBtn)
		{
			LeanTween.moveLocalY(this.gameObject, -1.2f, m_TweenInTime)
									 .setEase( LeanTweenType.easeOutQuad)
									 .setDelay(m_TweenInTime);
		}

//		if(m_StartBtn)
//		{
//			LeanTween.moveLocalX(m_StartBtn, 0.5f, m_TweenInTime).setEase( LeanTweenType.easeOutBounce);
//		}
//		if(m_OptionsBtn)
//		{
//			LeanTween.moveLocalX(m_OptionsBtn, 0.5f, m_TweenInTime)
//					 .setEase( LeanTweenType.easeOutBounce)
//					 .setDelay(m_TweenInTime);
//		}
//		if(m_CreditBtn)
//		{
//			LeanTween.moveLocalY(m_CreditBtn, 0.3f, m_TweenInTime)
//					 .setEase( LeanTweenType.easeOutBounce)
//					 .setDelay(m_TweenInTime*2);
//		}
//		if(m_Ttile)
//		{
//			LeanTween.moveLocalY(m_CreditBtn, 0.8f, m_TweenInTime)
//				     .setEase( LeanTweenType.easeOutBounce)
//					 .setDelay(m_TweenInTime*3);
//		}


	}
	
	// Update is called once per frame
	void Update () {

	//switch(m_MMState)
	//{
	//	case MainMenuState.Idle:
	//	break;
	//
	//	case MainMenuState.GoBack:
	//		ActiveButtons(true);
	//		transform.FindChild("ButtonOptions").FindChild("OptionsWindow").gameObject.SetActive(false);
	//		m_MMState = MainMenuState.Idle;
	//	break;
	//
	//	case MainMenuState.Options:
	//		transform.FindChild("ButtonOptions").GetComponent<ButtonOptions>().SetOptionsWindow();
	//		m_MMState = MainMenuState.Idle;
	//	break;
	//
	//	case MainMenuState.Credites:
	//	break;
	//
	//	case MainMenuState.InGame:
	//	break;
	//
	//}


	}

	//disable/ enable specific button.
	public void SetButton(string _childName, bool _input)
	{
		GameObject btn = GameObject.Find(_childName);

		if(btn)
		{
			btn.collider2D.enabled = _input;//.GetComponent<Button>().AcceptInputs(inputs);
			btn.GetComponent<SpriteRenderer>().enabled = _input;
	
		}
	}


	// set disable/enable children buttons.
	public void ActiveButtons(bool inputs)
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


}






