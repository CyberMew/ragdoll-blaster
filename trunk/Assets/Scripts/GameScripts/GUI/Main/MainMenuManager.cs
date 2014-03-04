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


	// Use this for initialization
	void Start () {
		m_TweenInTime = 0.5f;


		if(m_StartBtn)
		{
			LeanTween.moveLocalX(m_StartBtn, 0.5f, m_TweenInTime).setEase( LeanTweenType.easeOutBounce);
		}
		if(m_OptionsBtn)
		{
			LeanTween.moveLocalX(m_OptionsBtn, 0.5f, m_TweenInTime)
					 .setEase( LeanTweenType.easeOutBounce)
					 .setDelay(m_TweenInTime);
		}
		if(m_CreditBtn)
		{
			LeanTween.moveLocalY(m_CreditBtn, 0.3f, m_TweenInTime)
					 .setEase( LeanTweenType.easeOutBounce)
					 .setDelay(m_TweenInTime*2);
		}
		if(m_Ttile)
		{
			LeanTween.moveLocalY(m_CreditBtn, 0.8f, m_TweenInTime)
				     .setEase( LeanTweenType.easeOutBounce)
					 .setDelay(m_TweenInTime*3);
		}


	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//disable/ enable specific button.
	public void SetButton(string _childName, bool _input)
	{
		GameObject btn = GameObject.Find(_childName);

		if(btn)
		{
			btn.collider2D.enabled = _input;//.GetComponent<Button>().AcceptInputs(inputs);
			btn.GetComponent<SpriteRenderer>().enabled = _input;
			// todo: remove this text bullshit
			btn.guiText.enabled = _input;
		}
	}


	// set disable/enable children buttons.
	public void ActiveButtons(bool inputs)
	{
		GameObject go;
		for(int i = 0; i < gameObject.transform.childCount; ++i)
		{
			go = gameObject.transform.GetChild(i).gameObject;
			
			go.collider2D.enabled = inputs;//.GetComponent<Button>().AcceptInputs(inputs);
			go.GetComponent<SpriteRenderer>().enabled = inputs;
			// todo: remove this text bullshit
			go.guiText.enabled = inputs;
		}
	}


}






