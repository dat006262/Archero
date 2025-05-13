




using System.IO;
using UnityEngine;

public class UniWebViewHelper
{
	public static string StreamingAssetURLForPath(string path)
	{
		return Path.Combine("file:///android_asset/", path);
	}

	public static string PersistentDataURLForPath(string path)
	{
		return Path.Combine("file://" + Application.persistentDataPath, path);
	}
}
