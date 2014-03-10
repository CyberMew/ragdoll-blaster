using UnityEngine;
using System.Collections;
using UnityEditor;

public class FileMover : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	
	[MenuItem ("MyMenu/Do Something")]
	static void ahah () {
		Debug.Log ("Doing Something...");
	}

	// Add a menu item named "Do Something with a Shortcut Key" to MyMenu in the menu bar
	// and give it a shortcut (ctrl-g on Windows, cmd-g on OS X).
	[MenuItem ("MyMenu/Do Something with a Shortcut Key %g")]
	static void DoSomethingWithAShortcutKey () {
		Debug.Log ("Doing something with a Shortcut Key...");
	}
	// Add a menu item called "Double Mass" to a Rigidbody's context menu.
	[MenuItem ("CONTEXT/Rigidbody/Double Mass")]
	static void DoubleMass (MenuCommand command) {
		Rigidbody body = (Rigidbody)command.context;
		body.mass = body.mass * 2;
		Debug.Log ("Doubled Rigidbody's Mass to " + body.mass + " from Context Menu.");
	}
	// Add a menu item called "Double Mass" to a Rigidbody's context menu.
	[MenuItem ("CONTEXT/GameObject/MoveFile")]
	static void MoveFile (MenuCommand command) {
		Rigidbody body = (Rigidbody)command.context;
		body.mass = body.mass * 2;
		Debug.Log ("Doubled Rigidbody's Mass to " + body.mass + " from Context Menu.");
	}

	[MenuItem ("Assets/Create/New My Object")]
	static void HUATAH (MenuCommand command) {
		Rigidbody body = (Rigidbody)command.context;
		body.mass = body.mass * 2;
		Debug.Log ("Doubled Rigidbody's Mass to " + body.mass + " from Context Menu.");
	}
	// Validated menu item.
	// Add a menu item named "Log Selected Transform Name" to MyMenu in the menu bar.
	// We use a second function to validate the menu item
	// so it will only be enabled if we have a transform selected.
	//[MenuItem ("MyMenu/Log Selected Transform Name")]
	static void LogSelectedTransformName ()
	{
		Debug.Log ("Selected Transform is on " + Selection.activeTransform.gameObject.name + ".");
	}

	// Validate the menu item defined by the function above.
	// The menu item will be disabled if this function returns false.
	[MenuItem ("MyMenu/Log Selected Transform Name", true)]
	static bool ValidateLogSelectedTransformName () {
		// Return false if no transform is selected.
		return Selection.activeTransform != null;
	}

	// https://docs.unity3d.com/Documentation/ScriptReference/MenuItem-ctor.html
	// Source: http://altaf-navalur.blogspot.sg/2013/05/cut-paste-option-in-assets-context-menu.html
	static string _AssetPath = string.Empty;
	
	[MenuItem("Assets/Cut", false, 80)]
	static void MoveAsset ()
	{
		_AssetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
		Debug.Log("Copied asset at path : " + _AssetPath);
	}
	
	[MenuItem("Assets/Cut", true)]
	static bool MoveAssetValidate ()
	{
		return (Selection.activeObject != null);
	}
	
	[MenuItem("Assets/Paste", false, 80)]
	static void PasteAsset ()
	{
		string dstPath = AssetDatabase.GetAssetPath(Selection.activeObject);
		string fileExt = System.IO.Path.GetExtension(dstPath);
		if(!string.IsNullOrEmpty(fileExt))
			dstPath = System.IO.Path.GetDirectoryName(dstPath);
		string fileName = System.IO.Path.GetFileName(_AssetPath);
		string msg = AssetDatabase.MoveAsset(_AssetPath, dstPath + "/" + fileName);
		if(string.IsNullOrEmpty(msg))
		{
			_AssetPath = null;
			Debug.Log("Pasted asset at path : " + _AssetPath);
		}
		else
			Debug.LogError("Failed to paste asset : " + msg);
	}
	
	[MenuItem("Assets/Paste", true)]
	static bool PasteAssetValidate ()
	{
		//Have we copied anything?
		if(string.IsNullOrEmpty(_AssetPath))
			return false;
		//Try to paste no where?
		if(Selection.activeObject == null)
			return false;
		//Trying to paste on same asst again?
		if(AssetDatabase.GetAssetPath(Selection.activeObject) == _AssetPath)
			return false;
		
		return true;
	}

	////////////////////
	// Custom moving of prefabs and scripts
		
	[MenuItem("Assets/Custom Move/To Corresponding Prefabs Folder", false, 100)]
	static void CustomMoveAssetToPrefab ()
	{
		// Assumes verified beforehand
		string dstPath = AssetDatabase.GetAssetPath(Selection.activeObject);
		string newPrefabPath = dstPath.Replace("Assets/Sprites/", "Assets/Prefabs/");
		string newPrefabDirectory = System.IO.Path.GetDirectoryName(newPrefabPath);
		/*
		string fileExt = System.IO.Path.GetExtension(dstPath);
		if(!string.IsNullOrEmpty(fileExt))
			dstPath = System.IO.Path.GetDirectoryName(dstPath);
		string fileName = System.IO.Path.GetFileName(_AssetPath);*/
		
		// Create directory if it doesn't exist yet
		if(!System.IO.Directory.Exists(newPrefabDirectory))
		{
			System.IO.Directory.CreateDirectory(newPrefabDirectory);
			AssetDatabase.Refresh();
		}

		string msg = AssetDatabase.MoveAsset(dstPath, newPrefabPath);
		if(string.IsNullOrEmpty(msg))
		{
			_AssetPath = null;
			Debug.Log("Moved asset from path: " + dstPath + " to " + newPrefabPath);
		}
		else
			Debug.LogError("Failed to move asset : " + msg);
	}

	[MenuItem("Assets/Custom Move/To Corresponding Prefabs Folder", true)]
	static bool CustomMoveAssetToPrefabValidate ()
	{
		// Make sure it is selected first
		if(Selection.activeObject == null)
			return false;

		string path = AssetDatabase.GetAssetPath(Selection.activeObject);
		//Debug.Log(path);
		if(path.Contains("Assets/Sprites/") == false)
			return false;

		// Only custom move for .prefab files
		string fileExt = System.IO.Path.GetExtension(path);
		//Debug.Log(fileExt);
		if(fileExt.Equals(".prefab") == false)
		   return false;

		return true;
	}
	
	
	[MenuItem("Assets/Custom Move/To Corresponding Scripts Folder", false, 101)]
	static void CustomMoveAssetToScript ()
	{
		// Assumes verified beforehand
		string dstPath = AssetDatabase.GetAssetPath(Selection.activeObject);
		string newPrefabPath = dstPath.Replace("Assets/Sprites/Game/Levels", "Assets/Scripts/GameScripts");
		string newPrefabDirectory = System.IO.Path.GetDirectoryName(newPrefabPath);
		/*
		string fileExt = System.IO.Path.GetExtension(dstPath);
		if(!string.IsNullOrEmpty(fileExt))
			dstPath = System.IO.Path.GetDirectoryName(dstPath);
		string fileName = System.IO.Path.GetFileName(_AssetPath);*/
		
		// Create directory if it doesn't exist yet
		if(!System.IO.Directory.Exists(newPrefabDirectory))
		{
			System.IO.Directory.CreateDirectory(newPrefabDirectory);
			AssetDatabase.Refresh();
		}
		
		string msg = AssetDatabase.MoveAsset(dstPath, newPrefabPath);
		if(string.IsNullOrEmpty(msg))
		{
			_AssetPath = null;
			Debug.Log("Moved asset from path: " + dstPath + " to " + newPrefabPath);
		}
		else
			Debug.LogError("Failed to move asset : " + msg);
	}
	
	[MenuItem("Assets/Custom Move/To Corresponding Scripts Folder", true)]
	static bool CustomMoveAssetToScriptValidate ()
	{
		// Make sure it is selected first
		if(Selection.activeObject == null)
			return false;
		
		string path = AssetDatabase.GetAssetPath(Selection.activeObject);
		//Debug.Log(path);
		if(path.Contains("Assets/Sprites/") == false)
			return false;
		
		// Only custom move for .prefab files
		string fileExt = System.IO.Path.GetExtension(path);
		//Debug.Log(fileExt);
		if(fileExt.Equals(".cs") == false)
			return false;
		
		return true;
	}

}
