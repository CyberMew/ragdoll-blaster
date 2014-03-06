using UnityEngine;
using System.Collections;

public class PowerBarGuage : MonoBehaviour {

	public Texture2D texture;
	public float percent = 1.0f;
	public float posX = 0f;	// top left
	public float posY = 0f;	// top left
	public float width = 256f;
	public float height = 256f;

	
	// Use this for initialization
	void Start () {
		width = texture.width;
		height = texture.height;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
		if(Event.current.type.Equals(EventType.Repaint))
		{
			// Source is bottom left
			Graphics.DrawTexture(new Rect(posX,posY + (height - (height * percent)),width,height * percent), texture, new Rect(0,0,1f,percent), 0,0,0,0);
		}
	}
}
