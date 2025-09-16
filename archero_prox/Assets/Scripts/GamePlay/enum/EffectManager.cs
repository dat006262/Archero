




using DG.Tweening;
using Dxx.Util;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager
{
	private Dictionary<string, int> mEffectCounts = new Dictionary<string, int>
	{
		{
			"Game/Food/3001",
			20
		}
	};

	private int perCount = 999;

	private int stCount = 5;

	private const int RemoveTime = 190;

	private const int RemoveMoreTime = 10;

	private float checkTime;

	private Dictionary<string, Queue<GameObject>>.Enumerator iter;

	private Queue<GameObject> iterq;

	private GameObject iterobj;

	private float itertime;

	private string iterkey;

	private int itertimecount;

	private int stCountTemp;

	private Sequence seq_update;

	private Dictionary<string, Queue<GameObject>> mEffectList = new Dictionary<string, Queue<GameObject>>();

	private Dictionary<string, GameObject> mCloneList = new Dictionary<string, GameObject>();

	private Dictionary<string, float> mTimeList = new Dictionary<string, float>();

	public EffectManager()
	{
		KillSequence();
		seq_update = DOTween.Sequence().AppendInterval(1.4f).AppendCallback(OnUpdate)
			.SetLoops(-1);
	}

	private void KillSequence()
	{
		if (seq_update != null)
		{
			seq_update.Kill();
			seq_update = null;
		}
	}

	private void OnUpdate()
	{
		if (!(Time.time - checkTime > 3.1f))
		{
			return;
		}
		checkTime = Time.time;
		iter = mEffectList.GetEnumerator();
		while (iter.MoveNext())
		{
			iterkey = iter.Current.Key;
			itertime = GetLastUseTime(iterkey);
			if (!mEffectCounts.TryGetValue(iterkey, out stCountTemp))
			{
				stCountTemp = stCount;
			}
			if (mEffectList.TryGetValue(iterkey, out iterq) && Time.time - itertime > 190f)
			{
				RemoveMoreEffects(iterq, stCountTemp);
				if (Time.time - itertime > 200f)
				{
					itertimecount = (int)((Time.time - itertime - 190f - 10f) / 10f);
					itertimecount = stCountTemp - itertimecount;
					itertimecount = MathDxx.Clamp(itertimecount, 0, itertimecount);
					RemoveMoreEffects(iterq, itertimecount);
				}
			}
		}
	}

	private void RemoveMoreEffects(Queue<GameObject> iterq, int lastcount)
	{
		while (iterq.Count > lastcount)
		{
			iterobj = iterq.Dequeue();
			if ((bool)iterobj)
			{
				UnityEngine.Object.Destroy(iterobj);
			}
		}
	}

	public GameObject Get(string key)
	{
		return Get(key, GameNode.m_PoolParent);
	}

	public void Cache(GameObject o)
	{
		Cache(o, GameNode.m_PoolParent);
	}

	public void Cache(GameObject o, Transform parent)
	{
		if (!o)
		{
			return;
		}
		string name = o.name;
		o.transform.SetParent(parent);
		o.SetActive(value: false);
		if (mEffectList.TryGetValue(name, out Queue<GameObject> value))
		{
			if (!value.Contains(o))
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
					SetLastUseTime(key);
					gameObject.SetActive(value: true);
					gameObject.transform.position = new Vector3(10000f, 0f, 0f);
					return gameObject;
				}
			}
		}
		GameObject clone = GetClone(key);
		if ((bool)clone)
		{
			SetLastUseTime(key);
			GameObject gameObject = Object.Instantiate(clone);
			gameObject.name = key;
			gameObject.transform.SetParent(parent);
			gameObject.transform.position = new Vector3(10000f, 0f, 0f);
			gameObject.SetActive(value: true);
			return gameObject;
		}
		return null;
	}

	private GameObject GetClone(string key)
	{
		if (mCloneList.TryGetValue(key, out GameObject value))
		{
			return value;
		}
		GameObject gameObject = ResourceManager.Load<GameObject>(key);
		mCloneList.Add(key, gameObject);
		return gameObject;
	}

	private float GetLastUseTime(string key)
	{
		float value = 0f;
		if (mTimeList.TryGetValue(key, out value))
		{
			return value;
		}
		mTimeList.Add(key, value);
		return value;
	}

	private void SetLastUseTime(string key)
	{
		if (mTimeList.ContainsKey(key))
		{
			mTimeList[key] = Time.time;
		}
		else
		{
			mTimeList.Add(key, Time.time);
		}
	}

	public void Release()
	{
		KillSequence();
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
		GameNode.m_PoolParent.DestroyChildren();
		mEffectList.Clear();
		mCloneList.Clear();
		mTimeList.Clear();
	}
}
