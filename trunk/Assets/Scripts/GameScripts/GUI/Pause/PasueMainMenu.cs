using UnityEngine;
using System.Collections;

class PasueMainMenu :Button {

	private Vector3 m_Position;
	public GameObject Menu_Optioin;
	public GameObject Menu_Show;

	private bool m_isSlideout;
	// Use this for initialization
	void Start () {
		m_Position = new Vector3(transform.position.x, transform.position.y, -1f);
		Menu_Optioin.transform.position = m_Position;
	//	Menu_Show.transform.position 	= m_Position;  

		m_isSlideout = false;

		LeanTween.move(Menu_Show, new Vector3(6,2,-1), 0.5f)
			.setEase( LeanTweenType.easeInOutExpo);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseEnter()
	{

	
	}

	internal override void ButtonAction()
	{
		if(Menu_Show )//&& Menu_Show)
		{
			
			
		//	if(!m_isSlideout)
			//{
				//Menu_Show.transform.position = new Vector3(6,2,-1);
			//	m_isSlideout = true;
				
				
			//}
		}
		//MainMenuManager.Instance.SetMainMenuState(MainMenuManager.MainMenuState.Start);
	}

	private void SetFlag()
	{
		m_isSlideout = true;
	}



}
