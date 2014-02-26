using UnityEngine;
using System.Collections;
using System.Collections.Generic;	// Queue

public class CannonFire : MonoBehaviour {

	public GameObject BulletPrefab;
	Queue<GameObject> bullets = new Queue<GameObject>();
	GameObject parent;

	// Use this for initialization
	void Start () {
		// Create a master parent object for cannons - less mess in hierachy
		parent = new GameObject("Cannons");

		GameObject temp;
		// Reuse the 5 bullets that we have right now
		for(int i = 0; i < 5; ++i)
		{
			temp = Instantiate(BulletPrefab, gameObject.transform.position, Quaternion.identity) as GameObject;
			temp.transform.parent = parent.transform;
			temp.SetActive(false);
			bullets.Enqueue(temp);
		}
	}
	
	// Update is called once per frame
	void Update () {
		// Rotate the sprite to face the mouse when input depressed
		if(InputManager.GetIsInputPressed())
		{
			Vector2 worldPt = Camera.main.ScreenToWorldPoint(InputManager.GetCurrentPosition());
			// Determine the angle of the line.
			float angle = Mathf.Atan2(worldPt.y, worldPt.x);
			Debug.Log(angle.ToString());
			gameObject.transform.rotation = Quaternion.Euler(new Vector3(0,0,angle));
		}

		// Release it according to drag power
		if(InputManager.GetIsInputReleased())
		{
			// Remove oldest bullet
			GameObject oldestBullet = bullets.Dequeue();

			// Reset the bullet - remove force especially, and rotation of object?
			//oldestBullet.rigidbody2D
			// Determine spawn point of bullet based on angle and offset
			//oldestBullet.transform.position = gameObject.transform.position + ;
			// Fire the bullet
			oldestBullet.SetActive(true);
			oldestBullet.rigidbody2D.AddForce(InputManager.GetCurrentDragOffset());

			// Insert oldest bullet to the back as the freshest
			bullets.Enqueue(oldestBullet);
		}
	
	}
}
