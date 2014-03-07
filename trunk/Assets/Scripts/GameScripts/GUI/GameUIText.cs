using UnityEngine;
using System.Collections;

public class GameUIText : MonoBehaviour {
	public Font customFont;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
		GUISkin prevSkin = GUI.skin;
		GUIStyle lblStyle = new GUIStyle(prevSkin.GetStyle("Label"));
		lblStyle.alignment = TextAnchor.MiddleCenter;
		lblStyle.font = customFont;
		lblStyle.normal.textColor = new Color(0.5f, 0.5f, 0.5f, 1f);
		lblStyle.fontSize = 30;
		GUI.Label(new Rect(0f, 0f, Screen.width, 100f), "Total shots: " + (GameManager.totalShots + GameManager.tempShots).ToString(), lblStyle);
		
		lblStyle.alignment = TextAnchor.MiddleRight;
		lblStyle.padding.right = 20;
		string text = "Tutorial";
		if(GameManager.currLevel != 0)
		{
			text = GameManager.currLevel.ToString();
		}
		GUI.Label(new Rect(0f, 0f, Screen.width, 100f), "Level: " + text, lblStyle);
	}
}
