using UnityEngine;
using System.Collections;

public class TargetBoard : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// This will not work, since if we enable Trigger, then it doesn't collide with other rigidbodies
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.CompareTag("Bullet"))
		{
			//GameManager.GoToNextLevel();
			Debug.Log("Something hit the target board!");
		}
	}
		
	void OnCollisionEnter2D(Collision2D other)
	{
		if(other.gameObject.CompareTag("Bullet"))
		{
			//GameManager.GoToNextLevel();
			Debug.Log("Something hit the target board!");
		}
	}
}
