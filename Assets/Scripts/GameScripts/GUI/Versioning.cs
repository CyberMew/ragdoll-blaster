using UnityEngine;
using System.Collections;
using UnityEditor;

public class Versioning : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(guiText.text != PlayerSettings.bundleVersion)
		{
			Debug.LogError("You forgot to update the version number!");
		}
	}

	void OnGUI()
	{
	}
}
