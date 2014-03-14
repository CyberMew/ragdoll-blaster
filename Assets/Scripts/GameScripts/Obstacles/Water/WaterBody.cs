using UnityEngine;
using System.Collections;

public class WaterBody : MonoBehaviour {

	// Keep positions, velocities and accelerations of every node (water line)
	float[] xpositions;
	float[] ypositions;
	float[] velocities;
	float[] accelerations;
	LineRenderer waterLine;

	// Store each section of water
	GameObject[] meshobjects;
	Mesh[] meshes;
	// Colliders to interact with water
	GameObject[] colliders;

	// Constants
	// A low spring constant will make the springs loose. This means a force will cause large waves that oscillate slowly.
	// Conversely, a high spring constant will increase the tension in the spring.
	// Forces will create small waves that oscillate quickly. A high spring constant will make the water look more like jiggling Jello.
	public float springconstant = 0.02f;	// Controls the stiffness of the spring
	// It should be fairly small if you want your waves to oscillate.
	// A high dampening factor will make the water look thick like molasses, while a low value will allow the waves to oscillate for a long time.
	public float damping = 0.025f;
	// It controls how fast the waves spread. It can take values between 0 and 0.5, with larger values making the waves spread out faster.
	public float spread = 0.05f;	
	private float z = 0f;

	// Just dimensions of water
	float originalHeightYPos;
	//float left;
	float originalBottomYPos;

	// Particle system for splahes
	public GameObject splash;
	
	// Material for line renderer (able to switch to acid/lava/chemicals/etc)
	public Material WaterLineMaterial;
	// Material for water meshes
	public Material WaterBodyMaterial;

	public PhysicsMaterial2D LiquidMaterial;

	public Texture2D waterpng;

	// Mesh for main body of water
	//public GameObject watermesh;

	//public Vector2 topLeftCorner;	
	public float WaterBodyWidth = 200f; // in pixels
	public float WaterBodyHeight = 100f; // in pixels
	public float EdgeWidth = 5f; // in pixels
	public float WaterLineThickness = 5f; // in pixels

	public bool LetShitFloat = false;

	// Use this for initialization
	void Start () {
		// Convert all units from pixels to unity's unit
		WaterBodyWidth /= 72f;
		WaterBodyHeight /= 72f;
		EdgeWidth /= 72f;
		WaterLineThickness /= 72f;

		//SpawnWater(-10,20,-100,-10);
		//SpawnWater(-Screen.width * 0.5f,Screen.width,0,-Screen.height * 0.5f);

		//SpawnWater(gameObject.transform.position.x, WaterBodyWidth, gameObject.transform.position.y, gameObject.transform.position.y+WaterBodyHeight);
		//SpawnWater(gameObject.transform.position.x, WaterBodyWidth, gameObject.transform.position.y, gameObject.transform.position.y-WaterBodyHeight);
		z = transform.position.z;
		SpawnWater(gameObject.transform.position.x, WaterBodyWidth, gameObject.transform.position.y + WaterBodyHeight, gameObject.transform.position.y);
	}
	
	// Update is called once per frame
	//void Update () {
	
	//}
	
	void OnTriggerEnter2D(Collider2D Hit)
	{
		//Hit.rigidbody2D.velocity = Vector2.zero;

	}
	void OnTriggerStay2D(Collider2D Hit)
	{
		if(LetShitFloat == false)
		{
			return;
		}

		//Bonus exercise. Fill in your code here for making things float in your water.
		//You might want to even include a buoyancy constant unique to each object!
		Vector2 gravity = new Vector2( 0, -10 );
		
		//apply buoyancy force (fixtureA is the fluid)
		//float displacedMass = Hit.rigidbody2D.->GetDensity() * area;
		float displacedMass= 0.5f;
		Vector2 buoyancyForce = displacedMass * -gravity;
		//fixtureB->GetBody()->ApplyForce( buoyancyForce, centroid );
		//Hit.rigidbody2D.AddForceAtPosition(new 
	//	Hit.rigidbody2D.AddForce(new Vector2(0, 10));

		// Only bounce if the center of object inside the trigger box
		if(collider2D.OverlapPoint(Hit.transform.position))
		{
		float ypos = Hit.gameObject.transform.position.y;

		float force = springconstant * (ypos - WaterBodyHeight) + Hit.gameObject.rigidbody2D.velocity.y*damping ;
		float accelerations = -force;
		Hit.gameObject.rigidbody2D.velocity += new Vector2(0f, accelerations * 3f);
		//ypositions[i] += velocities[i];
		//waterLine.SetPosition(i, new Vector3(xpositions[i], ypositions[i], z));
		}
	}
	/*void OnTriggerExit2D(Collider2D Hit)
	{
		//Bonus exercise. Fill in your code here for making things float in your water.
		//You might want to even include a buoyancy constant unique to each object!
		Vector2 gravity = new Vector2( 0, -10 );
		
		//apply buoyancy force (fixtureA is the fluid)
		//float displacedMass = Hit.rigidbody2D.->GetDensity() * area;
		float displacedMass= 0.5f;
		Vector2 buoyancyForce = displacedMass * -gravity;
		//fixtureB->GetBody()->ApplyForce( buoyancyForce, centroid );
		//Hit.rigidbody2D.AddForceAtPosition(new 
		Hit.rigidbody2D.AddForce(new Vector2(0, 10));
	}*/

