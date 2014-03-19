using UnityEngine;
using System.Collections;
using System.Collections.Generic;	// Queue

public class CannonFire : MonoBehaviour {

	public GameObject BulletPrefab;
	Queue<GameObject> bullets = new Queue<GameObject>();
	GameObject emptyParent;
//	private float prefabGravityScale;
	
	public float MIN_POWER = 2000f;
	public float MAX_POWER = 6000f;
	public int maxPoolObjects = 5;
	public float spawnRadius = 1f;
	public float maxDistanceScaler = 800f;	// total amount of magnitude needed to hit MAX_POWER
	private float currDistance;
	
	public float maxCannonAngleinDegrees = 90f;
	public float minCannonAngleinDegrees = -20f;

	private Vector2 newRelVec;
	private float currPowerForce;

	// Variables for power gauge bar	
	public Texture2D frame;
	public Texture2D white;
	public Texture2D arrow;
	public Texture2D gradient;
	//public float percent = 1.0f;	// Amount of bar to fill
	public float posX = 50f;	// top left
	public float posY = 100f;	// top left

	// SmokeAnimator effect
	public GameObject smokeObj;
	private SmokeController smokeScript;

	// Use this for initialization
	void Start () {
		// Disable the prefab
		BulletPrefab.SetActive(false);

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

//		prefabGravityScale = BulletPrefab.rigidbody2D.gravityScale;
		// Strip it or everything except transforms
		StripPrefabExceptTransforms(BulletPrefab.transform);

		currPowerForce = MIN_POWER;
		currDistance = 0f;

		smokeScript = smokeObj.GetComponent<SmokeController>();
	}

