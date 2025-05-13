




using System.Collections.Generic;
using UnityEngine;

public class MapEffectManager
{
	private int perCount = 15;

	private Dictionary<string, Queue<GameObject>> mEffectList = new Dictionary<string, Queue<GameObject>>();

	private Dictionary<string, GameObject> mCloneList = new Dictionary<string, GameObject>();

	private Dictionary<string, List<GameObject>> mUseList = new Dictionary<string, List<GameObject>>();

	public GameObject Get(string key)
	{
		return Get(key, GameNode.m_PoolMapParent);
	}

	public void Cache(GameObject o)
	{
		Cache(o, GameNode.m_PoolMapParent);
	}

	public void Cache(GameObject o, Transform parent, bool useremove = true)
	{
		if (!o)
		{
			return;
		}
		string name = o.name;
		o.transform.SetParent(parent);
		o.SetActive(value: false);
		if (useremove)
		{
			UseRemove(o);
		}
		if (mEffectList.TryGetValue(name, out Queue<GameObject> value))
		{
			if (value.Count < perCount)
			{
				value.Enqueue(o);
			}
			else
			{
				UnityEngine.Object.Destroy(o);
			}
		}
		else
		{
			mEffectList.Add(name, new Queue<GameObject>());
			mEffectList[name].Enqueue(o);
		}
	}

	public GameObject Get(string key, Transform parent)
	{
		if (mEffectList.TryGetValue(key, out Queue<GameObject> value))
		{
			while (value.Count > 0)
			{
				GameObject gameObject = value.Dequeue();
				if ((bool)gameObject)
				{
					gameObject.SetActive(value: true);
					UseSet(key, gameObject);
					return gameObject;
				}
			}
		}
		GameObject clone = GetClone(key);
		if ((bool)clone)
		{
			clone.SetActive(value: true);
			GameObject gameObject = Object.Instantiate(clone);
			gameObject.name = key;
			gameObject.transform.SetParent(parent);
			gameObject.name = key;
			UseSet(key, gameObject);
			clone.SetActive(value: false);
			return gameObject;
		}
		return null;
	}

	public bool check_is_map_effect(GameObject o)
	{
		if (!o)
		{
			return false;
		}
		if (mUseList.TryGetValue(o.name, out List<GameObject> value) && value.Contains(o))
		{
			return true;
		}
		return false;
	}

	private void UseSet(string key, GameObject o)
	{
		string name = o.name;
		if (mUseList.TryGetValue(key, out List<GameObject> value))
		{
			value.Add(o);
			return;
		}
		mUseList.Add(key, new List<GameObject>());
		mUseList[key].Add(o);
	}

	private void UseRemove(GameObject o)
	{
		string name = o.name;
		if (mUseList.TryGetValue(name, out List<GameObject> value) && value.Contains(o))
		{
			value.Remove(o);
		}
	}

	private GameObject GetClone(string key)
	{
		if (mCloneList.TryGetValue(key, out GameObject value))
		{
			return value;
		}
		GameObject gameObject = ResourceManager.Load<GameObject>(key);
		if ((bool)gameObject)
		{
			value = Object.Instantiate(gameObject);
			value.name = key;
			value.transform.SetParent(GameNode.m_PoolMapParent);
			value.transform.position = new Vector3(10000f, 0f, 0f);
			value.name = key;
			mCloneList.Add(key, value);
		}
		return value;
	}

	public void Release()
	{
		MapCache();
		Dictionary<string, Queue<GameObject>>.Enumerator enumerator = mEffectList.GetEnumerator();
		while (enumerator.MoveNext())
		{
			Queue<GameObject> value = enumerator.Current.Value;
			while (value.Count > 0)
			{
				GameObject gameObject = value.Dequeue();
				if ((bool)gameObject)
				{
					UnityEngine.Object.Destroy(gameObject);
				}
			}
		}
		GameNode.m_PoolMapParent.DestroyChildren();
		mUseList.Clear();
		mEffectList.Clear();
		mCloneList.Clear();
	}

	public void Clear()
	{
		MapCache();
	}

	public void MapCache()
	{
		Dictionary<string, List<GameObject>>.Enumerator enumerator = mUseList.GetEnumerator();
		while (enumerator.MoveNext())
		{
			List<GameObject> value = enumerator.Current.Value;
			int i = 0;
			for (int count = value.Count; i < count; i++)
			{
				GameObject gameObject = value[i];
				if ((bool)gameObject)
				{
					Cache(gameObject, GameNode.m_PoolMapParent, useremove: false);
				}
			}
		}
	}
}
