using UnityEngine;
using System.Collections;

public class SpiderController : MonoBehaviour {

	public bool startTowardsLeft = true;

	public bool startIdle = true;

	public float walkSpeed = 50f;	// Pixels travelled per second
	public float distanceToTravel = 200f;	// In Pixels
	
	private Animator animator;
	// Set timing for the animations
	public float idleTime = 1f;
	private float idleCounter = 0f;

	private bool isWalking;

	private Vector3 startPoint, endPoint;

	private float totalDistanceTravelled;

	public bool ActivateOnHit = false;
	private bool isHit;

	// Use this for initialization
	void Start () {
		isHit = false;

		animator = GetComponent<Animator>();
		//animator.StartPlayback();

		animator.SetBool("IsIdle", startIdle);

		isWalking = !startIdle;
		if(startIdle)
		{
			animator.Play("SpiderIdle", -1, 1f);
		}
		else
		{
			animator.Play("SpiderWalk", -1, 1f);
		}

		startPoint = transform.position;
		endPoint = new Vector3(transform.position.x + (startTowardsLeft ? -distanceToTravel: distanceToTravel) / 72f, transform.position.y, transform.position.z);

		totalDistanceTravelled = 0f;

		if(startTowardsLeft)
		{
			transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
			Vector3 tmp = endPoint;
			endPoint = startPoint;
			startPoint = tmp;
			startTowardsLeft = !startTowardsLeft;
		}
		Debug.Log(startPoint.ToString() + " " + endPoint.ToString());

	}
	
	// Update is called once per frame
	void Update () {
		if(ActivateOnHit && isHit == false)
		{
			return;
		}

		// Idling
		if(isWalking == false)
		{
			//animator.SetFloat("IdleTime", idleCounter);
			//animator.SetBool("IsIdle", true);
			idleCounter += Time.deltaTime;
			if(idleCounter > idleTime)
			{
				startIdle = false;
				animator.SetBool("IsIdle", startIdle);
				// Reset distance counter
				totalDistanceTravelled = 0f;

				isWalking = true;
				idleCounter = 0f;
				Vector3 tmp = endPoint;
				endPoint = startPoint;
				startPoint = tmp;

				Debug.Log(startPoint.ToString() + " " + endPoint.ToString());

				// Idle done, flip over!
				transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
				// Flip current moving direction too
				startTowardsLeft = !startTowardsLeft;

			//	animator.Play("SpiderWalk", -1, 1f);
			}
		}
		else // Walking
		{
			totalDistanceTravelled += (startTowardsLeft ? -walkSpeed : walkSpeed ) * Time.deltaTime;
			//Debug.Log(totalDistanceTravelled.ToString());
			if(Mathf.Abs(totalDistanceTravelled) > distanceToTravel)
			{
				isWalking = false;
				startIdle = true;
				animator.SetBool("IsIdle", startIdle);
		//		animator.StartPlayback();
				// Make sure the ending is precise
				totalDistanceTravelled = totalDistanceTravelled < 0 ? -distanceToTravel: distanceToTravel;
				//Debug.Log(startPoint.ToString() + " " + endPoint.ToString());

			}
			// Convert pixels to units
			transform.position = startPoint + new Vector3(totalDistanceTravelled / 72f, 0f, 0f);//new Vector3(totalDistanceTravelled, transform.transform.position.y, 0);
		}

		//if(animator.GetCurrentAnimatorStateInfo(0).IsName("SpiderWalk"))
		{
		}
	}
		
	void OnCollisionEnter2D(Collision2D other)
	{
		// We do not want to execute if it hits the walls as well! Only bullet works!
		if(ActivateOnHit && isHit == false && other.gameObject.CompareTag("Bullet"))
		{
			isHit = true;
		}
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		// We do not want to execute if it hits the walls as well! Only bullet works!
		if(ActivateOnHit && isHit == false && other.CompareTag("Bullet"))
		{
			isHit = true;
		}
	}
	[ExecuteInEditMode]
	static void DrawLine()
	{
		//todo: visually draw 2d line to indicate path
	}
}