	bool firstCheckOutsideDone = false;
	bool exceedAngle = false;
	bool isFirstTriggered = false;	// This var keep track of whether we have followed the sequence (if not I could dismiss the pause menu and IMMEDIATELY triggered the Pressed
	float angle = 0f;
	// Update is called once per frame
	void Update () {
		if(GameManager.IsGamePaused() || GameManager.isUIBusy == true)
		{
			return;
		}

		// Get the bullet from queue, and make it appear inside the cannon
		if(InputManager.GetIsInputDown())
		{
			ResetEverything();
		}

		// Rotate the sprite to face the mouse when input depressed
		if(isFirstTriggered && InputManager.GetIsInputPressed())
		{
			// Convert screen space to world space
			Vector2 mouseWorldPt = Camera.main.ScreenToWorldPoint(InputManager.GetCurrentPositionScreenSpace());
			// Set the cannon barrel as origin
			mouseWorldPt.x -= gameObject.transform.position.x;	// hardcoded because we know gameobject in negative region
			mouseWorldPt.y -= gameObject.transform.position.y;	// hardcoded because we know gameobject in negative region
			// Determine the angle of the line.
			float rawAngle = Mathf.Atan2(mouseWorldPt.y, mouseWorldPt.x) * Mathf.Rad2Deg;
			if(rawAngle < maxCannonAngleinDegrees && rawAngle > minCannonAngleinDegrees)
			{
				// Re-get the world point again
				mouseWorldPt = Camera.main.ScreenToWorldPoint(InputManager.GetCurrentPositionScreenSpace());
				// Reset position of whole object only, not torso (the rest will follow!)
				Vector2 cannonToMouse = (mouseWorldPt - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y)).normalized;
				newRelVec = cannonToMouse;

				// Store last used angle (for other functions)
				angle = CalculatePositionRotation(rawAngle);

				firstCheckOutsideDone = true;
			}
			else // if we exceed angle
			{
				// Check if we click outside for the first time, if so, we calculate the extreme values needed
				if(firstCheckOutsideDone == false)
				{
					firstCheckOutsideDone = true;

					// we only actually have to calculate rotation + position ONCE if we clicked outside the angle
					float tmpAngle = maxCannonAngleinDegrees; //todo: replace this with actual current cannon angle

					Vector2 cannonToMouse = Vector2.zero;
					/*if(tmpAngle > 90f && tmpAngle <= 180f)
					{
						cannonToMouse = new Vector2( Mathf.Cos(tmpAngle),-Mathf.Sin(tmpAngle));
					}
					else if(tmpAngle > -90f && tmpAngle <= 0f)
					{
						cannonToMouse = new Vector2(-Mathf.Cos(tmpAngle), Mathf.Sin(tmpAngle));
					}
					else if(tmpAngle > -180f && tmpAngle <= -90f)
					{
						cannonToMouse = new Vector2(-Mathf.Cos(tmpAngle),-Mathf.Sin(tmpAngle));
					}
					else*/
					{						
						cannonToMouse = new Vector2(Mathf.Cos(tmpAngle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
					}

					// We still need to update the global vars for others to use! (i.e. force etc)
					newRelVec = cannonToMouse.normalized;
					angle = tmpAngle;

					Debug.Log(cannonToMouse);
					GameObject oldestBullet = bullets.Peek();

					oldestBullet.transform.position = gameObject.transform.position + new Vector3(newRelVec.x * spawnRadius, newRelVec.y * spawnRadius, 0f);
					oldestBullet.transform.rotation = Quaternion.Euler(0f, 0f, tmpAngle - 90f);	// value is hardcoded here to fit the human sprite

					// Rotate about the z axis by angle degrees
					gameObject.transform.rotation = Quaternion.Euler(0f, 0f, tmpAngle);

					smokeObj.transform.position = gameObject.transform.position + new Vector3(newRelVec.x * 1.5f, newRelVec.y * 1.5f, 0f);
					smokeObj.transform.rotation = Quaternion.Euler(0f, 0f, tmpAngle);

					Debug.Log("this should only run once when we first click outside");
				}
			}

			// No matter what we need to calculate force
			currPowerForce = CalculatePowerForce();
		}

		// Release it according to drag power
		if(isFirstTriggered && InputManager.GetIsInputReleased())
		{
			// Remove oldest bullet
			GameObject oldestBullet = bullets.Dequeue();

			// Set is kinematic so the body parts can fling around!
			SetMoving(oldestBullet.transform);
			float power = currPowerForce;//CalculatePowerForce();
			// Fire the bullet - in the direction from Cannon
			oldestBullet.transform.FindChild("torso").rigidbody2D.AddForce(newRelVec * power);
			//Debug.Log((newRelVec * power * 20).magnitude.ToString());
			
			// Insert oldest bullet to the back as the freshest
			bullets.Enqueue(oldestBullet);

			// Reset the whole sequence
			isFirstTriggered = false;
			// Reset values
			currPowerForce = MIN_POWER;
			currDistance = 0f;

			// Store fire counts
			++GameManager.tempShots;

			// Show smoke animation
			if(smokeScript)
			{
				smokeScript.ResetAndPlay();
			}

			// Reset the bool check when we release the mouse
			firstCheckOutsideDone = false;
		}	
	}

	void ResetEverything()
	{
		GameObject oldestBullet = bullets.Peek();
		// Enable the object so it is visible
		oldestBullet.SetActive(true);
		// Make the human inanimate
		SetStill(oldestBullet.transform);
		// Copy transform data from Prefab (reset it's body parts correctly)
		ResetFromPrefab(oldestBullet.transform);

		isFirstTriggered = true;
		
		
		// Reset values
		currPowerForce = MIN_POWER;
		currDistance = 0f;
	}

	float CalculatePositionRotation(float rawAngle)
	{
		// Restraint the MAX angle of the cannon angle
		float tmpAngle = Mathf.Min(maxCannonAngleinDegrees, rawAngle);
		// Restraint the MIN angle of the cannon angle
		tmpAngle = Mathf.Max(minCannonAngleinDegrees, tmpAngle);
			
		// Rotate about the z axis by angle degrees
		gameObject.transform.rotation = Quaternion.Euler(0f, 0f, tmpAngle);
		
		GameObject oldestBullet = bullets.Peek();
		oldestBullet.transform.position = gameObject.transform.position + new Vector3(newRelVec.x * spawnRadius, newRelVec.y * spawnRadius, 0f);
		oldestBullet.transform.rotation = Quaternion.Euler(0f, 0f, tmpAngle - 90f);	// value is hardcoded here to fit the human sprite
		
		smokeObj.transform.position = gameObject.transform.position + new Vector3(newRelVec.x * 1.5f, newRelVec.y * 1.5f, 0f);
		smokeObj.transform.rotation = Quaternion.Euler(0f, 0f, tmpAngle);

		return tmpAngle;
	}

	float CalculatePowerForce()
	{
		// Get a value between 0 to maxDistance (= 800)
		currDistance = Mathf.Min(InputManager.GetCurrentDragOffset().magnitude, maxDistanceScaler);
		// Calculate the amount of power force based on the distance travelled
		float rawPower = MIN_POWER + (MAX_POWER - MIN_POWER) / maxDistanceScaler * currDistance;
		//float power = Mathf.Max(rawPower, MIN_POWER);
		//power = Mathf.Min(MAX_POWER, power);
//		Debug.Log(InputManager.GetCurrentDragOffset().magnitude.ToString());

		return rawPower;
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
			// Reset back gravity

			// Repeat for all child
			SetMoving(go.transform);
		}
	}

