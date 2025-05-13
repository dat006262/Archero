




using DG.Tweening;
using Dxx.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager
{
	public Transform parent;

	private Dictionary<int, Queue<GameObject>> mBulletList = new Dictionary<int, Queue<GameObject>>();

	private Dictionary<int, GameObject> mCloneList = new Dictionary<int, GameObject>();

	private Action OnCache;

	private Dictionary<int, float> mTimeList = new Dictionary<int, float>();

	private Dictionary<int, int> mBulletCounts = new Dictionary<int, int>
	{
		{
			5092,
			20
		}
	};

	private int mType;

	private const int perCount = 99;

	private int stCount = 5;

	private const int RemoveTime = 180;

	private float checkTime;

	private int stCountTemp;

	private Dictionary<int, Queue<GameObject>>.Enumerator iter;

	private Queue<GameObject> iterq;

	private GameObject iterobj;

	private float itertime;

	private int iterkey;

	private int iterindex;

	private int itermax;

	private int itertimecount;

	private Sequence seq_update;

	public BulletManager(int type)
	{
		mType = type;
		if (mType == 1)
		{
			stCount = 15;
		}
		KillSequence();
		seq_update = DOTween.Sequence().AppendInterval(1.3f).AppendCallback(OnUpdate)
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
		if (!(Time.time - checkTime > 3f))
		{
			return;
		}
		checkTime = Time.time;
		iter = mBulletList.GetEnumerator();
		while (iter.MoveNext())
		{
			iterkey = iter.Current.Key;
			if (!mBulletCounts.TryGetValue(iterkey, out stCountTemp))
			{
				stCountTemp = stCount;
			}
			itertime = GetLastUseTime(iterkey);
			iterq = iter.Current.Value;
			if (iterq != null && Time.time - itertime > 180f)
			{
				RemoveMoreBullets(iterq, stCountTemp);
				if (Time.time - itertime > 540f)
				{
					itertimecount = (int)((Time.time - itertime - 540f) / 180f);
					itertimecount = stCountTemp - itertimecount;
					itertimecount = MathDxx.Clamp(itertimecount, 0, itertimecount);
					RemoveMoreBullets(iterq, itertimecount);
				}
			}
		}
	}

	private void RemoveMoreBullets(Queue<GameObject> q, int holdcount)
	{
		while (q.Count > holdcount)
		{
			iterobj = q.Dequeue();
			if ((bool)iterobj)
			{
				BulletBase component = iterobj.GetComponent<BulletBase>();
				OnCache = (Action)Delegate.Remove(OnCache, new Action(component.BulletCache));
				UnityEngine.Object.Destroy(iterobj);
			}
		}
	}

	public GameObject Get(int bulletID)
	{
		if (mBulletList.TryGetValue(bulletID, out Queue<GameObject> value))
		{
			while (value.Count > 0)
			{
				GameObject gameObject = value.Dequeue();
				if ((bool)gameObject)
				{
					SetLastUseTime(bulletID);
					OnCache = (Action)Delegate.Combine(OnCache, new Action(gameObject.GetComponent<BulletBase>().BulletCache));
					gameObject.SetActive(value: true);
					return gameObject;
				}
			}
		}
		SetLastUseTime(bulletID);
		GameObject clone = GetClone(bulletID);
		GameObject gameObject2 = UnityEngine.Object.Instantiate(clone);
		OnCache = (Action)Delegate.Combine(OnCache, new Action(gameObject2.GetComponent<BulletBase>().BulletCache));
		return gameObject2;
	}

	public void Cache(int bulletID, GameObject o)
	{
		o.SetActive(value: false);
		o.transform.SetParent(parent);
		if (mBulletList.TryGetValue(bulletID, out Queue<GameObject> value))
		{
			if (value.Contains(o))
			{
				return;
			}
			if (value.Count >= 99)
			{
				OnCache = (Action)Delegate.Remove(OnCache, new Action(o.GetComponent<BulletBase>().BulletCache));
				UnityEngine.Object.Destroy(o);
				return;
			}
			value.Enqueue(o);
		}
		else
		{
			value = new Queue<GameObject>();
			mBulletList.Add(bulletID, value);
			value.Enqueue(o);
		}
		OnCache = (Action)Delegate.Remove(OnCache, new Action(o.GetComponent<BulletBase>().BulletCache));
	}

	public void CacheAll()
	{
		if (OnCache != null)
		{
			OnCache();
		}
	}

	public BulletSlopeBase CreateSlopeBullet(EntityBase entity, int BulletID, Vector3 startpos, Vector3 endpos)
	{
		BulletBase bulletBase = CreateBullet(entity, BulletID, startpos, 0f);
		BulletSlopeBase bulletSlopeBase = bulletBase as BulletSlopeBase;
		bulletSlopeBase.SetEndPos(endpos);
		return bulletSlopeBase;
	}

	public BulletBase CreateBullet(EntityBase entity, int BulletID, Vector3 pos, float rota)
	{
		return CreateBulletInternal(entity, BulletID, pos, rota, clear: true);
	}

	public BulletBase CreateBulletInternal(EntityBase entity, int BulletID, Vector3 pos, float rota, bool clear)
	{
		Transform transform = GameLogic.BulletGet(BulletID).transform;
		transform.localRotation = Quaternion.Euler(0f, rota, 0f);
		transform.SetParent(GameNode.m_PoolParent);
		transform.position = pos;
		transform.localScale = Vector3.one;
		BulletBase component = transform.GetComponent<BulletBase>();
		component.Init(entity, BulletID);
		BulletTransmit bulletAttribute = new BulletTransmit(entity, BulletID, clear);
		component.SetBulletAttribute(bulletAttribute);
		return component;
	}

	public BulletBase CreateCallBullet(EntityBase entity, int BulletID, int callid, Vector3 startpos, Vector3 endpos)
	{
		Transform transform = GameLogic.BulletGet(BulletID).transform;
		float angle = Utils.getAngle(endpos - startpos);
		transform.localRotation = Quaternion.Euler(0f, angle, 0f);
		transform.SetParent(GameNode.m_PoolParent);
		transform.position = startpos;
		transform.localScale = Vector3.one;
		BulletCallBase component = transform.GetComponent<BulletCallBase>();
		component.Init(entity, BulletID);
		BulletTransmit bulletAttribute = new BulletTransmit(entity, BulletID, clear: true);
		component.SetBulletAttribute(bulletAttribute);
		component.SetTarget(null);
		component.SetEndPos(endpos);
		return component;
	}

	public BulletBase CreateBullet(EntityBase entity, int BulletID)
	{
		Transform transform = GameLogic.BulletGet(BulletID).transform;
		BulletBase component = transform.GetComponent<BulletBase>();
		component.Init(entity, BulletID);
		return component;
	}

	private GameObject GetClone(int key)
	{
		if (mCloneList.TryGetValue(key, out GameObject value))
		{
			return value;
		}
		GameObject gameObject = ResourceManager.Load<GameObject>(Utils.GetString("Game/Bullet/Bullet", key));
		mCloneList.Add(key, gameObject);
		return gameObject;
	}

	private float GetLastUseTime(int key)
	{
		float value = 0f;
		if (mTimeList.TryGetValue(key, out value))
		{
			return value;
		}
		mTimeList.Add(key, value);
		return value;
	}

	private void SetLastUseTime(int key)
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
		CacheAll();
		OnCache = null;
		Dictionary<int, Queue<GameObject>>.Enumerator enumerator = mBulletList.GetEnumerator();
		while (enumerator.MoveNext())
		{
			Queue<GameObject> value = enumerator.Current.Value;
			while (value.Count > 0)
			{
				UnityEngine.Object.Destroy(value.Dequeue());
			}
		}
		parent.DestroyChildren();
		mCloneList.Clear();
		mBulletList.Clear();
	}
}
