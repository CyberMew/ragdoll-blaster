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
			LogWarning("Texture already processed, skipping custom processing for: " + assetPath);
			// I could also do something like this to prevent overriding my manual settings
			/*Object asset = AssetDatabase.LoadAssetAtPath(importer.assetPath, typeof(Texture2D));
			if (asset)
			{
				EditorUtility.SetDirty(asset);
				return;
			}*/
		}
		else if(assetPath.EndsWith(".png"))
		{
			assetImporter.userData += "loadedBefore|";

			TextureImporter textureImporter = assetImporter as TextureImporter;
			
			TextureImporterSettings tis = new TextureImporterSettings();
			textureImporter.ReadTextureSettings(tis);

			// Custom settings
			//tis.spriteImportMode = SpriteImportMode.Single;
			tis.spriteMode = 1;
			tis.spritePixelsToUnits = 72f;
			tis.spritePivot = new Vector2(0.5f, 0.5f);
			tis.spriteExtrude = 1;
			//Center = 0, TopLeft = 1, TopCenter = 2, TopRight = 3, LeftCenter = 4, RightCenter = 5, BottomLeft = 6, BottomCenter = 7, BottomRight = 8, Custom = 9.
			//tis.spriteAlignment = SpriteAlignment.BottomCenter;

			//tis.spriteMeshType = SpriteMeshType.Tight;
			tis.filterMode = FilterMode.Point;
			tis.wrapMode = TextureWrapMode.Clamp;
			textureImporter.textureType = TextureImporterType.Sprite;

			int width = 0, height = 0;
				
			if(GetImageSize(textureImporter, out width, out height))
			{
				if(Mathf.Max(width, height) > 1280)
				{
					tis.maxTextureSize = 2048;
				}
				else if(Mathf.Max(width, height) > 512)
				{
					tis.maxTextureSize = 1024;
				}
				else if(Mathf.Max(width, height) > 256)
				{
					tis.maxTextureSize = 512;
				}
				else if(Mathf.Max(width, height) > 128)
				{
					tis.maxTextureSize = 256;
				}
				else if(Mathf.Max(width, height) > 64)
				{
					tis.maxTextureSize = 128;
				}
				else if(Mathf.Max(width, height) > 32)
				{
					tis.maxTextureSize = 64;
				}
				else
				{
					tis.maxTextureSize = 32;
				}
			}
			if(assetPath.Contains("Assets/Sprites/Game/Levels/Ground/"))
			{
				//textureImporter.spritePivot = new Vector2(0.5f, 0f);
				tis.spriteAlignment = 7;
			}
			//TextureImporter haha = AssetImporter.GetAtPath(assetPath) as TextureImporter;
			//Debug.Log(haha.ToString());
			textureImporter.SetTextureSettings(tis);
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

		int pos = fullPath.LastIndexOf("/");
		string fullDirectory = fullPath.Remove(pos + 1);
		string fileName = Path.GetFileNameWithoutExtension(fullPath);
		//string fullWithoutExtension = fullDirectory + fileName;

		// Ask if they want the prefab to be created
		if(EditorUtility.DisplayDialog("Create prefab from asset",
		                               "Do you want to create a new prefab automatically for asset:\n\n" +
		                               fullPath + 
		                               "\n\nSelect \"Do not Create\" if you have no idea.",
		                               "Create Prefab", "Do Not Create"))
		{
			// Create prefab at Sprites folder
			GeneratePrefab(fullPath);

			string fullPrefabPath = fullPath.Replace(".png", ".prefab");//fullWithoutExtension + ".prefab";
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

				string ret;
				// Shift created prefab
				ret = AssetDatabase.MoveAsset(fullPrefabPath, newPrefabPath);
				if(ret == "")
				{
					Debug.Log("Asset moved to " + newPrefabPath);
				}
				else
				{
					Debug.Log(ret);
				}
			}
		}
		else
		{
			// Log this down in console when user cancels
			Debug.Log("Current cancelled. Asset meta userData for " + fullPath + ": " + ai.userData);
		}
	}

	static void GeneratePrefab(string fullPath)
	{
		// Create and prepare your game object.
		GameObject go = new GameObject(fullPath);

		// All of them will have a SpriteRenderer
		SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
		sr.sortingLayerName = "GameObjects";
		//sr.sprite = Resources.LoadAssetAtPath(omg, typeof(Sprite)) as Sprite;
		sr.sprite = AssetDatabase.LoadAssetAtPath(fullPath, typeof(Sprite)) as Sprite;
		//sr.sprite = Sprite.Create(testing, new Rect(0,0,testing.width,testing.height), new Vector2(0.5f, 0f));
		//sr.sprite = Sprite.Create(sr.sprite.texture, new Rect(0,0,sr.sprite.texture.width,sr.sprite.texture.height), new Vector2(0.5f, 0f));
		
		if(fullPath.Contains("Sprites/Game/GUI"))
		{
			sr.sortingLayerName = "GUI";
		}

		if(fullPath.Contains("Game/Levels/Ground"))
		{
			go.transform.position = new Vector3(0f, -5f, go.transform.position.z);
			go.tag = "Obstacle";
			go.layer = LayerMask.NameToLayer("Obstacle");
			go.AddComponent<PolygonCollider2D>();
			sr.sortingLayerName = "GUI";
		}

		// Check if the object is obstacle
		if(fullPath.Contains("Game/Levels/Obstacles"))
		{
			go.AddComponent<PolygonCollider2D>();
			go.tag = "Obstacle";
			go.layer = LayerMask.NameToLayer("Obstacle");
		}

		// Check if the object is decoration
		if(fullPath.Contains("Game/Levels/Decorations"))
		{
			sr.sortingLayerName = "Decorations";
			sr.sortingOrder = -3;
		}
		
		string fullPrefabPath = fullPath.Replace(".png", ".prefab");//fullWithoutExtension + ".prefab";
		// Create prefab from object.
		PrefabUtility.CreatePrefab(fullPrefabPath, go, ReplacePrefabOptions.ConnectToPrefab);
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
