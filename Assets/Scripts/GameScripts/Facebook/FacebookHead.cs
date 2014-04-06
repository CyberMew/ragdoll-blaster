using UnityEngine;
using System.Collections;

public class FacebookHead : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if(FBUtils.IsLoggedIn && User.profilePicture.width > 1)
		{
			GetComponent<SpriteRenderer>().sprite = Sprite.Create(User.profilePicture, new Rect(0,0,User.profilePicture.width,User.profilePicture.height), new Vector2(0.5f,0.5f), 72f);
			transform.localScale *= 0.75f;
			((BoxCollider2D) collider2D).size = new Vector2(1,1);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
