




using System.Collections.Generic;
using UnityEngine;

public class HoldPoolManager
{
	private Dictionary<string, Queue<GameObject>> mEffectList = new Dictionary<string, Queue<GameObject>>();

	public GameObject Get(string key)
	{
		return Get(key, GameNode.SoundNode);
	}

	public void Cache(GameObject o)
	{
		Cache(o, GameNode.SoundNode);
	}

	public void Cache(GameObject o, Transform parent)
	{
		if ((bool)o)
		{
			string name = o.name;
			o.transform.SetParent(parent);
			o.SetActive(value: false);
			if (mEffectList.TryGetValue(name, out Queue<GameObject> value))
			{
				value.Enqueue(o);
				return;
			}
			mEffectList.Add(name, new Queue<GameObject>());
			mEffectList[name].Enqueue(o);
		}
	}

	public GameObject Get(string key, Transform parent)
	{
		GameObject gameObject;
		if (mEffectList.TryGetValue(key, out Queue<GameObject> value) && value.Count > 0)
		{
			gameObject = value.Dequeue();
			if ((bool)gameObject)
			{
				gameObject.SetActive(value: true);
				return gameObject;
			}
		}
		gameObject = Object.Instantiate(ResourceManager.Load<GameObject>(key));
		gameObject.transform.SetParent(parent);
		gameObject.name = key;
		return gameObject;
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
