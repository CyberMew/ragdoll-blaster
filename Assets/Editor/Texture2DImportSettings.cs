using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Reflection;

public class Texture2DImportSettings : AssetPostprocessor  {
	
	void OnPreprocessTexture()
	{
		if(assetPath.Contains(".png"))
		{
			TextureImporter textureImporter = assetImporter as TextureImporter;
			textureImporter.spritePixelsToUnits = 72f;
			textureImporter.filterMode = FilterMode.Point;
			textureImporter.wrapMode = TextureWrapMode.Clamp;
			int width = 0, height = 0;
			if(GetImageSize(textureImporter, out width, out height))
			{
				if(Mathf.Max(width, height) > 1280)
				{
					textureImporter.maxTextureSize = 2048;
				}
				else if(Mathf.Max(width, height) > 512)
				{
					textureImporter.maxTextureSize = 1024;
				}
				else if(Mathf.Max(width, height) > 256)
				{
					textureImporter.maxTextureSize = 512;
				}
				else if(Mathf.Max(width, height) > 128)
				{
					textureImporter.maxTextureSize = 256;
				}
				else if(Mathf.Max(width, height) > 64)
				{
					textureImporter.maxTextureSize = 128;
				}
				else if(Mathf.Max(width, height) > 32)
				{
					textureImporter.maxTextureSize = 64;
				}
				else
				{
					textureImporter.maxTextureSize = 32;
				}
			}
			if(assetPath.Contains("Assets/Sprites/Game/Backgrounds/"))
			{
				if(assetPath.EndsWith("ground.png"))
				{
					textureImporter.spritePivot = new Vector2(0.5f, 0f);
				//	Debug.Log("Ground texture detected, changing pivot point to bottom");
					//EditorUtility.SetDirty(assetImporter);
					//AssetDatabase.Refresh();
				}
			}
			if(System.IO.File.Exists(assetPath))
			{
				Debug.Log(".meta also generated before preprocess");
				
			}
		//	Debug.Log("Changing Texture2D import settings for: " + assetPath + ". TextureImporter name; " + textureImporter.name);
		}
	}

	void OnPostprocessTexture(Texture2D texture)
	{
		/*Debug.Log(texture.name);

		string texturePath = AssetDatabase.GetAssetPath(texture);
		Debug.Log(texturePath);
		TextureImporter textureImporter = (TextureImporter)AssetImporter.GetAtPath(texturePath);
		if(textureImporter && textureImporter.isReadable)
		{
			//have some fun!
			Debug.Log("Texture2d is readable!");
		}*/
		/*TextureImporter textureImporter = assetImporter as TextureImporter;
		int width = texture.width, height = texture.height;	// already processed, cannot use this!
		//GetImageSize(texture, out width, out height);
		if(width > 1280 || height > 1280)
		{
			textureImporter.maxTextureSize = 2048;
		}*/
		TextureImporter textureImporter = assetImporter as TextureImporter;
		//Debug.Log(textureImporter.spritePivot);
		//AssetDatabase.Refresh();


		int pos = textureImporter.assetPath.LastIndexOf("/");
		string dir = textureImporter.assetPath.Remove(pos + 1);
		//Debug.Log(dir);
		string filename = textureImporter.assetPath.Substring(pos + 1);
		filename = filename.Remove(filename.LastIndexOf("."));
		//Debug.Log(filename + " and " + texture.name);

		// Check if prefab already exists in destination folder
		if(AssetDatabase.LoadAssetAtPath(dir + filename + ".prefab", (typeof(GameObject))) as GameObject)
		{
			if(EditorUtility.DisplayDialog("Override existing prefab?",
			                            "Are you sure you want to create a new prefab? Existing prefabs instances in scene will be messed up!" +
			                            "\n\nSelect Do not Create if you have no idea what is going on", "Create Prefab", "Do Not Create"))
			{
				//CreatePrefab(dir, filename);
				/*http://docs.unity3d.com/Documentation/ScriptReference/PrefabUtility.CreateEmptyPrefab.html
	http://answers.unity3d.com/questions/437431/why-does-createemptyprefab-return-null.html
			http://answers.unity3d.com/questions/8633/how-do-i-programmatically-assign-a-gameobject-to-a.html
				http://answers.unity3d.com/questions/8633/how-do-i-programmatically-assign-a-gameobject-to-a.html*/
				//EditorUtility.SetDirty(assetImporter);
			}
		}
		else
		{
			// Check if meta file already exists before
			if(System.IO.File.Exists(dir + filename + ".png"))
			{
				Debug.Log("Old art assets detected! Skipping prefab generation. " + dir + filename + ".meta");
				
			}
			else

			{
			Debug.Log("Creating prefab at " + dir + filename + ".prefab");
			}
			//CreatePrefab (dir, filename);
		}
	}

	void CreatePrefab (string dir, string filename)
	{
		// Create and prepare your game object.
		GameObject go = new GameObject (filename);
		go.AddComponent<SpriteRenderer> ();
		// Create prefab from object.
		PrefabUtility.CreatePrefab (dir + filename + ".prefab", go, ReplacePrefabOptions.ConnectToPrefab);
		// or ReplacePrefabOptions.ReplaceNameBased?
		//Destroy GameObject
		GameObject.DestroyImmediate (go);
	}

	// http://forum.unity3d.com/threads/165295-Getting-original-size-of-texture-asset-in-pixels
	public static bool GetImageSize(TextureImporter importer, out int width, out int height)
	{
		if (importer != null)
		{
			object[] args = new object[2] { 0, 0 };
			MethodInfo mi = typeof(TextureImporter).GetMethod("GetWidthAndHeight", BindingFlags.NonPublic | BindingFlags.Instance);
			mi.Invoke(importer, args);
		
			width = (int)args[0];
			height = (int)args[1];

			return true;				
		}

		height = width = 0;
		
		return false;		
	}
}
