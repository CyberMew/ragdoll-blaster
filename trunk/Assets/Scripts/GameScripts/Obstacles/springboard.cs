using UnityEngine;
using System.Collections;

public class springboard : MonoBehaviour {

	private bool isUp;
	private const float upTimeConst = 0.1f;
	private float upTime = 0.0f;
	private SpriteRenderer sbRenderer;
	public Sprite upSprite;
	public Sprite downSprite;


	// Use this for initialization
	void Start () {
		isUp = false;
		upTime = 0.0f;
		sbRenderer = gameObject.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if (isUp) {
			sbRenderer.sprite = upSprite;
			if(upTime >= upTimeConst){
				upTime = 0.0f;
				sbRenderer.sprite = downSprite;
				isUp = false;
			}
			else{
				upTime += Time.deltaTime;
			}
		}
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if(other.gameObject.CompareTag("Bullet") && isUp == false){
			isUp = true;
			other.gameObject.rigidbody2D.AddForce(transform.up.normalized * 9000);

		}
	}
}
