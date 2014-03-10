using UnityEngine;
using System.Collections;

public class Lava : MonoBehaviour {

	private bool isTriggered;

	// Use this for initialization
	void Start () {
		isTriggered = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(isTriggered == false && (other.CompareTag("Bullet") || other.CompareTag("Target")))
		{
			Debug.Log("Something triggered the LAVA!");
			isTriggered = true;

			// Remove forces and let it sink down slowly
			other.rigidbody2D.velocity = Vector2.zero;
			other.rigidbody2D.angularVelocity = 0f;

		}
	}
	/*
	void OnCollisionEnter2D(Collision2D other)
	{
		//if(other.gameObject.CompareTag("Bullet"))
		{
			Debug.Log("Something collided the LAVA!");
		}
	}*/
}
