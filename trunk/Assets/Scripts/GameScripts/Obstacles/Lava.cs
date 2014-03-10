using UnityEngine;
using System.Collections;

public class Lava : MonoBehaviour {

	private bool isTriggered;

	public GameObject fireSmokeObj;

	// Use this for initialization
	void Start () {
		isTriggered = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		//if(isTriggered == false)
		{
			Debug.Log("Something triggered the LAVA!");
			//isTriggered = true;

			if(other.CompareTag("Bullet"))
			{
				// Only affect the body parts in Lava!
				other.tag = "Untagged";	// We don't want Bullet to collide with target in Lava by accident
				// Create firesmoke
				fireSmokeObj.SetActive(true);
				Instantiate(fireSmokeObj, other.gameObject.transform.position, Quaternion.identity);
				fireSmokeObj.SetActive(false);

				SetLowerGravity(other.transform.root.GetChild(0));
			}

			if(other.CompareTag("Target"))
			{
				// Create firesmoke
				fireSmokeObj.SetActive(true);
				Instantiate(fireSmokeObj, other.gameObject.transform.position, Quaternion.identity);
				fireSmokeObj.SetActive(false);

				// Remove forces and let it sink down slowly
				other.rigidbody2D.velocity = Vector2.zero;
				other.rigidbody2D.angularVelocity = 0f;
				// Slow down gravity for that object
				other.rigidbody2D.gravityScale = 0.05f;

				// Restart the level
				Application.LoadLevel(Application.loadedLevel);
				Debug.Log("Restart leveling, target destroyed");
			}
		}
	}

	
	void SetLowerGravity(Transform rootTransform)
	{
		GameObject go;
		for(int i = 0; i < rootTransform.childCount; ++i)
		{
			go = rootTransform.GetChild(i).gameObject;
			// Remove forces and let it sink down slowly
			go.rigidbody2D.velocity = Vector2.zero;
			go.rigidbody2D.angularVelocity = 0f;
			// Slow down gravity for that object
			go.rigidbody2D.gravityScale = 0.05f;
		//	go.tag = "Untagged";	// We don't want Bullet to collide with target in Lava by accident
			
			// Repeat for all child
			SetLowerGravity(go.transform);
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
