using UnityEngine;
using System.Collections;

public class CaveTeleport : MonoBehaviour {

	public GameObject caveOut;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.CompareTag("Bullet"))
		{
			other.transform.position = caveOut.transform.position;
		}
	}
}
