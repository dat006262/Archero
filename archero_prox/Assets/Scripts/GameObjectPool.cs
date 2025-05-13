




using Dxx;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : SingletonMono<GameObjectPool>
{
	protected class PoolCache : UnityObjectPool<GameObject>
	{
		protected GameObject origin;

		protected Transform cacheParent;

		public PoolCache(Transform cacheParent)
			: base((Func<GameObject>)null, (Action<GameObject>)null, (Action<GameObject>)null)
		{
			this.cacheParent = cacheParent;
			m_actionCreate = CreateNew;
			m_ActionOnGet = OnGet;
			m_ActionOnRelease = OnRelease;
		}

		public virtual GameObject CreateNew()
		{
			return UnityEngine.Object.Instantiate(origin);
		}

		public virtual void OnGet(GameObject obj)
		{
			obj.SetActive(value: true);
			obj.transform.SetParent(null);
		}

		public virtual void OnRelease(GameObject obj)
		{
			obj.SetActive(value: false);
			obj.transform.SetParent(cacheParent);
		}

		public virtual void OnDestroy()
		{
			while (m_Stack.Count > 0)
			{
				UnityEngine.Object.Destroy(m_Stack.Pop());
			}
		}
	}

	protected class CustomPool : PoolCache
	{
		public CustomPool(Transform cacheParent, GameObject origin)
			: base(cacheParent)
		{
			base.origin = origin;
		}
	}

	protected class ResourcePool : PoolCache
	{
		private GameObject res;

		public ResourcePool(Transform cacheParent, string resPath)
			: base(cacheParent)
		{
			res = ResourceManager.Load<GameObject>(resPath);
			origin = UnityEngine.Object.Instantiate(res);
		}

		public override void OnDestroy()
		{
			if (origin != null)
			{
				UnityEngine.Object.Destroy(origin);
				res = null;
			}
			base.OnDestroy();
		}
	}

	protected static Dictionary<string, PoolCache> cacheDict = new Dictionary<string, PoolCache>();

	public static bool HasPool(string path)
	{
		return cacheDict.ContainsKey(path);
	}

	public static void CreatePool(string path)
	{
		if (!cacheDict.ContainsKey(path))
		{
			cacheDict.Add(path, new ResourcePool(SingletonMono<GameObjectPool>.Instance.trans, path));
		}
	}

	public static void CreatePool(string poolKey, GameObject obj)
	{
		if (!cacheDict.ContainsKey(poolKey))
		{
			cacheDict.Add(poolKey, new CustomPool(SingletonMono<GameObjectPool>.Instance.trans, obj));
		}
	}

	public static GameObject Instantiate(string resPath)
	{
		if (!HasPool(resPath))
		{
			CreatePool(resPath);
		}
		return Get(resPath);
	}

	public static GameObject Get(string poolKey)
	{
		if (cacheDict.ContainsKey(poolKey))
		{
			return cacheDict[poolKey].Get();
		}
		return null;
	}

	public static bool Release(string poolKey, GameObject obj)
	{
		if (cacheDict.ContainsKey(poolKey))
		{
			cacheDict[poolKey].Release(obj);
			return true;
		}
		return false;
	}

	public static void DeletePool(string path)
	{
		if (cacheDict.ContainsKey(path))
		{
			cacheDict[path].OnDestroy();
			cacheDict.Remove(path);
		}
	}

	public static void Clear()
	{
		Dictionary<string, PoolCache>.Enumerator enumerator = cacheDict.GetEnumerator();
		List<string> list = ListPool<string>.Get();
		while (enumerator.MoveNext())
		{
			list.Add(enumerator.Current.Key);
		}
		for (int i = 0; i < list.Count; i++)
		{
			DeletePool(list[i]);
		}
		ListPool<string>.Release(list);
	}
}
