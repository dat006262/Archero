




using Dxx.Util;
using System.Collections.Generic;
using UnityEngine;

public class GoodsCreateManager
{
	private Dictionary<int, Queue<GameObject>> mList = new Dictionary<int, Queue<GameObject>>();

	private List<GameObject> mUseList = new List<GameObject>();

	public GameObject Get(int id)
	{
		if (mList.TryGetValue(id, out Queue<GameObject> value))
		{
			while (value.Count > 0)
			{
				GameObject gameObject = value.Dequeue();
				if ((bool)gameObject)
				{
					gameObject.SetActive(value: true);
					UseAdd(gameObject);
					return gameObject;
				}
			}
		}
		GameObject gameObject2 = Object.Instantiate(ResourceManager.Load<GameObject>(Utils.GetString("Game/Goods/GoodsCreate", id)));
		gameObject2.name = id.ToString();
		UseAdd(gameObject2);
		return gameObject2;
	}

	public void Cache(GameObject o)
	{
		o.SetActive(value: false);
		int key = int.Parse(o.name);
		o.transform.SetParent(GameNode.m_BulletParent);
		UseRemove(o);
		if (mList.TryGetValue(key, out Queue<GameObject> value))
		{
			value.Enqueue(o);
			return;
		}
		mList.Add(key, new Queue<GameObject>());
		mList[key].Enqueue(o);
	}

	private void UseAdd(GameObject o)
	{
		mUseList.Add(o);
	}

	private void UseRemove(GameObject o)
	{
		mUseList.Remove(o);
	}

	public void Release()
	{
		int i = 0;
		for (int count = mUseList.Count; i < count; i++)
		{
			GameObject gameObject = mUseList[i];
			if ((bool)gameObject)
			{
				UnityEngine.Object.Destroy(gameObject);
			}
		}
		mUseList.Clear();
		mList.Clear();
	}
}
