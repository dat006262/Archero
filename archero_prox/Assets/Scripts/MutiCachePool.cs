




using System.Collections.Generic;
using UnityEngine;

public class MutiCachePool<T> where T : Component
{
	private LocalUnityObjctPool mPool;

	private T tempitem;

	private List<T> mUsed = new List<T>();

	private List<T> mCached = new List<T>();

	public void Init(GameObject obj, GameObject copyitem)
	{
		mPool = LocalUnityObjctPool.Create(obj);
		mPool.CreateCache<T>(copyitem);
	}

	public T get()
	{
		if (mCached.Count > 0)
		{
			tempitem = mCached[0];
			tempitem.gameObject.SetActive(value: true);
			mCached.RemoveAt(0);
		}
		else
		{
			tempitem = mPool.DeQueue<T>();
		}
		mUsed.Add(tempitem);
		return tempitem;
	}

	public void cache(T one)
	{
		mCached.Add(one);
		one.gameObject.SetActive(value: false);
	}

	public void clear()
	{
		Vector3 zero = Vector3.zero;
		int i = 0;
		for (int count = mUsed.Count; i < count; i++)
		{
			zero.x = GameLogic.Random(-5000, -10000);
			zero.y = GameLogic.Random(-50000, 50000);
			T val = mUsed[i];
			val.transform.localPosition = zero;
			mCached.Add(mUsed[i]);
		}
		mUsed.Clear();
	}

	public void hold(int count)
	{
		int i = count;
		for (int count2 = mCached.Count; i < count2; i++)
		{
			T val = mCached[i];
			val.gameObject.SetActive(value: false);
		}
	}
}
