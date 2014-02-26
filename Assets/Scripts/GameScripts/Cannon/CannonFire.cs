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
		}

		// Release it according to drag power
		if(InputManager.GetIsInputReleased())
		{
			// Remove oldest bullet
			GameObject oldestBullet = bullets.Dequeue();

			// Reset the bullet - remove force especially, and rotation of object?
			oldestBullet.rigidbody2D.velocity = Vector2.zero;
			oldestBullet.rigidbody2D.angularVelocity = 0f;
			// Determine spawn point of bullet based on angle and offset
			Vector2 cannonToMouse = (InputManager.GetCurrentPosition() - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y)).normalized;
			// fixme: it could spawn not within the range of that angle above
			//if(angle <= 90f && angle <= 0f)
			//{
			Vector2 newRelPos = cannonToMouse * spawnRadius;
			oldestBullet.transform.position = gameObject.transform.position + new Vector3(newRelPos.x, newRelPos.y, 0f);
			//oldestBullet.transform.rotation = Quaternion.Euler(cannonToMouse.x, cannonToMouse.y, 0f);//Quaternion.LookRotation(new Vector3(cannonToMouse.x, cannonToMouse.y, Camera.main.transform.position.z));
			//oldestBullet.transform.rotation = Quaternion.Euler(cannonToMouse.x, 0f, cannonToMouse.y);//Quaternion.LookRotation(new Vector3(cannonToMouse.x, cannonToMouse.y, Camera.main.transform.position.z));
			//oldestBullet.transform.eulerAngles = new Vector3(cannonToMouse.x, 0f, cannonToMouse.y);
			//Vector3 newRotation = Quaternion.LookRotation(Camera.main.transform.position - transform.position).eulerAngles;
			//newRotation.x = 0;
			//newRotation.y = 0;
			//oldestBullet.transform.rotation = Quaternion.Euler(newRotation);
			oldestBullet.transform.rotation = Quaternion.Euler(0f, 0f, angle);
			Debug.Log(cannonToMouse.ToString());
			//oldestBullet.transform.Rotate(new Vector3(cannonToMouse.x, cannonToMouse.y, 0f), Space.Self);
			//}
			// Fire the bullet - in the direction from Cannon to Mouse
			oldestBullet.SetActive(true);
			float power = Mathf.Max(InputManager.GetCurrentDragOffset().magnitude, MIN_POWER);
			oldestBullet.rigidbody2D.AddForce(cannonToMouse * power);

			// Insert oldest bullet to the back as the freshest
			bullets.Enqueue(oldestBullet);
		}
	
	}
}
