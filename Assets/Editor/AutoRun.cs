using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class Autorun
{
	static Autorun()
	{
		EditorApplication.update += RunOnce;
	}
	
	static void RunOnce()
	{
		Debug.Log("RunOnce!");
		EditorApplication.update -= RunOnce;
	}
}