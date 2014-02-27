using UnityEngine;
using System.Collections;
using System.Collections.Generic;	// Queue

public class CannonFire : MonoBehaviour {

	public GameObject BulletPrefab;
	Queue<GameObject> bullets = new Queue<GameObject>();
	GameObject parent;

	public const float MIN_POWER = 500f;
	public int maxPoolObjects = 5;
	public float spawnRadius = 1f;

	// Use this for initialization
	void Start () {
		// Create a master parent object for cannons - less mess in hierachy
		parent = new GameObject("Cannons");

		GameObject temp;
		// Reuse the 5 bullets that we have right now
		for(int i = 0; i < maxPoolObjects; ++i)
		{
			temp = Instantiate(BulletPrefab, gameObject.transform.position, Quaternion.identity) as GameObject;
			temp.transform.parent = parent.transform;
			temp.SetActive(false);
			bullets.Enqueue(temp);
		}
	}

	float angle = 0f;
	// Update is called once per frame
	void Update () {
		// Get the bullet from queue, and make it appear inside the cannon
		if(InputManager.GetIsInputDown())
		{
			GameObject oldestBullet = bullets.Peek();
			// Enable the object so it is visible
			oldestBullet.SetActive(true);
			// Make the human inanimate
			SetStill(oldestBullet.transform);
			// Copy transform data from Prefab (reset it's body parts correctly)
			ResetFromPrefab(oldestBullet.transform);
		}

		// Rotate the sprite to face the mouse when input depressed
		if(InputManager.GetIsInputPressed())
		{
			Vector2 mouseWorldPt = Camera.main.ScreenToWorldPoint(InputManager.GetCurrentPosition());
			mouseWorldPt.x += Mathf.Abs(gameObject.transform.position.x);	// hardcoded because we know gameobject in negative region
			mouseWorldPt.y += Mathf.Abs(gameObject.transform.position.y);	// hardcoded because we know gameobject in negative region
			//mouseWorldPt.x -= gameObject.transform.position.x;	// hardcoded because we know gameobject in negative region
			//mouseWorldPt.y -= gameObject.transform.position.y;	// hardcoded because we know gameobject in negative region
			// Determine the angle of the line.
			angle = Mathf.Atan2(mouseWorldPt.y, mouseWorldPt.x) * Mathf.Rad2Deg;
			// Rotate about the z axis by angle degrees
			gameObject.transform.rotation = Quaternion.Euler(0f, 0f, angle);

			
			GameObject oldestBullet = bullets.Peek();
			// Reset position of whole object only, not torso (the rest will follow!)
			Vector2 cannonToMouse = (InputManager.GetCurrentPosition() - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y)).normalized;
			Vector2 newRelPos = cannonToMouse * spawnRadius;
			//oldestBullet.transform.position = gameObject.transform.position + new Vector3(newRelPos.x, newRelPos.y, 0f);
			oldestBullet.transform.FindChild("torso").position = gameObject.transform.position + new Vector3(newRelPos.x, newRelPos.y, 0f);
			//oldestBullet.transform.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
			oldestBullet.transform.FindChild("torso").transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);

