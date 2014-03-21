using UnityEngine;
using System.Collections;

public class PlaneController : MonoBehaviour {

	public GameObject burningParticle;
	public GameObject fireworksParticle;

	public Transform flagObj;

	private bool isHit;

	private float planeSpeed;
	
	// Plane flying speed in pixels/sec
	public float minSpeed = 3f;
	public float maxSpeed = 10f;
	
	// Plane (Y-axis) relative spawn position
	public float maxRange = 50f;

	//private Transform originalPosition;
	private Vector3 originalPosition;
	private Quaternion originalRotation;

	private ParticleEmitter smokeEmitterInstance;

	float horzExtent;

	// Use this for initialization
	void Start () {
		isHit = false;
		planeSpeed = 0f;

		horzExtent = Camera.main.orthographicSize * Screen.width / Screen.height;
		transform.position = new Vector3(-horzExtent - renderer.bounds.extents.x, transform.position.y, transform.position.z); //todo check if this just nice outside the left boundary of camera

		originalPosition = transform.position;
		originalRotation = transform.rotation;

		gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		// If we are offscreen, we restart from beginning
		if(renderer.bounds.min.x > horzExtent + 5f)
		{
			// RebuildPlane
			gameObject.SetActive(false);
			isHit = false;

			if(smokeEmitterInstance)
			{
				smokeEmitterInstance.emit = false; // so that it would autodestruct

				// Since we have no use for the emitter (i.e. it doesn't need to follow the plane anymore, just reset it's parent
				smokeEmitterInstance.transform.parent = gameObject.transform.parent;
			}
		}
	}

	// If we left the screen, we reset ourselves too
	/*void OnBecameInvisible() {
		SetUp(); // doesn't seems to be working
	}*/

	void OnCollisionEnter2D(Collision2D other)
	{
		// Crash if human hit me
		if(other.gameObject.CompareTag("Bullet") && isHit == false)
		{
			isHit = true;
			GameObject go = Instantiate(fireworksParticle, gameObject.transform.position, Quaternion.identity) as GameObject;
			go.GetComponent<ParticleAnimator>().autodestruct = true;
			go = Instantiate(burningParticle, transform.position, Quaternion.identity) as GameObject;
			go.transform.parent = gameObject.transform;
			go.GetComponent<ParticleAnimator>().autodestruct = true;
			smokeEmitterInstance = go.particleEmitter;
		}

	}

	float privateTimer = 0f;

	void FixedUpdate()
	{
		privateTimer += Time.deltaTime;
	
		float pixelsToUnits = GameManager.height * 0.5f / Camera.main.orthographicSize;
		float yPos = Mathf.Sin(privateTimer) / pixelsToUnits * 100f;	// todo: some hard code scaling
		Debug.Log(yPos);
		float yDir = originalPosition.y + yPos;
		if(isHit)
		{
			yDir = transform.transform.position.y + Physics2D.gravity.y / pixelsToUnits * 0.5f;
			float speed = 3f;
			Quaternion to = Quaternion.AngleAxis(Random.Range(-45f,-30f), Vector3.forward);
			transform.rotation = Quaternion.Slerp(originalRotation, to, Time.fixedDeltaTime * speed);
		}
		transform.position = new Vector3(transform.position.x + planeSpeed, yDir, transform.position.z);
	}

	public void SetUp()
	{
		gameObject.SetActive(true);
		if(smokeEmitterInstance)
		{
			smokeEmitterInstance.ClearParticles();
			smokeEmitterInstance = null;
		}

		// Reset to original trasform
		transform.rotation = originalRotation;
		transform.position = originalPosition;

		float pixelsToUnits = GameManager.height * 0.5f / Camera.main.orthographicSize;
		// Randomise the y starting point again
		float spawnYPos = originalPosition.y + Random.Range(-maxRange, maxRange) / pixelsToUnits * 50f; //todo remove hardcode
		transform.position = new Vector3(transform.position.x, spawnYPos, transform.position.z);
	
		collider2D.enabled = true;
		rigidbody2D.isKinematic = true;
		
		// Randomise the speed again
		planeSpeed = Random.Range(minSpeed, maxSpeed) / pixelsToUnits;
		flagObj.localPosition = new Vector3(-50f / pixelsToUnits,10f/pixelsToUnits,0f);

		rigidbody2D.gravityScale = 10f;

	}
}
