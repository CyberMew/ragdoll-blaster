using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour {

	public GameObject Tap;
	public GameObject Hold;
	public GameObject SwipeRight;
	
	LTDescr tweenTapAlpha;
	LTDescr tweenTapMove;
	LTDescr tweenHoldAlpha;
	//LTDescr tweenHoldMove;
	LTDescr tweenSwipeAlpha;
	LTDescr tweenSwipeMoveX;
	//LTDescr tweenSwipeMoveY;

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

		// Reset data FIRST
		ResetTweenData();
		// Stop all tweens
		CancelTweens();
	}

	void OnDestroy()
	{
		GameManager.isTutorialOn = false;
	}

	void OnGUI()
	{
		GUI.Label(new Rect(Screen.width * 0.5f, Screen.height * 0.5f,100,100), "BACKGROUND HERE??");
	}

	void CancelTweens ()
	{
		LeanTween.cancel(Tap,tweenTapAlpha.id);
		LeanTween.cancel(Tap,tweenTapMove.id);
		if(tweenHoldAlpha != null)
		{
			LeanTween.cancel(Hold,tweenHoldAlpha.id);
		}
		//LeanTween.cancel(Hold,tweenHoldMove.id);
		if(tweenSwipeAlpha != null)
		{
			LeanTween.cancel(SwipeRight,tweenSwipeAlpha.id);
		}
		if(tweenSwipeMoveX != null)
		{
			LeanTween.cancel(SwipeRight,tweenSwipeMoveX.id);
		}
//		LeanTween.cancel(SwipeRight,tweenSwipeMoveY.id);
	}

	void ResetTweenData ()
	{
		// Reset their alpha values back to 1f
		Color col = Tap.GetComponent<SpriteRenderer>().color;
		col.a = 1f;
		Tap.GetComponent<SpriteRenderer>().color = col;
		Hold.GetComponent<SpriteRenderer>().color = col;
		SwipeRight.GetComponent<SpriteRenderer>().color = col;

		Debug.Log("ending:"+tweenTapMove.from.y);
		Tap.transform.localPosition = new Vector2(Tap.transform.position.x, tweenTapMove.from.x);
		
		//SwipeRight.transform.position =  new Vector2(tweenSwipeMoveX.from.x, tweenSwipeMoveY.from.x);
		// Check if the tween has even been created yet
		if(tweenSwipeMoveX != null)
		{
			SwipeRight.transform.localPosition = tweenSwipeMoveX.from;
		}
		//CancelTweens();

		StartAnimationSequence();
	}

	void StartAnimationSequence()
	{
		Color col = Tap.GetComponent<SpriteRenderer>().color;
		col.a = 0f;
		Tap.GetComponent<SpriteRenderer>().color = col;
		Hold.GetComponent<SpriteRenderer>().color = col;
		SwipeRight.GetComponent<SpriteRenderer>().color = col;

		StartTap ();
	}


	void StartTap()
	{
		tweenTapAlpha = LeanTween.alpha(Tap, 1f, 1f);
		tweenTapMove = LeanTween.moveY(Tap, Tap.transform.position.y - 1f, 0.5f).setDelay(0.1f).setEase(LeanTweenType.easeInSine).setOnComplete(StartHold);//.setRepeat(-1).setLoopClamp();;
		Debug.Log("starting:"+tweenTapMove.from.y);
	}

	void StartHold()
	{
		//tweenHoldAlpha = LeanTween.alpha(Hold, 1f, 1f).setDelay(0.5f).setLoopClamp().setRepeat(2).id;
		tweenHoldAlpha = LeanTween.alpha(Hold, 1f, 1f).setDelay(0.5f).setOnComplete(StartSwipeRight);
		//tweenHoldMove = LeanTween.scale(Hold, new Vector2(3f, 3f), 1f).setLoopClamp().setRepeat(2).id;//.setOnComplete(StartSwipeRight);
	}

	void StartSwipeRight()
	{
		tweenSwipeAlpha = LeanTween.alpha(SwipeRight, 1f, 1f).setOnComplete(FadeAway);
		//tweenSwipeMoveX = LeanTween.moveX(SwipeRight, SwipeRight.transform.position.x + 1f, 0.5f).setDelay(0.1f).setEase(LeanTweenType.easeInSine);//.setOnComplete(StartAnimationSequence);
		//tweenSwipeMoveY = LeanTween.moveY(SwipeRight, SwipeRight.transform.position.y + 1f, 0.5f).setDelay(0.1f).setEase(LeanTweenType.easeInSine);
		tweenSwipeMoveX = LeanTween.move(SwipeRight, new Vector2(SwipeRight.transform.position.x, SwipeRight.transform.position.y) + new Vector2(1f, 1f), 0.5f).setDelay(0.1f).setEase(LeanTweenType.easeInSine);//.setOnComplete(StartAnimationSequence);

	}

	void FadeAway()
	{
		LeanTween.alpha(Tap, 0f, 0.8f);
		LeanTween.alpha(Hold, 0f, 0.8f);
		LeanTween.alpha(SwipeRight, 0f, 0.8f).setOnComplete(ResetTweenData);
	}
}
