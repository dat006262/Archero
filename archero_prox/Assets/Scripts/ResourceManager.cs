




using DG.Tweening;
using Dxx;
using Dxx.Util;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ResourceManager
{
	private class ResourceData
	{
		public DxxSpriteAtlas atlas;

		public float time;
	}

	private const string ATLAS_PATH = "Atlas";

	private const float remove_time = 300f;

	private static Dictionary<string, ResourceData> mAtlasList = new Dictionary<string, ResourceData>();

	private static StringBuilder strTemp = new StringBuilder();

	private static Dictionary<string, AssetBundle> bundleDict = new Dictionary<string, AssetBundle>();

	private static DxxSpriteAtlas GetAtlas(string name)
	{
		if (mAtlasList.TryGetValue(name, out ResourceData value))
		{
			if (value.atlas != null)
			{
				value.time = Time.realtimeSinceStartup;
			}
			else
			{
				value.atlas = Load<DxxSpriteAtlas>(name);
				value.time = Time.realtimeSinceStartup;
			}
		}
		else
		{
			value = new ResourceData();
			value.atlas = Load<DxxSpriteAtlas>(name);
			value.time = Time.realtimeSinceStartup;
			mAtlasList.Add(name, value);
		}
		return value.atlas;
	}

	public static void Init()
	{
		DOTween.Sequence().AppendInterval(33f).AppendCallback(delegate
		{
			Dictionary<string, ResourceData>.Enumerator enumerator = mAtlasList.GetEnumerator();
			do
			{
				if (!enumerator.MoveNext())
				{
					return;
				}
			}
			while (!(Time.realtimeSinceStartup - enumerator.Current.Value.time > 300f) || !(enumerator.Current.Value.atlas != null));
			mAtlasList.Remove(enumerator.Current.Key);
		})
			.SetLoops(-1)
			.SetUpdate(isIndependentUpdate: true);
	}

	public static T Load<T>(string path) where T : UnityEngine.Object
	{
		return Resources.Load<T>(path);
	}

	public static bool TryLoad<T>(string path, out T t) where T : UnityEngine.Object
	{
		T val = Resources.Load<T>(path);
		if ((bool)(UnityEngine.Object)val)
		{
			t = val;
			return true;
		}
		t = (T)null;
		return false;
	}

	public static bool TryLoad(string path)
	{
		UnityEngine.Object x = Resources.Load(path);
		return x != null;
	}

	public static Sprite GetSprite(string atlasName, string spriteName)
	{
		atlasName = atlasName.ToLower();
		strTemp.Clear();
		strTemp.AppendFormat("{0}/{1}", "Atlas", atlasName);
		string text = strTemp.ToString();
		DxxSpriteAtlas atlas = GetAtlas(text);
		if (atlas == null)
		{
			SdkManager.Bugly_Report("ResourceManager.GetSprite", Utils.FormatString("GetAtlas[{0}] is not found!", text));
		}
		Sprite sprite = null;
		try
		{
			sprite = atlas.GetSprite(spriteName);
			return sprite;
		}
		catch
		{
			SdkManager.Bugly_Report("ResourceManager.GetSprite", Utils.FormatString("GetAtlas[{0}] try sprite:{1} is not found!", text, spriteName));
		}
		if (sprite == null)
		{
			bool flag = true;
			if (spriteName != null && spriteName == "element1201")
			{
				flag = false;
			}
			if (flag)
			{
				SdkManager.Bugly_Report("ResourceManager.GetSprite", Utils.FormatString("GetAtlas[{0}] sprite:{1} is not found!", text, spriteName));
			}
		}
		return sprite;
	}

	public static void UnloadUnused()
	{
		ClearPool();
		Resources.UnloadUnusedAssets();
	}

	protected static void ClearPool()
	{
		GameObjectPool.Clear();
	}

	public static void Release(UnityEngine.Object assetToUnload)
	{
		Resources.UnloadAsset(assetToUnload);
	}

	private static AssetBundle GetAssetBundle(string bundlePath)
	{
		if (!bundleDict.ContainsKey(bundlePath))
		{
			AssetBundle value = AssetBundle.LoadFromFile(bundlePath);
			bundleDict.Add(bundlePath, value);
		}
		return bundleDict[bundlePath];
	}

	public static void LoadAnsyc<T>(string path, Action<T> onLoadComplete) where T : UnityEngine.Object
	{
		GameObject gameObject = new GameObject(Utils.FormatString("ResourceManager.LoadAnsyc[{0}]", path));
	}

	public static void UnloadBundleAsset(UnityEngine.Object assetToUnload)
	{
	}
}
