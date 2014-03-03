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
				//if(assetPath.EndsWith("ground.png"))
				{
					textureImporter.spritePivot = new Vector2(0.5f, 0f);
					Debug.Log("Ground texture detected, changing pivot point to bottom");
					EditorUtility.SetDirty(assetImporter);
					AssetDatabase.Refresh();
				}
			}
			Debug.Log("Changing Texture2D import settings for: " + assetPath + ". TextureImporter name; " + textureImporter.name);
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
		Debug.Log(textureImporter.spritePivot);
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
