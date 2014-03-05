using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Reflection;
using System.IO;

public class Texture2DImportSettings : AssetPostprocessor  {

	private static EditorApplication.CallbackFunction createPrefabCB;

	void OnPreprocessTexture()
	{
		// We don't want to preprocess it again when user switch platforms
		if(assetImporter.userData.Contains("loadedBefore"))
		{
			Debug.Log("Texture already processed, skipping custom processing for: " + assetPath);
		}
		else if(assetPath.EndsWith(".png"))
		{
			assetImporter.userData += "loadedBefore|";

			TextureImporter textureImporter = assetImporter as TextureImporter;
			textureImporter.spritePixelsToUnits = 72f;
			textureImporter.filterMode = FilterMode.Point;
			textureImporter.wrapMode = TextureWrapMode.Clamp;
			textureImporter.spriteImportMode = SpriteImportMode.Single;
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
			if(assetPath.Contains("Assets/Sprites/Game/Levels/Ground/"))
			{
				textureImporter.spritePivot = new Vector2(0.5f, 0f);
			}
		}		
	}

	void OnPostprocessTextureDONOTCALLTHIS(Texture2D texture)
	{
		Debug.LogError("aerdere");
		if(!assetImporter.userData.Equals("loadedBefore"))
		{
			// Totally done with custom processing
			// Set userdata to loadBefore
			assetImporter.userData = "loadedBefore";	// we check this to make sure we don't overwrite existing changes
			
//			AssetDatabase.SaveAssets();
			EditorUtility.SetDirty(assetImporter);
			AssetDatabase.Refresh();	// This will trigger the whole postprocess on the same asset once again - not good. But we have no choice.
			
		
			TextureImporter textureImporter = assetImporter as TextureImporter;
			string fullPath = textureImporter.assetPath;

			// Getting the full path to the asset, but just the folder path
			int pos = fullPath.LastIndexOf("/");
			string fullPrefabDirectory = fullPath.Remove(pos + 1);

			// Get just the filename (excluding extension)
			string filename = fullPath.Substring(pos + 1);
			filename = filename.Remove(filename.LastIndexOf("."));
			// todo: we might have to make use of these function if this were to work on MacOS?
			//string fileName = Path.GetFileNameWithoutExtension(fullPath);

			string fullPrefabPath = fullPrefabDirectory + filename + ".prefab";
//			AssetDatabase.SaveAssets();
			EditorUtility.SetDirty(assetImporter);
			AssetDatabase.Refresh();	// This will trigger the whole postprocess on the same asset once again - not good. But we have no choice.

			// Check if prefab already exists in destination folder
			if(AssetDatabase.LoadAssetAtPath(fullPrefabPath, (typeof(GameObject))) as GameObject)
			{
				Debug.Log("Prefab already exists! " + fullPrefabPath);
				if(EditorUtility.DisplayDialog("Override existing prefab?",
				                            "Are you sure you want to create a new prefab? Existing prefabs instances in scene will be messed up!" +
				                            "\n\nSelect Do not Create if you have no idea what is going on", "Overwrite Prefab", "Cancel Action"))
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
				{
					if(EditorUtility.DisplayDialog("Create prefab from asset",
					                               "Do you want to create a new prefab automatically for asset?\n\n" +
					                               fullPrefabPath + 
					                               "\n\nSelect \"Do not Create\" if you have no idea.",
					                               "Create Prefab", "Do Not Create"))
					{

						// Replace first occurance of Sprites to Prefabs
						//string newPrefabDirectory = 
						//pos = fullPrefabDirectory.IndexOf("Assets/Sprites/");
						string newPrefabDirectory = fullPrefabDirectory.Replace("Assets/Sprites/", "Assets/Prefabs/");
						string newPrefabPath = newPrefabDirectory + filename + ".prefab";
						if(EditorUtility.DisplayDialog("Move prefab?",
						                               "Do you want to move the newly created prefab from\n\"" + fullPrefabPath + "\"\nto\n\"" + newPrefabPath + "\"\n?",
						                               "Yes please", "No, leave it as it is"))
						{
							if(!Directory.Exists(newPrefabDirectory))
							{
								Directory.CreateDirectory(newPrefabDirectory);
								// Making sure the folder shows up in Project panel
								// This will import any assets that have changed their content modification data or have been added-removed to the project folder.
								AssetDatabase.Refresh();	// This will trigger the whole postprocess on the same asset once again - not good. But we have no choice.
							}
							//Debug.Log("Created prefab: " + newPrefabPath);
							// EditorApplication.delayCall += CreatePrefab(AssetDatabase.LoadAssetAtPath(fullPath, typeof(Texture2D)) as Texture2D, newPrefabPath);
						}
						else
						{
							Debug.Log("Created prefab: " + fullPrefabPath);
							//CreatePrefab(AssetDatabase.LoadAssetAtPath(fullPath, typeof(Texture2D)) as Texture2D, fullPrefabPath);

//							createPrefabCB = new EditorApplication.CallbackFunction(createPrefabCB, );

//							EditorApplication.delayCall += createPrefabCB;
						}

						AssetDatabase.Refresh();
					}
				}
			}
		}
	}
	/// <summary>
	/// Handles when ANY asset is imported, deleted, or moved.  Each parameter is the full path of the asset, including filename and extension.
	/// </summary>
	/// <param name="importedAssets">The array of assets that were imported.</param>
	/// <param name="deletedAssets">The array of assets that were deleted.</param>
	/// <param name="movedAssets">The array of assets that were moved.  These are the new file paths.</param>
	/// <param name="movedFromPath">The array of assets that were moved.  These are the old file paths.</param>
	private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromPath)
	{
		foreach(string asset in importedAssets)
		{
			//Debug.Log("Imported: " + asset);
			CreatePrefab(asset);
		}

		/*
		foreach (string asset in deletedAssets)
		{
			Debug.Log("Deleted: " + asset);
		}
		
		for (int i = 0; i < movedAssets.Length; i++ )
		{
			Debug.Log("Moved: from " + movedFromPath[i] + " to " + movedAssets[i]);
		}*/
	}

	static void CreatePrefab(string fullPath)
	{
		// We do not want to process non png files
		if(!fullPath.EndsWith(".png"))
		{
			return;
		}
		// We do not want to prompt user again to create with each reimport
		AssetImporter ai = AssetImporter.GetAtPath(fullPath);
		if(ai.userData.Contains("prefabPrompted"))
		{
			// Check if user was prompted to create prefab before
			return;
		}
		ai.userData += "prefabPrompted|";
		Debug.LogWarning("Sanity check for 'prefabPrompted' string: " + AssetImporter.GetAtPath(fullPath).userData);

		Texture2D texture = Resources.LoadAssetAtPath(fullPath, typeof(Texture2D)) as Texture2D;
		if(texture == null)
		{
			Debug.LogError("Unable to load texture for prefab creation!");
			return;
		}
		Sprite sprite = Resources.LoadAssetAtPath(fullPath, typeof(Sprite)) as Sprite;
		if(sprite == null)
		{
			Debug.LogError("Unable to load sprite for prefab creation!");
			return;
		}

		int pos = fullPath.LastIndexOf("/");
		string fullDirectory = fullPath.Remove(pos + 1);
		string fileName = Path.GetFileNameWithoutExtension(fullPath);
		string fullWithoutExtension = fullDirectory + fileName;

		// Ask if they want the prefab to be created
		if(EditorUtility.DisplayDialog("Create prefab from asset",
		                               "Do you want to create a new prefab automatically for asset:\n\n" +
		                               fullPath + 
		                               "\n\nSelect \"Do not Create\" if you have no idea.",
		                               "Create Prefab", "Do Not Create"))
		{
			// Create prefab at Sprites folder
			string fullPrefabPath = fullWithoutExtension + ".prefab";
			GeneratePrefab(texture, fullPrefabPath);

			string newPrefabDirectory = fullDirectory.Replace("Assets/Sprites/", "Assets/Prefabs/");
			string newPrefabPath = newPrefabDirectory + fileName + ".prefab";
			if(EditorUtility.DisplayDialog("Move prefab?",
			                               "Do you want to move the newly created prefab from\n\"" + fullPrefabPath + "\"\nto\n\"" + newPrefabPath + "\"\n?",
			                               "Yes please", "No, leave it as it is"))
			{
				// Create directory if it doesn't exist yet
				if(!Directory.Exists(newPrefabDirectory))
				{
					Directory.CreateDirectory(newPrefabDirectory);
				}
				// Shift created prefab
				AssetDatabase.MoveAsset(fullPrefabPath, newPrefabPath);
			}
		}


	}

	static void GeneratePrefab(Texture2D texture, string fullPath)
	{
		Debug.LogWarning("Sanity check for 'texture name' string: " + texture.name);
		// Create and prepare your game object.
		GameObject go = new GameObject(fullPath);
		EditorUtility.SetDirty(go);
		SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
		sr.sortingLayerName = "GameObjects";
		
		Sprite tmpSprite = new Sprite();

		if(fullPath.Contains("Levels/Ground"))
		{
			tmpSprite = Sprite.Create(texture, new Rect(0,0,texture.width,texture.height), new Vector2 (0.5f, 0f));
			go.transform.position = new Vector3(0f, -5f, go.transform.position.z);
			go.tag = "Obstacle";
		}
		else
		{
			tmpSprite = Sprite.Create(texture, new Rect(0,0,texture.width,texture.height), new Vector2 (0.5f, 0.0f));
		}

		// FK UPPPPPPP HEREE HOWOWOWHWHOWOW
		EditorUtility.SetDirty(tmpSprite);
		sr.sprite = tmpSprite;
		sr.sprite = Resources.LoadAssetAtPath(fullPath, typeof(Sprite)) as Sprite;

		// Check if the object is obstacle
		if(fullPath.Contains("Levels/Obstacles"))
		{
			go.AddComponent<PolygonCollider2D>();
			go.tag = "Obstacle";
		}

		// Check if the object is decoration
		if(fullPath.Contains("Levels/Decorations"))
		{
			sr.sortingLayerName = "Decorations";
			sr.sortingOrder = -3;
		}
		/*
		SpriteRenderer rr = go.GetComponent<SpriteRenderer>();
		rr.sprite = new Sprite()
		Debug.Log(texture.name + " " + go.GetComponent<SpriteRenderer>().sprite.name);
*/
		EditorUtility.SetDirty(go);

		// Create prefab from object.
		EditorUtility.SetDirty(PrefabUtility.CreatePrefab(fullPath, go, ReplacePrefabOptions.ConnectToPrefab));
		// or ReplacePrefabOptions.ReplaceNameBased?

		//Destroy GameObject
		GameObject.DestroyImmediate (go);

		AssetDatabase.Refresh();
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
