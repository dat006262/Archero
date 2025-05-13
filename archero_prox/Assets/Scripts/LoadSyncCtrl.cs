




using Dxx.Util;
using System;
using System.Collections;
using UnityEngine;

public class LoadSyncCtrl : MonoBehaviour
{
	private bool bComplete;

	public static LoadSyncCtrl Load<T>(string path, Action<T> complete) where T : UnityEngine.Object
	{
		if (complete == null)
		{
			return null;
		}
		GameObject gameObject = new GameObject(Utils.FormatString("LoadSyncCtrl.Load[{0}]", path));
		LoadSyncCtrl loadSyncCtrl = gameObject.AddComponent<LoadSyncCtrl>();
		loadSyncCtrl.LoadInternal(path, complete);
		return loadSyncCtrl;
	}

	public void LoadInternal<T>(string path, Action<T> complete) where T : UnityEngine.Object
	{
		StartCoroutine(LoadIE(path, complete));
	}

	private IEnumerator LoadIE<T>(string path, Action<T> complete) where T : UnityEngine.Object
	{
		ResourceRequest request = Resources.LoadAsync<GameObject>(path);
		yield return request;
		T o = request.asset as T;
		bComplete = true;
		complete(o);
		DeInit();
	}

	public void DeInit()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
