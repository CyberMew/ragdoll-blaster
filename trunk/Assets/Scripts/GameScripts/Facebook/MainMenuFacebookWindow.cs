using UnityEngine;
using System.Collections;

public class MainMenuFacebookWindow : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDisable()
	{
		//transform.parent.collider2D.enabled = true;
		transform.parent.GetComponent<EnableFacebookButton>().Reset();
	}
}
