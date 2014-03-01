using UnityEngine;
using System.Collections;

public class Gesture_Swipe_Right : MonoBehaviour {

	public float UnitsToMove = 1f;

	// Use this for initialization
	void Start () {
		// Move towards the right by x pixels
		LTDescr tween = LeanTween.moveX(this.gameObject, this.gameObject.transform.position.x + UnitsToMove, 0.5f);
		tween.setEase(LeanTweenType.easeInCubic);
//		tween.setOnComplete
	}
	
	// Update is called once per frame
	void Update () {
	}
}
