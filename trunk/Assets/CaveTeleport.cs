
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
			Debug.Log("Bullet Triggered Cave: " + other.name);
			Transform parent = GetParentRoot(other.transform);
			parent.position = caveOut.transform.position;
			//other.transform.root.position = caveOut.transform.position;
		}
	}

	Transform GetParentRoot(Transform transform)
	{
		// Exit if we reach the topmost root
		if(transform == transform.root)
		{
			return transform;
		}
		//if(transform.name.Contains("HumanCannonballPrefab"))
		if(transform.name.Contains("torso"))
		{
			Debug.Log("Found parent's child!");
			return transform;
		}
		return GetParentRoot(transform.parent);
	}
}
