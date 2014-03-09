using UnityEngine;
using System.Collections;

public class SmokeController : MonoBehaviour {

	private Animator animator;

	private Animation ani;

	private Sprite originalSprite;
	private SpriteRenderer sr;

	//static int atakState = Animator.StringToHash("Base Layer.SmokeOut"); 
	
	private bool isClicked;
	private bool isAnInterrupt;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		sr = GetComponent<SpriteRenderer>();
		ani = GetComponent<Animation>();
		// Store original sprite
		originalSprite = sr.sprite;

		isAnInterrupt = false;
		isClicked = false;
	}
	
	// Update is called once per frame
	void Update () {
		/*if(animation.isPlaying == false)
		{
			gameObject.SetActive(false);
		}*/
		/*
		if(isClicked && animator.GetCurrentAnimatorStateInfo(0).IsName("NotPlaying") && animator.GetBool("PlaySmoke") == false)
		{
			//animator.StopPlayback();
			//gameObject.SetActive(false);
			
			isClicked = false;
			animator.SetBool("PlaySmoke", false);
		}
		 
		if(isAnInterrupt && animator.GetCurrentAnimatorStateInfo(0).IsName("NotPlaying"))
		{
			isAnInterrupt = false;
			
			animator.SetBool("Interrupt", false);
		}*/
	}

	void OnEnable()
	{
		//if(animator)
		{
			//animator.StartPlayback();
		//	animator.Play("SmokeOut");
		}
		//animation.Play();
	}

	void OnDisable()
	{
		if(animator)
		{
			//Animation an = animator.getstate
			//if(an)
			{
			//	an.Rewind();
			}

		}
		//animation.Stop();
		//animation.Rewind();
	}

	public void ResetAndPlay()
	{
		if(animator)
		{
			// If we are currently playing, interrupt
			/*if(animator.GetCurrentAnimatorStateInfo(0).IsName("SmokeOut"))
			{
				//animator.StopPlayback();
				//gameObject.SetActive(false);
				sr.sprite = originalSprite;
				//animator.SetBool("Interrupt", true);
				//animator.SetBool("Interrupt", false);
				isAnInterrupt = true;
				//animator
			}
			else // 
			{
			//animator.StartPlayback();
			//animator.sto
			//animator.Play("NotPlaying");
			isClicked = true;
			animator.SetBool("PlaySmoke", true);
			Debug.Log("clicked");
			//animator.Play("SmokeOut");

			}*/
			//animator.ForceStateNormalizedTime(0.0);
			//animator.StartPlayback();
			animator.Play("SmokeOut", -1, 1f);
			//animator.Play("SmokeOut");
			//animator.playbackTime = 0f;
		}
		if(animation)
		{
			//animation.Play ("SmokeOut", AnimationPlayMode.Stop);
		//	animation.Play();
		}
		if(ani)
		{
			//ani.Play ("SmokeOut", AnimationPlayMode.Stop);
		//	ani.Play();
		}
	}
}