	void ResetFromPrefab(Transform rootTransform)
	{
		BulletPrefab.SetActive(true);
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
			// Reset Tags (by Lava)
			allChildren[i].tag = "Bullet";
			// Reset gravity scale
//			allChildren[i].rigidbody2D.gravityScale = prefabGravityScale;
			if(allChildren[i].rigidbody2D && transforms[i].rigidbody2D)
			{
				//allChildren[i].rigidbody2D.mass = transforms[i].rigidbody2D.mass;
				allChildren[i].rigidbody2D.gravityScale = transforms[i].rigidbody2D.gravityScale;
				//Debug.Log(transforms[i].rigidbody2D.gravityScale.ToString());
			}
		}
		BulletPrefab.SetActive(false);
	}

	void StripPrefabExceptTransforms(Transform rootTransform)
	{
		GameObject go;
		for(int i = 0; i < rootTransform.childCount; ++i)
		{
			go = rootTransform.GetChild(i).gameObject;
			// Strip and remove unnecessary components (might show up in game, affect physics etc)
			Destroy(go.GetComponent<HingeJoint2D>());
			//Destroy(go.rigidbody2D); // We still need the gravity scale for reset
			Destroy(go.collider2D);
			Destroy(go.GetComponent<SpriteRenderer>());

			// Repeat for all child
			StripPrefabExceptTransforms(go.transform);
		}
	}

	// Draw power bar gauge
	void OnGUI()
	{
		if(Event.current.type.Equals(EventType.Repaint))
		{
			// We will be offsetting based on the frame since it is bigger (getting frame border thickness width/height here)
			float offsetX = (frame.width - gradient.width) * 0.5f;
			float offsetY = (frame.width - gradient.width) * 0.5f;

			// Draw white background
			Graphics.DrawTexture(new Rect(posX + offsetX, posY + offsetY, white.width, white.height), white);
			// Draw gradient
			//float percent = (currPowerForce - MIN_POWER) / (MAX_POWER - MIN_POWER);
			float percent = currDistance / maxDistanceScaler;
			//Debug.Log(percent.ToString());
			//Debug.Log(percent.ToString() +" power:"+currPowerForce.ToString());
			float amountOfYtoShow = gradient.height * percent;
			float amountOfYEmptySpace = gradient.height - amountOfYtoShow;
			Graphics.DrawTexture(new Rect(posX + offsetX, posY + offsetY + amountOfYEmptySpace, gradient.width, amountOfYtoShow), gradient, new Rect(0,0,1f,percent), 0,0,0,0);
			// Draw frame
			Graphics.DrawTexture(new Rect(posX, posY, frame.width, frame.height), frame);
			// Draw arrow to follow gradient (but staying away from the border by x pixels)
			float spacingFromBorder = 30f;
			Graphics.DrawTexture(new Rect(posX - offsetX - spacingFromBorder, posY + offsetY + amountOfYEmptySpace - (arrow.height * 0.5f), arrow.width, arrow.height), arrow);
		}
	}
}
