




using System.Collections.Generic;
using UnityEngine;

public class LocalUnityObjctPool : MonoBehaviour
{
	protected class Cache
	{
		public GameObject copyItem;

		private List<GameObject> collection = new List<GameObject>();

		private Queue<GameObject> cache = new Queue<GameObject>();

		private Transform rootParent;

		public Cache(Transform rootParent, GameObject copyItem)
		{
			this.rootParent = rootParent;
			this.copyItem = copyItem;
		}

		public void EnQueue(GameObject item)
		{
			item.gameObject.SetActive(value: false);
			item.transform.SetParent(rootParent.transform);
			cache.Enqueue(item);
			if (collection.Contains(item))
			{
				collection.Remove(item);
			}
		}

		public GameObject Dequeue()
		{
			return Dequeue(string.Empty);
		}

		public GameObject Dequeue(string name)
		{
			GameObject gameObject = (cache.Count <= 0) ? UnityEngine.Object.Instantiate(copyItem) : cache.Dequeue();
			collection.Add(gameObject);
			if (!string.IsNullOrEmpty(name))
			{
				gameObject.name = name;
			}
			gameObject.SetActive(value: true);
			return gameObject;
		}

		public void Collect()
		{
			for (int num = collection.Count - 1; num >= 0; num--)
			{
				EnQueue(collection[num]);
			}
			collection.Clear();
		}

		public void Destroy()
		{
			Collect();
			while (cache.Count > 0)
			{
				UnityEngine.Object.DestroyImmediate(cache.Dequeue());
			}
		}
	}

	protected Dictionary<string, Cache> m_Cache = new Dictionary<string, Cache>();

	public static LocalUnityObjctPool Create(GameObject parent)
	{
		GameObject gameObject = new GameObject("LocalPool");
		gameObject.transform.SetParent(parent.transform, worldPositionStays: false);
		LocalUnityObjctPool result = gameObject.AddComponent<LocalUnityObjctPool>();
		gameObject.SetActive(value: false);
		return result;
	}

	public void CreateCache<T>(GameObject copyItem) where T : Component
	{
		CreateCache<T>(base.gameObject, copyItem);
	}

	public void CreateCache<T>(GameObject parent, GameObject copyItem) where T : Component
	{
		CreateCache(typeof(T).Name, parent.transform, copyItem);
	}

	public T DeQueue<T>() where T : Component
	{
		return DeQueue<T>(typeof(T).Name);
	}

	public T DeQueueWithName<T>(string name) where T : Component
	{
		return DeQueueWithName<T>(typeof(T).Name, name);
	}

	public void EnQueue<T>(GameObject item) where T : Component
	{
		EnQueue(typeof(T).Name, item);
	}

	public void Collect<T>() where T : Component
	{
		Collect(typeof(T).Name);
	}

	public void ClearCache<T>() where T : Component
	{
		ClearCache(typeof(T).Name);
	}

	public void CreateCache(string cacheName, GameObject copyItem)
	{
		CreateCache(cacheName, base.transform, copyItem);
	}

	public void CreateCache(string cacheName, Transform parent, GameObject copyItem)
	{
		m_Cache.Add(cacheName, new Cache(parent, copyItem));
	}

	public T DeQueue<T>(string cacheName) where T : Component
	{
		return m_Cache[cacheName].Dequeue().GetComponent<T>();
	}

	public T DeQueueWithName<T>(string cacheName, string name) where T : Component
	{
		return m_Cache[cacheName].Dequeue(name).GetComponent<T>();
	}

	public void EnQueue(string cacheName, GameObject item)
	{
		if (m_Cache.ContainsKey(cacheName))
		{
			m_Cache[cacheName].EnQueue(item);
		}
		else
		{
			Debugger.Log("enqueue " + cacheName + " dont have");
		}
	}

	public void Collect(string cacheName)
	{
		if (m_Cache.ContainsKey(cacheName))
		{
			m_Cache[cacheName].Collect();
		}
	}

	public void ClearCache(string cacheName)
	{
		if (m_Cache.ContainsKey(cacheName))
		{
			m_Cache[cacheName].Destroy();
		}
	}

	public void ClearAllCache()
	{
		foreach (Cache value in m_Cache.Values)
		{
			value.Destroy();
		}
		m_Cache.Clear();
	}
}
