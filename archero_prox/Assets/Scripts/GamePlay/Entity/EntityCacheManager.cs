




using System.Collections.Generic;
using UnityEngine;

public class EntityCacheManager
{
	private Dictionary<string, Queue<GameObject>> mEffectList = new Dictionary<string, Queue<GameObject>>();

	public void Cache(GameObject o, int maxcount)
	{
		if (!o)
		{
			return;
		}
		string name = o.name;
		o.transform.SetParent(GameNode.m_PoolParent);
		o.SetActive(value: false);
		if (mEffectList.TryGetValue(name, out Queue<GameObject> value))
		{
			if (value.Count < maxcount)
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

	public GameObject Get(string key)
	{
		if (mEffectList.TryGetValue(key, out Queue<GameObject> value))
		{
			while (value.Count > 0)
			{
				GameObject gameObject = value.Dequeue();
				if ((bool)gameObject)
				{
					gameObject.SetActive(value: true);
					gameObject.transform.position = new Vector3(10000f, 0f, 0f);
					return gameObject;
				}
			}
		}
		GameObject gameObject2 = ResourceManager.Load<GameObject>(key);
		if ((bool)gameObject2)
		{
			GameObject gameObject = Object.Instantiate(gameObject2);
			gameObject.name = key;
			return gameObject;
		}
		return null;
	}

	public void Release()
	{
		Dictionary<string, Queue<GameObject>>.Enumerator enumerator = mEffectList.GetEnumerator();
		while (enumerator.MoveNext())
		{
			Queue<GameObject> value = enumerator.Current.Value;
			while (value.Count > 0)
			{
				UnityEngine.Object.Destroy(value.Dequeue());
			}
		}
		mEffectList.Clear();
	}
}
