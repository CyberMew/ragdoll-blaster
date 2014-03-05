using UnityEngine;
using System.Collections;

// This class is used only for Texture2DImportSettings.cs
public class SetSpritePivot : MonoBehaviour {

	public Texture2D texture;

	// Use this for initialization
	void Start () {
		GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0f), 72f);
		Destroy(this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