			//ResetFromPrefab(oldestBullet.transform);
		}

		// Release it according to drag power
		if(InputManager.GetIsInputReleased())
		{
			// Remove oldest bullet
			GameObject oldestBullet = bullets.Dequeue();

			// Reset the bullet - remove force especially, and rotation of object?
			//FIXME: need to push the data all the way downwards
			/*for(int i = 0; i < oldestBullet.transform.GetChildCount(); ++i)
			{
				GameObject Go = oldestBullet.transform.GetChild(i).gameObject;
				Go.rigidbody2D.velocity = Vector2.zero;
                Go.rigidbody2D.angularVelocity = 0f;
				Debug.Log(Go.name);
			}*/
			//oldestBullet.rigidbody2D.velocity = Vector2.zero;
			//oldestBullet.rigidbody2D.angularVelocity = 0f;
			// Determine spawn point of bullet based on angle and offset
			Vector2 cannonToMouse = (InputManager.GetCurrentPosition() - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y)).normalized;
			// fixme: it could spawn not within the range of that angle above
			//if(angle <= 90f && angle <= 0f)
			//{
	//		Vector2 newRelPos = cannonToMouse * spawnRadius;
	//		oldestBullet.transform.FindChild("torso").position = gameObject.transform.position + new Vector3(newRelPos.x, newRelPos.y, 0f);
			//oldestBullet.transform.rotation = Quaternion.Euler(cannonToMouse.x, cannonToMouse.y, 0f);//Quaternion.LookRotation(new Vector3(cannonToMouse.x, cannonToMouse.y, Camera.main.transform.position.z));
			//oldestBullet.transform.rotation = Quaternion.Euler(cannonToMouse.x, 0f, cannonToMouse.y);//Quaternion.LookRotation(new Vector3(cannonToMouse.x, cannonToMouse.y, Camera.main.transform.position.z));
			//oldestBullet.transform.eulerAngles = new Vector3(cannonToMouse.x, 0f, cannonToMouse.y);
			//Vector3 newRotation = Quaternion.LookRotation(Camera.main.transform.position - transform.position).eulerAngles;
			//newRotation.x = 0;
			//newRotation.y = 0;
			//oldestBullet.transform.rotation = Quaternion.Euler(newRotation);
	//		oldestBullet.transform.FindChild("torso").transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
	//		Debug.Log(cannonToMouse.ToString());
			//oldestBullet.transform.Rotate(new Vector3(cannonToMouse.x, cannonToMouse.y, 0f), Space.Self);
			//}
			// Fire the bullet - in the direction from Cannon to Mouse
			//oldestBullet.SetActive(true);
			SetMoving(oldestBullet.transform);
			float power = Mathf.Max(InputManager.GetCurrentDragOffset().magnitude, MIN_POWER);
			//oldestBullet.rigidbody2D.AddForce(cannonToMouse * power);
			//oldestBullet.GetComponentInChildren<Rigidbody2D>().rigidbody2D.AddForce(cannonToMouse * power);
			oldestBullet.transform.FindChild("torso").rigidbody2D.AddForce(cannonToMouse * power);// * 20);
			// Set is kinematic so the body parts can fling around!

			// Insert oldest bullet to the back as the freshest
			bullets.Enqueue(oldestBullet);
		}	
	}
	
	// Loop through all the nest child recursively
	void SetStill(Transform rootTransform)
	{
		GameObject go;
		for(int i = 0; i < rootTransform.childCount; ++i)
		{
			go = rootTransform.GetChild(i).gameObject;
			// Disable collision
			go.collider2D.enabled = false;
			// Reset forces
			go.rigidbody2D.velocity = Vector2.zero;
			go.rigidbody2D.angularVelocity = 0f;
			// Make sure it isn't affected by physics
			go.rigidbody2D.isKinematic = true;
			// Repeat for all child
			SetStill(go.transform);
		}
	}
	void SetMoving(Transform rootTransform)
	{
		GameObject go;
		for(int i = 0; i < rootTransform.childCount; ++i)
		{
			go = rootTransform.GetChild(i).gameObject;
			// Re-Enable collision
			go.collider2D.enabled = true;
			// Make sure it is affected by physics
			go.rigidbody2D.isKinematic = false;
			// Repeat for all child
			SetMoving(go.transform);
		}
	}

	void ResetFromPrefab(Transform rootTransform)
	{
		/*GameObject go;
		for(int i = 0; i < rootTransform.GetChildCount(); ++i)
		{
			go = rootTransform.GetChild(i).gameObject;
			// Re-Enable collision
			go.collider2D.enabled = true;
			// Make sure it is affected by physics
			go.rigidbody2D.isKinematic = false;
			// Repeat for all child
			for(int i = 0; i < rootTransform.GetChildCount(); ++i)
			{
			}
		}*/
		
		Transform[] allChildren = rootTransform.GetComponentsInChildren<Transform>();
		Transform[] transforms = BulletPrefab.GetComponentsInChildren<Transform>();
		if(transforms.Length != allChildren.Length)
		{
			Debug.Break();
		}
		else
		{
			Debug.Log(transforms.Length.ToString());
		}
		for(int i = 0; i < allChildren.Length; ++i)
		{
			// do whatever with child transform here
			allChildren[i].position = transforms[i].position;
			allChildren[i].rotation = transforms[i].rotation;
		}
	}
}