	// Building water from left to right (totalwidth in pixels in our case)
	public void SpawnWater(float leftXPos, float totalWidth, float topYPos, float bottomYPos)
	{		
		//WaterBodyWidth = totalWidth;

		// Pixels (or units) between each node
		//float EdgeWidth = 5f;
		// Find out how many nodes are needed
		int edgecount = Mathf.RoundToInt(totalWidth / EdgeWidth);	// Using 5 units/width, or 
		int nodecount = edgecount + 1;	// + 1 for the extra node at the end

		//Debug.Log("edges:" + edgecount.ToString() + " nodes:" + nodecount);

		waterLine = gameObject.AddComponent<LineRenderer>();
		waterLine.useWorldSpace = true;
		waterLine.material = WaterLineMaterial;
		waterLine.material.renderQueue = 2000;
		waterLine.SetVertexCount(nodecount);
		waterLine.SetWidth(WaterLineThickness, WaterLineThickness);
//		waterLine.SetColors(Color.white, Color.white);
		//WaterLine.transform.parent = transform;

		// Initialise all our class variables
		// Physics arrays
		xpositions = new float[nodecount];
		ypositions = new float[nodecount];
		velocities = new float[nodecount];
		accelerations = new float[nodecount];

		// Mesh arrays
		meshobjects = new GameObject[edgecount];
		meshes = new Mesh[edgecount];
		colliders = new GameObject[edgecount];

		originalHeightYPos = topYPos;
		//baseheight = topYPos;
		originalBottomYPos = bottomYPos;
		//left = leftXPos;

		// y-positions to be at the top of the water, and then incrementally add all the nodes side by side.
		// Our velocities and accelerations are zero initially, as the water is still.
		for (int i = 0; i < nodecount; ++i)
		{
			ypositions[i] = topYPos;
			xpositions[i] = leftXPos + EdgeWidth * i;
			accelerations[i] = 0;
			velocities[i] = 0;
			waterLine.SetPosition(i, new Vector3(xpositions[i], ypositions[i], z));
		}

		MeshRenderer mr;
		float leftUV = 0f, rightUV = 0f;
		float incrementUV = EdgeWidth / WaterBodyWidth;
		rightUV += incrementUV;
		////////////////////////
		/// WaterLine is done, now let's construct water body based on the water line
		/// 
		/// We need to create a quad mesh for each edge
		for (int i = 0; i < edgecount; i++)
		{
			// Create an empty mesh first
			meshes[i] = new Mesh();
			// Assign vertices
			Vector3[] Vertices = new Vector3[4];
			Vertices[0] = new Vector3(xpositions[i], ypositions[i], z);
			Vertices[1] = new Vector3(xpositions[i + 1], ypositions[i + 1], z);
			Vertices[2] = new Vector3(xpositions[i], bottomYPos, z);
			Vertices[3] = new Vector3(xpositions[i+1], bottomYPos, z);
			// Assign UVs
			Vector2[] UVs = new Vector2[4];
			UVs[0] = new Vector2(leftUV, 1);
			UVs[1] = new Vector2(rightUV, 1);
			UVs[2] = new Vector2(leftUV, 0);
			UVs[3] = new Vector2(rightUV, 0);
			rightUV += incrementUV;
			leftUV += incrementUV;
			// Assign triangles
			int[] tris = new int[6] { 0, 1, 3, 3, 2, 0 };

			meshes[i].vertices = Vertices;
			meshes[i].uv = UVs;
			meshes[i].triangles = tris;

			//meshobjects[i] = Instantiate(watermesh,Vector3.zero,Quaternion.identity) as GameObject;
			meshobjects[i] = new GameObject("WaterPartQuad" + i.ToString());
			meshobjects[i].transform.parent = transform;
			//meshobjects[i].transform.position = transform.position;
			//meshobjects[i].transform.rotation = transform.rotation;
			meshobjects[i].AddComponent<MeshFilter>().mesh = meshes[i];
			mr = meshobjects[i].AddComponent<MeshRenderer>();
			mr.material = WaterBodyMaterial;
			mr.sortingLayerName = "GUI";
			mr.sortingOrder = 1;
			//meshobjects[i].transform.parent = transform;
			meshobjects[i].layer = LayerMask.NameToLayer("Obstacle");


			// We want to detect if someone is touching the water line - each edge has it's own collider
			// todo: merge this with meshobject?
			colliders[i] = new GameObject();
			colliders[i].name = "WaterLineTrigger" + i.ToString();
			colliders[i].transform.parent = transform;
			// Set it just slightly below the water line
			colliders[i].transform.position = new Vector3(leftXPos + (EdgeWidth * i) + EdgeWidth * 0.5f, topYPos - (0.5f / 72f), 0);
			colliders[i].transform.localScale = new Vector3(EdgeWidth, EdgeWidth, 1);
			colliders[i].AddComponent<BoxCollider2D>().isTrigger = true;
			colliders[i].AddComponent<WaterDetector>();
			colliders[i].layer = LayerMask.NameToLayer("Obstacle");
		}

		
		if(LetShitFloat)
		{
			//Bonus exercise: Add a box collider to the water that will allow things to float in it.
			BoxCollider2D bc2 = gameObject.AddComponent<BoxCollider2D>();
			bc2.center = new Vector2(WaterBodyWidth * 0.5f, WaterBodyHeight * 0.5f);
			//Debug.Log(gameObject.GetComponent<BoxCollider2D>().center);
			bc2.size = new Vector2(WaterBodyWidth, WaterBodyHeight);
			bc2.isTrigger = true;
			bc2.sharedMaterial = LiquidMaterial;
		}
	}

