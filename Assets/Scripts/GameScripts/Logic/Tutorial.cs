using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour {

	public GameObject Tap;
	public GameObject Hold;
	public GameObject SwipeRight;

	// Use this for initialization
	void Start () {
		GameManager.isTutorialOn = true;

		//LeanTween.value(Hold, 0f, 0f);
		//LeanTween.value(SwipeRight, 0f, 0f);

		
		StartAnimationSequence ();
		
	}
	
	// Update is called once per frame
	void Update () {
		//if(InputManager.GetIsInputDown())
		{
			//Destroy(this.gameObject);
		}
	}

	void OnDisable()
	{
		Destroy(this.gameObject);
		// Stop all tweens
	}

	void OnDestroy()
	{
		GameManager.isTutorialOn = false;
	}

	void OnGUI()
	{
		GUI.Label(new Rect(Screen.width * 0.5f, Screen.height * 0.5f,100,100), "BACKGROUND HERE??");
	}

	void Tets()
	{
		Debug.Log("ok");
	}

	void StartAnimationSequence()
	{
		Color col = Tap.GetComponent<SpriteRenderer> ().color;
		col.a = 0f;
		Tap.GetComponent<SpriteRenderer> ().color = col;
		Hold.GetComponent<SpriteRenderer> ().color = col;
		SwipeRight.GetComponent<SpriteRenderer> ().color = col;
		//LeanTween.value(Tap, SetTapAlphaValue, 1f, 0f, 0.000001f).setOnComplete(Tets).setd;
		StartTap ();
	}

	void SetTapAlphaValue(float alpha)
	{
		Color col = Tap.GetComponent<SpriteRenderer>().color;
		col.a = alpha;
		Tap.GetComponent<SpriteRenderer>().color = col;
	}

	void StartTap()
	{
		// Appear
		LeanTween.value(Tap, SetTapAlphaValue, Tap.GetComponent<SpriteRenderer>().color.a, 1f, 1f);//.setRepeat(-1).setLoopClamp();;
		LeanTween.moveY(Tap, Tap.transform.position.y - 1f, 0.5f).setDelay(0.1f).setEase(LeanTweenType.easeInSine).setOnComplete(StartHold);//.setRepeat(-1).setLoopClamp();;
	}
	
	void SetHoldAlphaValue(float alpha)
	{
		Color col = Hold.GetComponent<SpriteRenderer>().color;
		col.a = alpha;
		Hold.GetComponent<SpriteRenderer>().color = col;
	}
	void StartHold()
	{
		LeanTween.value(Hold, SetHoldAlphaValue, Hold.GetComponent<SpriteRenderer>().color.a, 1f, 1f).setDelay(0.3f);
		LeanTween.scale(Hold, new Vector2(3f, 3f), 1f).setOnComplete(StartSwipeRight);
	}
	
	void SetSwipeRightAlphaValue(float alpha)
	{
		Color col = SwipeRight.GetComponent<SpriteRenderer>().color;
		col.a = alpha;
		SwipeRight.GetComponent<SpriteRenderer>().color = col;
	}
	void StartSwipeRight()
	{
		LeanTween.value(SwipeRight, SetSwipeRightAlphaValue, SwipeRight.GetComponent<SpriteRenderer>().color.a, 1f, 1f);
		LeanTween.moveX(SwipeRight, SwipeRight.transform.position.x + 1f, 0.5f).setDelay(0.1f).setEase(LeanTweenType.easeInSine);//.setOnComplete(StartAnimationSequence);
		LeanTween.moveY(SwipeRight, SwipeRight.transform.position.y + 1f, 0.5f).setDelay(0.1f).setEase(LeanTweenType.easeInSine);//.setOnComplete(StartAnimationSequence);
	}
}
