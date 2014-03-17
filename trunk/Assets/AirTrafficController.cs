using UnityEngine;
using System.Collections;

public class AirTrafficController : MonoBehaviour {

	public RuntimeAnimatorController redController;
	public RuntimeAnimatorController greenController;
	public RuntimeAnimatorController blueController;
	public RuntimeAnimatorController yellowController;

	public GameObject redPlane;
	public GameObject greenPlane;
	public GameObject bluePlane;
	public GameObject yellowPlane;

	// Plane flying speed in pixels/sec
	public float minSpeed = 10f;
	public float maxSpeed = 300f;

	// Plane (Y-axis) spawn range from object
	public float maxRange;


	public float timeTillDispatch = 0f;

	private enum PLANECOLOR { RED, GREEN, BLUE, YELLOW };

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		timeTillDispatch -= Time.deltaTime;
		if(timeTillDispatch <= 0f)
		{
			// Reset timer if we are auccessful
			if(DispatchPlane())
			{
				timeTillDispatch = Random.Range(3f, 5f);
			}
		}
	}


	bool DispatchPlane()
	{
		bool success = false;
		System.Array A = System.Enum.GetValues(typeof(PLANECOLOR));
		PLANECOLOR V = (PLANECOLOR)A.GetValue(UnityEngine.Random.Range(0,A.Length));
//		PLANECOLOR selected = Random.Range(0, 3);
		switch(V)
		{
		case PLANECOLOR.RED:
			if(redPlane.activeInHierarchy == false)
			{
				redPlane.GetComponent<PlaneController>().SetUp();
				success = true;
			}
			break;
			
		case PLANECOLOR.GREEN:
			if(greenPlane.activeInHierarchy == false)
			{
				greenPlane.GetComponent<PlaneController>().SetUp();
				success = true;
			}
			break;
			
		case PLANECOLOR.BLUE:
			if(bluePlane.activeInHierarchy == false)
			{
				bluePlane.GetComponent<PlaneController>().SetUp();
				success = true;
			}
			break;
			
		case PLANECOLOR.YELLOW:
			if(yellowPlane.activeInHierarchy == false)
			{
				yellowPlane.GetComponent<PlaneController>().SetUp();
				success = true;
			}
			break;
		}

		return success;
	}
}