	void UpdateMeshes()
	{
		for (int i = 0; i < meshes.Length; i++)
		{			
			Vector3[] Vertices = new Vector3[4];
			Vertices[0] = new Vector3(xpositions[i], ypositions[i], z);
			Vertices[1] = new Vector3(xpositions[i+1], ypositions[i+1], z);
			Vertices[2] = new Vector3(xpositions[i], originalBottomYPos, z);
			Vertices[3] = new Vector3(xpositions[i+1], originalBottomYPos, z);
			
			meshes[i].vertices = Vertices;
		}
	}

	// Physics
	void FixedUpdate()
	{
		float mass = 1f;
		for (int i = 0; i < xpositions.Length ; i++)
		{
			float force = springconstant * (ypositions[i] - originalHeightYPos) + velocities[i]*damping ;
			accelerations[i] = -force/mass;
			ypositions[i] += velocities[i];
			velocities[i] += accelerations[i];
			waterLine.SetPosition(i, new Vector3(xpositions[i], ypositions[i], z));
		}

		// Height difference between me and the left node
		float[] leftDeltas = new float[xpositions.Length];
		// Height difference between me and the right node
		float[] rightDeltas = new float[xpositions.Length];

		// Loop 8 times to make it feel more fluid
		for (int j = 0; j < 8; j++)
		{
			for (int i = 0; i < xpositions.Length; i++)
			{
				// We cannot check for the left neighbour if I am the most left node!
				if (i > 0)
				{
					// Get a percentage difference of me and my left neighbour's height, and assign that as velocity
					leftDeltas[i] = spread * (ypositions[i] - ypositions[i-1]);
					velocities[i - 1] += leftDeltas[i];
				}
				// We cannot check for the right neighbour if I am the last node!
				if (i < xpositions.Length - 1)
				{
					rightDeltas[i] = spread * (ypositions[i] - ypositions[i + 1]);
					velocities[i + 1] += rightDeltas[i];
				}
			}

			// We only want to affect their positions when we are done calculating
			for (int i = 1; i < xpositions.Length; ++i)
			{
				if (i > 0) 
				{
					ypositions[i-1] += leftDeltas[i];
				}
				if (i < xpositions.Length - 1) 
				{
					ypositions[i + 1] += rightDeltas[i];
				}
			}
		}
		
		//Finally we update the water body meshes to reflect this
		UpdateMeshes();
	}

	
	public void Splash(float xpos, float velocity)
	{
		//If the position is within the bounds of the water:
		if (xpos >= xpositions[0] && xpos <= xpositions[xpositions.Length-1])
		{
			//Offset the x position to be the distance from the left side
			//xpos -= xpositions[0];
			
			//Find which spring we're touching
			//int index = Mathf.RoundToInt((xpositions.Length-1)*(xpos / (xpositions[xpositions.Length-1] - xpositions[0])));
			int totalNodes = xpositions.Length;
			int index = Mathf.RoundToInt((xpos - xpositions[0]) / WaterBodyWidth * totalNodes);

			//Add the velocity of the falling object to the spring
			velocities[index] += velocity;
			
			//Set the lifetime of the particle system.
			float lifetime = 0.93f + Mathf.Abs(velocity)*0.07f;
			
			//Set the splash to be between two values in Shuriken by setting it twice.
			splash.GetComponent<ParticleSystem>().startSpeed = 8+2*Mathf.Pow(Mathf.Abs(velocity),0.5f);
			splash.GetComponent<ParticleSystem>().startSpeed = 9 + 2 * Mathf.Pow(Mathf.Abs(velocity), 0.5f);
			splash.GetComponent<ParticleSystem>().startLifetime = lifetime;
			
			//Set the correct position of the particle system.
			Vector3 position = new Vector3(xpositions[index],ypositions[index]-0.35f/72f,0.5f);
			
			//This line aims the splash towards the middle. Only use for small bodies of water:
			Quaternion rotation = Quaternion.LookRotation(new Vector3(xpositions[Mathf.FloorToInt(xpositions.Length / 2)], originalHeightYPos + 8, 5) - position);
			
			//Create the splash and tell it to destroy itself.
			GameObject splish = Instantiate(splash,position,rotation) as GameObject;
			Destroy(splish, lifetime+0.3f);
		}
	}
}
