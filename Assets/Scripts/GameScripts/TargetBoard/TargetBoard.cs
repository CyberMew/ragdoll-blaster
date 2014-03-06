using UnityEngine;
using System.Collections;

public class TargetBoard : MonoBehaviour {

	private bool levelComplete;

	// Use this for initialization
	void Start () {
		levelComplete = false;
	}
	
	// Update is called once per frame
	void Update () {
#if UNITY_EDITOR
		// todo: remove this code when done
		if(Input.GetKeyDown(KeyCode.P))
			Debug.Log("Graphics Level:  " + QualitySettings.GetQualityLevel());
		// todo: remove this code when done
		if(Input.GetKeyDown(KeyCode.N))
		{
			GameManager.isGameWon = true;
			GameManager.GoToNextLevel();
		}
#endif
	}

	// This will not work, since if we enable Trigger, then it doesn't collide with other rigidbodies
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.CompareTag("Bullet"))
		{
			Debug.Log("Something hit the target board!");
		}
	}
		
	void OnCollisionEnter2D(Collision2D other)
	{
		// We do not want to execute if it hits the walls as well! Only bullet works!
		if(other.gameObject.CompareTag("Bullet") && levelComplete == false)
		{
			levelComplete = true;

			GameManager.isGameWon = true;
			GameManager.GoToNextLevel();

			#if UNITY_IPHONE || UNITY_ANDROID
			Handheld.Vibrate();
			#endif
		}
	}
}
