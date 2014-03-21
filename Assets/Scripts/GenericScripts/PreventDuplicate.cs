using UnityEngine;
using System.Collections;

public class PreventDuplicate : MonoBehaviour {

	public bool doNotDestroyOnLoad = true;

	private static GameObject alreadyExists;

	// Use this for initialization
	void Start () {
		if(alreadyExists == null)
		{
			alreadyExists = this.gameObject;
		}
		else
		{
			Destroy(this.gameObject);
		}

		if(doNotDestroyOnLoad)
		{
			GameObject.DontDestroyOnLoad(this.gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
