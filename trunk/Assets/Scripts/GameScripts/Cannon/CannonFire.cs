﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;	// Queue

public class CannonFire : MonoBehaviour {

	public GameObject BulletPrefab;
	Queue<GameObject> bullets = new Queue<GameObject>();
	GameObject emptyParent;

	public const float MIN_POWER = 100f;
	public int maxPoolObjects = 5;
	public float spawnRadius = 1f;

	// Use this for initialization
	void Start () {
		// Create a master parent object for cannons - less mess in hierachy
		emptyParent = new GameObject("HumanCannons");

		GameObject temp;
		// Reuse the 5 bullets that we have right now
		for(int i = 0; i < maxPoolObjects; ++i)
		{
			temp = Instantiate(BulletPrefab, gameObject.transform.position, Quaternion.identity) as GameObject;
			temp.transform.parent = emptyParent.transform;
			temp.SetActive(false);
			bullets.Enqueue(temp);
		}

		// Strip it or everything except transforms
		StripPrefabExceptTransforms(BulletPrefab.transform);
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
			// Convert screen space to world space
			Vector2 mouseWorldPt = Camera.main.ScreenToWorldPoint(InputManager.GetCurrentPositionScreenSpace());
			// Set the cannon barrel as origin
			mouseWorldPt.x += Mathf.Abs(gameObject.transform.position.x);	// hardcoded because we know gameobject in negative region
			mouseWorldPt.y += Mathf.Abs(gameObject.transform.position.y);	// hardcoded because we know gameobject in negative region
			// Determine the angle of the line.
			angle = Mathf.Atan2(mouseWorldPt.y, mouseWorldPt.x) * Mathf.Rad2Deg;
			// Rotate about the z axis by angle degrees
			gameObject.transform.rotation = Quaternion.Euler(0f, 0f, angle);

			GameObject oldestBullet = bullets.Peek();
			// Re-get the world point again
			mouseWorldPt = Camera.main.ScreenToWorldPoint(InputManager.GetCurrentPositionScreenSpace());
			// Reset position of whole object only, not torso (the rest will follow!)
			Vector2 cannonToMouse = (mouseWorldPt - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y)).normalized;
			Vector2 newRelPos = cannonToMouse * spawnRadius;
			//oldestBullet = oldestBullet;//.transform.FindChild("torso").gameObject;
			oldestBullet.transform.position = gameObject.transform.position + new Vector3(newRelPos.x, newRelPos.y, 0f);
			oldestBullet.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);	// value is hardcoded here to fit the human sprite

		}

		// Release it according to drag power
		if(InputManager.GetIsInputReleased())
		{
			// Remove oldest bullet
			GameObject oldestBullet = bullets.Dequeue();
			
			// Convert screen space to world space
			Vector2 mouseWorldPt = Camera.main.ScreenToWorldPoint(InputManager.GetCurrentPositionScreenSpace());
			// Determine spawn point of bullet based on angle and offset
			Vector2 cannonToMouse = (mouseWorldPt - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y)).normalized;

			// Set is kinematic so the body parts can fling around!
			SetMoving(oldestBullet.transform);
			// Fire the bullet - in the direction from Cannon to Mouse
			float power = Mathf.Max(InputManager.GetCurrentDragOffset().magnitude, MIN_POWER);
			oldestBullet.transform.FindChild("torso").rigidbody2D.AddForce(cannonToMouse * power * 20);
			
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
		Transform[] allChildren = rootTransform.GetComponentsInChildren<Transform>();
		Transform[] transforms = BulletPrefab.GetComponentsInChildren<Transform>();
		if(transforms.Length != allChildren.Length)
		{
			Debug.Break();
		}
		for(int i = 0; i < allChildren.Length; ++i)
		{
			// Reset transforms
			allChildren[i].position = transforms[i].position;
			allChildren[i].rotation = transforms[i].rotation;
		}
	}

	void StripPrefabExceptTransforms(Transform rootTransform)
	{
		GameObject go;
		for(int i = 0; i < rootTransform.childCount; ++i)
		{
			go = rootTransform.GetChild(i).gameObject;
			// Strip and remove unnecessary components (might show up in game, affect physics etc)
			Destroy(go.GetComponent<HingeJoint2D>());
			Destroy(go.rigidbody2D);
			Destroy(go.collider2D);
			Destroy(go.GetComponent<SpriteRenderer>());

			// Repeat for all child
			StripPrefabExceptTransforms(go.transform);
		}
	}
}