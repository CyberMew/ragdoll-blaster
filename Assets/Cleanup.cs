using UnityEngine;
using System.Collections;

public class Cleanup : MonoBehaviour {
	
	private Animator animator;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(animator && animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
		{
			Destroy(this.gameObject);
		}
	}
}
