using UnityEngine;
using System.Collections;

public class BulletManager : MonoBehaviour {



	GameObject m_Bullet;

	// Use this for initialization
	void Start () {
		m_Bullet = GameObject.Find("Bullet");
	
	}
	
	// Update is called once per frame
	void Update () {

		int x = Random.Range(-1000,1000);
		int y = Random.Range(0,1000);
//		Debug.Log(new Vector2(x,y));
		if(InputManager.GetIsInputReleased())
		{
			m_Bullet.rigidbody2D.AddForce(InputManager.GetCurrentDragOffset());
			// Reset the direction after we are done
			//InputManager.SetOffset(direction);

		}

	
	}
}
